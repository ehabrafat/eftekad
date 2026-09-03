# Eftekad Web API

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or any code editor

## Getting Started

### 1. Configure Database Connection

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=MyApiDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

For SQL Server Authentication:
```
"Server=localhost\SQLEXPRESS;Database=eftekad;Trusted_Connection=True;TrustServerCertificate=true;"
```

### 2. Apply Migrations

Open terminal/command prompt in the project root and run:

```bash
dotnet ef database update
```

Or using Package Manager Console in Visual Studio:
```powershell
Update-Database
```

### 3. Run the Application

```bash
dotnet run
```

Access Swagger UI for API documentation:
- `https://localhost:5001/swagger`

---

## Quick Start Commands

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Apply migrations
dotnet ef database update

# Run the app
dotnet run
```
