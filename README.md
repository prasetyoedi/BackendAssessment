# Backend Assessment - .NET 8

**Token Kandidat:** `VEH-EDI_BACKEND`

## 🚀 Cara Menjalankan

### Prasyarat
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) atau SQL Server LocalDB
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (disarankan) atau IDE C# lainnya

### Langkah-langkah

1. **Clone repositori**
   ```bash
   git clone <repository-url>
   cd BackendAssessment
2. **Restore & Build proyek**
   ```bash
   dotnet restore 
   dotnet build
3. **Terapkan migrasi database**
   ```bash
   dotnet ef database update --context AppDbContext --project BackendAssessment.Persistence --startup-project BackendAssessment.API
4. **Jalankan API**
   ```bash
   cd BackendAssessment.API
   dotnet run
5. **Buka Swagger UI**
   ```bash
   http://localhost:5091/swagger

6. **Menjalankan Test**
   ```bash
   dotnet test

## Teknologi yang Digunakan

| Komponen | Teknologi |
|---|---|
| Runtime | .NET 8 |
| Framework | ASP.NET Core Web API |
| ORM | Entity Framework Core 8 |
| Database | SQL Server / SQL Server LocalDB |
| Testing | xUnit |
| Dokumentasi API | Swagger / OpenAPI |
| Version Control | Git |

### Lokasi Script SQL

Semua query untuk **Case 3** tersedia di:
```text
case3.sql
```

## 📝 Referensi

- [Dokumentasi Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Dokumentasi ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
- [xUnit Testing Framework](https://xunit.net/)