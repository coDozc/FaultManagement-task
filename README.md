# Fault Management System

Hata bildirim yönetim sistemi. .NET 8 ve ASP.NET Core ile geliştirilmiş, JWT tabanlı kimlik doğrulaması ve durum makinesine sahip REST API.

## Teknoloji Stack

- **.NET 8** - Application framework
- **ASP.NET Core 8** - Web API
- **Entity Framework Core 8.0.12** - ORM
- **SQL Server** - Database
- **JWT Bearer** - Authentication (HS256)
- **Serilog** - Logging
- **BCrypt.Net-Next** - Password hashing
- **Swagger/OpenAPI** - API documentation

## Proje Mimarisi

Beş katmanlı mimariye sahip:

```
FaultManagement.Domain/
  ├── Entities (AppUser, FaultNotification)
  ├── Enums (UserRole, PriorityLevel, FaultStatus)
  ├── Exceptions (InvalidStatusTransitionException)
  └── StateMachines (FaultStatusStateMachine)

FaultManagement.Application/
  └── DTOs (Data Transfer Objects)

FaultManagement.Infrastructure/
  ├── Data (DbContext, Migrations)
  └── Dependencies (EF Core configuration)

FaultManagement.Api/
  ├── Controllers (AuthController, FaultsController)
  ├── Middleware (GlobalExceptionMiddleware, ApiResponse)
  ├── Services (TokenService)
  └── Configuration (JwtSettings)

## Kurulum

### Ön Koşullar

- .NET 8 SDK
- SQL Server (Windows Authentication)
- Visual Studio 2022 / VS Code

### Adımlar

1. **Repository'yi klonlayın:**
   ```bash
   git clone <repo-url>
   cd FaultManagement
   ```

2. **Bağlantı Stringini Düzenleyin** (isteğe bağlı):
   - `appsettings.json` dosyasında `DefaultConnection` güncelle:
   ```json
   "DefaultConnection": "Server=your-server;Database=LotusTaskDb;Integrated Security=true;TrustServerCertificate=true;"
   ```

3. **Projeyi Çalıştırın:**
   ```bash
   dotnet build
   dotnet run --project FaultManagement.Api
   ```

4. **Migration Otomatik Uygulanır:**
   - Program.cs'de `db.Database.Migrate()` çağrısı startup'ta veritabanını hazırlar
   - Admin ve normal kullanıcı otomatik oluşturulur

## API Endpoints

### Kimlik Doğrulama

**POST** `/api/auth/login`
```json
{
  "userName": "admin",
  "password": "admin123"
}
```
**Response:** JWT Token

---

### Fault Notifications

**POST** `/api/faults` - Yeni hata oluştur (Authorized)
```json
{
  "title": "Server bağlantısı hatası",
  "description": "Veritabanı sunucusuna bağlanılamıyor",
  "location": "Istanbul",
  "priority": 3
}
```

**GET** `/api/faults` - Hatalar listele (Authorized)
- Query params: `page`, `pageSize`, `status`, `priority`, `location`, `sortBy`
- Admin tüm hataları görebilir
- User sadece kendi hatasını görebilir

**GET** `/api/faults/{id}` - Belirli hatayı getir (Authorized)

**PATCH** `/api/faults/{id}/status` - Durum değiştir (Admin Only)
```json
{
  "newStatus": 2
}
```

## Kimlik Doğrulama

### Seed Users

| UserName | Password  | Role  |
|----------|-----------|-------|
| admin    | admin123  | Admin |
| user     | user123   | User  |

### JWT Token

- **Algorithm:** HS256
- **Expiration:** 60 dakika (özelleştirebilir)
- **Issuer:** FaultManagement
- **Audience:** FaultManagementUsers

Swagger'da "Authorize" butonundan token gir:
```
***Token girmeden önce bir boşluk bırakılmalı.

Bearer eyJhbGciOiJIUzI1NiIs...
```

## Durum Makinesi (State Machine)

Hata bildirimleri aşağıdaki state geçişlerine uyar:

```
New ──────┬──→ UnderReview ──→ Assigned ──→ InProgress ──→ Completed
          │
          └──────────────────────────→ Cancelled
```

- **New (1):** İlk durum, sadece Assigned veya UnderReview'e geçebilir
- **UnderReview (2):** Assigned veya Cancelled'e geçebilir
- **Assigned (3):** InProgress veya Cancelled'e geçebilir  
- **InProgress (4):** Completed veya Cancelled'e geçebilir
- **Completed (5):** Terminal durum (geçiş yok)
- **Cancelled (6):** Terminal durum (geçiş yok)

Geçersiz geçişler `422 Unprocessable Entity` döner.

## İş Kuralları

1. **1 Saatin İçinde Aynı Şehirde Tekrar Hata Oluşturulamaz:**
   - Aynı user aynı location'a son 1 saatte hata oluşturmışsa, yeni hata 400 Bad Request döner

2. **Role-Based Access Control:**
   - `/api/auth/login` - Herkes
   - `/api/faults` - Authorized users
   - `/api/faults/{id}/status` - Admin only

3. **Durum Geçişi Doğrulama:**
   - FaultStatusStateMachine geçerli geçişleri kontrol eder
   - Geçersiz geçiş InvalidStatusTransitionException fırlatır

## Test Etme

### Swagger UI
```
http://localhost:5155/swagger/index.html
```
Swagger'da "Authorize" butonundan token gir:
```
***Token girmeden önce bir boşluk bırakılmalı.

### cURL Örneği

**Login:**
```bash
curl -X POST http://localhost:5155/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"userName":"admin","password":"admin123"}'
```

**Hata Oluştur:**
```bash
curl -X POST http://localhost:5155/api/faults \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{
    "title":"Network timeout",
    "description":"API response slow",
    "location":"Ankara",
    "priority":2
  }'
```

**Hatalar Listele:**
```bash
curl -X GET "http://localhost:5155/api/faults?page=1&pageSize=10" \
  -H "Authorization: Bearer <TOKEN>"
```

**Durum Değiştir (Admin):**
```bash
curl -X PATCH http://localhost:5155/api/faults/1/status \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"newStatus":2}'
```

## Veritabanı

### Şema

**Users Table**
- Id (int, Identity)
- UserName (nvarchar(100), Unique)
- Email (nvarchar(255), Unique)
- PasswordHash (nvarchar(max))
- Role (int: 1=Admin, 2=User)
- CreatedAtUtc (datetime2)

**FaultNotifications Table**
- Id (int, Identity)
- Title (nvarchar(200))
- Description (nvarchar(2000))
- Location (nvarchar(500))
- Priority (int: 1=Low, 2=Medium, 3=High)
- Status (int: 1=New, 2=UnderReview, 3=Assigned, 4=InProgress, 5=Completed, 6=Cancelled)
- CreatedByUserId (int, Foreign Key → Users.Id)
- CreatedAtUtc (datetime2)
- UpdatedAtUtc (datetime2, nullable)

### Indexes
- Users.Email (Unique)
- Users.UserName (Unique)
- FaultNotifications.Status
- FaultNotifications.Priority
- FaultNotifications.CreatedByUserId

## Günlükleme

Serilog ile logging yapılıyor:
- **Console:** İnstant output
- **File:** `logs/log-*.txt` (günlük rotating)

Minimum Level: **Information**

## Hata Yönetimi

Global Exception Middleware HTTP status codes'u döner:

| Exception | Status Code |
|-----------|------------|
| InvalidStatusTransitionException | 422 |
| UnauthorizedAccessException | 403 |
| Diğer exceptions | 500 |

Response format:
```json
{
  "success": false,
  "data": null,
  "message": "Error message",
  "errors": ["Detail 1", "Detail 2"]
}
```

## Özelleştirme

### JWT Ayarları (`appsettings.json`)

```json
"Jwt": {
  "SecretKey": "your-secret-key-min-32-chars",
  "Issuer": "FaultManagement",
  "Audience": "FaultManagementUsers",
  "ExpirationMinutes": 60
}
```

### Bağlantı Stringi

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DB;..."
}
```

## Bilinen Sınırlamalar

- Email verification yapılmıyor
- Password reset özelliği yok
- Real-time notification yok

## Lisans

MIT