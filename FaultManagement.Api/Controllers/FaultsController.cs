using FaultManagement.Api.Middleware;
using FaultManagement.Application.DTOs;
using FaultManagement.Domain.Entities;
using FaultManagement.Domain.Exceptions;
using FaultManagement.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FaultManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FaultsController : ControllerBase
{
    private readonly FaultManagementDbContext _context;
    private readonly ILogger<FaultsController> _logger;

    public FaultsController(FaultManagementDbContext context, ILogger<FaultsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FaultNotificationDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateFault([FromBody] CreateFaultNotificationDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());

        _logger.LogInformation("Creating fault for user: {UserId}", userId);

        // Check 1-hour rule
        var lastFaultSameLocation = await _context.FaultNotifications
            .Where(f => f.Location == dto.Location && f.CreatedByUserId == userId)
            .OrderByDescending(f => f.CreatedAtUtc)
            .FirstOrDefaultAsync();

        if (lastFaultSameLocation != null && 
            (DateTime.UtcNow - lastFaultSameLocation.CreatedAtUtc).TotalHours < 1)
        {
            _logger.LogWarning("User {UserId} attempted to create duplicate fault in same location within 1 hour", userId);
            return BadRequest(ApiResponse.ErrorResponse("Cannot create another fault for the same location within 1 hour."));
        }

        var fault = new FaultNotification(
            dto.Title,
            dto.Description,
            dto.Location,
            dto.Priority,
            userId);

        _context.FaultNotifications.Add(fault);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Fault created successfully: {FaultId}", fault.Id);

        var resultDto = MapToDto(fault);
        return CreatedAtAction(nameof(GetFault), new { id = fault.Id }, ApiResponse<FaultNotificationDto>.SuccessResponse(resultDto, "Fault created successfully."));
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListFaults([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
        [FromQuery] int? status = null, [FromQuery] int? priority = null, [FromQuery] string? location = null,
        [FromQuery] string? sortBy = "createdAt")
    {
        _logger.LogInformation("Fetching faults - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        var query = _context.FaultNotifications.AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(f => (int)f.Status == status.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(f => (int)f.Priority == priority.Value);
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(f => f.Location.Contains(location));
        }

        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        if (userRole == "User")
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            query = query.Where(f => f.CreatedByUserId == userId);
        }

        query = sortBy?.ToLower() == "priority" 
            ? query.OrderByDescending(f => f.Priority) 
            : query.OrderByDescending(f => f.CreatedAtUtc);

        var total = await query.CountAsync();
        var faults = await query.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = faults.Select(MapToDto).ToList();

        return Ok(ApiResponse<object>.SuccessResponse(new
        {
            data = dtos,
            pagination = new { page, pageSize, total }
        }, "Faults retrieved successfully."));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<FaultNotificationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFault(Guid id)
    {
        var fault = await _context.FaultNotifications.FindAsync(id);

        if (fault == null)
        {
            return NotFound(ApiResponse.ErrorResponse("Fault not found."));
        }

        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());

        if (userRole == "User" && fault.CreatedByUserId != userId)
        {
            return Forbid();
        }

        var dto = MapToDto(fault);
        return Ok(ApiResponse<FaultNotificationDto>.SuccessResponse(dto));
    }

    [HttpPatch("{id}/status")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ApiResponse<FaultNotificationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeFaultStatusDto dto)
    {
        var fault = await _context.FaultNotifications.FindAsync(id);

        if (fault == null)
        {
            return NotFound(ApiResponse.ErrorResponse("Fault not found."));
        }

        try
        {
            fault.ChangeStatus(dto.NewStatus);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Status changed for fault {FaultId} to {NewStatus}", id, dto.NewStatus);

            var resultDto = MapToDto(fault);
            return Ok(ApiResponse<FaultNotificationDto>.SuccessResponse(resultDto, "Status updated successfully."));
        }
        catch (InvalidStatusTransitionException ex)
        {
            return UnprocessableEntity(ApiResponse.ErrorResponse(ex.Message, new List<string> { ex.Message }));
        }
    }

    private static FaultNotificationDto MapToDto(FaultNotification fault)
    {
        return new FaultNotificationDto
        {
            Id = fault.Id,
            Title = fault.Title,
            Description = fault.Description,
            Location = fault.Location,
            Priority = fault.Priority,
            Status = fault.Status,
            CreatedByUserId = fault.CreatedByUserId,
            CreatedAtUtc = fault.CreatedAtUtc,
            UpdatedAtUtc = fault.UpdatedAtUtc
        };
    }
}
