using System.Collections.Concurrent;
using FaultManagement.Api.Middleware;
using Microsoft.Extensions.Options;

namespace FaultManagement.Api.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly RateLimitOptions _options;
    private static readonly ConcurrentDictionary<string, RequestLog> RequestLogs = new();

    public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger, IOptions<RateLimitOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ipAddress = GetClientIp(context);
        
        if (string.IsNullOrEmpty(ipAddress))
        {
            ipAddress = "unknown";
        }

        var key = ipAddress;
        var now = DateTime.UtcNow;
        
        if (RequestLogs.Count > 10000)
        {
            var oldEntries = RequestLogs.Where(x => (now - x.Value.LastResetTime).TotalMinutes > 5).Select(x => x.Key).ToList();
            foreach (var oldKey in oldEntries)
            {
                RequestLogs.TryRemove(oldKey, out _);
            }
        }

        var requestLog = RequestLogs.GetOrAdd(key, _ => new RequestLog { LastResetTime = now });

        if ((now - requestLog.LastResetTime).TotalSeconds >= _options.WindowInSeconds)
        {
            requestLog.Count = 0;
            requestLog.LastResetTime = now;
        }

        requestLog.Count++;

        if (requestLog.Count > _options.RequestLimit)
        {
            _logger.LogWarning("Rate limit exceeded for IP: {IpAddress}, Requests: {Count}/{Limit}", 
                ipAddress, requestLog.Count, _options.RequestLimit);
            
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.ContentType = "application/json";
            
            var response = new ApiResponse
            {
                Success = false,
                Message = ("Rate limit exceeded. Maximum {_options.RequestLimit} requests per {_options.WindowInSeconds} seconds.", _options.RequestLimit, _options.WindowInSeconds),
                Errors = new List<string> { "Too many requests" }
            };
            
            await context.Response.WriteAsJsonAsync(response);
            return;
        }

        await _next(context);
    }

    private string GetClientIp(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            var ips = forwardedFor.ToString().Split(',');
            if (ips.Length > 0 && !string.IsNullOrEmpty(ips[0]))
            {
                return ips[0].Trim();
            }
        }

        if (context.Request.Headers.TryGetValue("CF-Connecting-IP", out var cloudflareIp))
        {
            return cloudflareIp.ToString();
        }

        if (context.Request.Headers.TryGetValue("X-Real-IP", out var realIp))
        {
            return realIp.ToString();
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}

public class RateLimitOptions
{
    public int RequestLimit { get; set; } = 100;
    public int WindowInSeconds { get; set; } = 60;
}

public class RequestLog
{
    public int Count { get; set; }
    public DateTime LastResetTime { get; set; }
}
