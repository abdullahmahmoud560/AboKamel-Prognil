# AboKamel API

**Status: Production Ready (Security Audit Completed: June 2026)**

---

## Project Overview

AboKamel API is a secure, high-performance ASP.NET Core 9 API with MySQL 8.0 backend, optimized for scalability and production use. The system features role-based access control, data isolation, and composite database indexes for fast query performance.

---

## Security Highlights

- **Role-Based Access Control (RBAC)** - Enforces `[Authorize(Roles = "SuperAdmin")]` on all sensitive dashboard and admin endpoints
- **IDOR Prevention** - Uses `IHttpContextAccessor` to extract current user from JWT claims, ensuring data isolation
- **Input Validation** - Enhanced file upload validation with:
  - Allowed extensions (`.jpg`, `.jpeg`, `.png`, `.webp`)
  - Content-type verification
  - 5MB size limit
  - Filename sanitization to prevent path traversal
- **Container Security** - Localhost-only port bindings, version-pinned dependencies, and persistent volumes

---

## Infrastructure & Deployment

### Docker Compose Stack

The project uses Docker Compose for consistent, reproducible deployments:
- **Persistent Volumes**: `mysql_data` for database, `images_data` for uploaded images
- **Version Pinning**: MySQL pinned to `8.0`
- **Security**: All ports bound to `127.0.0.1` (no public exposure)

### Quick Start

1. Create an `.env` file with required environment variables
2. Run the stack:
   ```bash
   docker-compose up -d
   ```

---

## Database Optimization

- **Composite Indexes**: Added for high-traffic query patterns on:
  - `Favorites (UserId, ProductId)`
  - `Notifications (CustomerId, AreaId, CreatedDate)`
  - `Orders (Status, CreatedDate)`
  - `Addresses (CustomerId, IsPrimary)`

---

## Built With

- **.NET 9** - Modern ASP.NET Core API framework
- **MySQL 8.0** - Relational database for production
- **Docker & Docker Compose** - Containerization and orchestration

---

## License

[Placeholder]
