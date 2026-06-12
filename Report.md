# 🚀 AboKamel Backend Deployment Report
---

## 1. 🌐 Domain & DNS Configuration
Accessed the cPanel dashboard to configure the abokamel domain.

Performed necessary updates in the Zone Editor to configure DNS records (A Record) and point the domain directly to the project's VPS.

---

## 2. 📋 Project Overview
A comprehensive environment was set up using **Docker** and **Docker Compose** to ensure the API services, database, and phpMyAdmin operate in a secure, interconnected environment within a single virtual network.

---

## 3. 🐳 Container Architecture
Three primary services were defined in the `docker-compose.yml` file:

| Service | Description |
|---------|-------------|
| **abokamel-mysql** | The MySQL database service, configured to run on the project's internal network with healthcheck enabled to ensure service readiness before API startup |
| **abokamel-api** | The Backend application container (ASP.NET Core), built using a custom Dockerfile and linked to the database |
| **phpmyadmin** | The database management tool, connected to the project network for direct data access |

abokamel-mysql: The MySQL database service, configured to run on the project's internal network with healthcheck enabled to ensure service readiness before API startup.

abokamel-api: The Backend application container (ASP.NET Core), built using a custom Dockerfile and linked to the database.

phpmyadmin: The database management tool, connected to the project network for direct data access.

---

## 4. 🔒 Network & Security Settings
### Network Isolation
- Created a custom bridge network named `abokamel-net` to ensure services communicate internally only

Port Binding:

Database: Bound to 127.0.0.1:3306 to prevent direct external access.

API: Bound to 127.0.0.1:5000 for security.

phpMyAdmin ports are kept closed when not in use to minimize the attack surface.

---

## 5. 💾 Database Configuration
### Database Schema & Access
- **Database Name**: abokamel
- **Application Database User**: abokamel_app (with least-privilege permissions: SELECT, INSERT, UPDATE, DELETE on application tables only)
- **Secrets Management**: All database credentials (${DB_USER}, ${DB_PASSWORD}) are injected via environment variables, with no secrets stored in source control or documentation.

### Persistence
- **Named Volumes**: `mysql_data` ensures data persistence across container restarts and recreations.

---

## 6. ✅ Current System Status
All containers are running efficiently (Up and Running).

Verified open ports using the `ss -tulpn` command, confirming that sensitive services operate on Localhost only.

---

## 7. Backup System

The VPS runs an automated backup routine that protects both **application data** (MySQL) and **static assets** (product images and uploaded files). Backups are created on the host and kept outside the Docker volumes so they survive container recreation.

### Overview

On each run, the backup script performs two independent tasks:

1. **Database backup** — exports the `abokamel` database from the `abokamel-mysql` container.
2. **Files backup** — archives the API image/upload directories into a compressed archive.

Both outputs are timestamped, deduplicated against the previous run, and subject to a **7-day retention** policy.

### MySQL backup (`mysqldump`)

The database dump is produced by executing `mysqldump` **inside** the running MySQL container, which avoids installing MySQL client tools on the host and uses the same network and credentials as the live service.
