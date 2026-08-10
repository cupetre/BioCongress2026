# BioCongress Architecture

BioCongress is a medical congress and event-management website built as a clean modular monolith:

- One Angular frontend
- One ASP.NET Core Web API
- One PostgreSQL database
- Managed cloud infrastructure for scale, reliability, storage, and monitoring

This shape is intentionally boring in the best way. It keeps development fast for a small team while still letting the API scale horizontally during registration launches.

## Local Development

Each developer runs the same application stack locally:

- Angular app in `frontend/icof-web`
- ASP.NET Core API in `backend/Icof.Api`
- PostgreSQL through Docker Compose at the repository root

Local databases are not shared. Schema changes are shared through EF Core migrations committed to Git.

## Backend Boundaries

The API uses controllers, DTOs, services, EF Core entities, and ASP.NET Core Identity.

Initial domain areas:

- Users and roles
- Events
- Event registrations
- Team members
- Editable page content
- Site settings

Event registration is treated as a high-risk business flow. The database enforces one registration per user/event, and the service performs capacity changes atomically inside a transaction so concurrent requests cannot overbook an event.

## Authentication

Authentication is based on ASP.NET Core Identity. Passwords are hashed by Identity and never stored directly.

The system must support:

- Account registration
- Login/logout
- Email confirmation
- Password recovery
- User/Admin roles
- Account status
- Server-side authorization for admin routes and protected API actions

## Production Shape

The intended production path:

```text
Domain -> Cloudflare -> Angular frontend / ASP.NET API -> PostgreSQL
```

The ASP.NET API should remain stateless so Azure Container Apps can run multiple replicas during launch periods. PostgreSQL will use Azure Database for PostgreSQL Flexible Server. PgBouncer can be enabled for high-traffic registration launches if load tests show connection pressure.

Images, event banners, videos, PDFs, and downloadable assets should live in Azure Blob Storage. PostgreSQL stores identifiers, paths, and metadata, not the binary files.

## Admin System

The admin dashboard should allow authorized admins to manage:

- Events and registrations
- Team members and bios
- Editable public website content
- Users and roles
- Site settings
- Media references stored in Blob Storage

The goal is to avoid source-code edits for routine website updates after launch.

## Two-Week Build Direction

Recommended order:

1. Lock backend entities, migrations, Identity, and registration rules.
2. Convert supplied HTML/CSS designs into Angular layouts and reusable components.
3. Build public pages: Home, About, Team, Events, Event Details, Contact.
4. Add auth/profile pages and connect them to the API.
5. Build admin CRUD screens and protect them by role.
6. Add media storage integration.
7. Prepare staging environment, monitoring, and load tests.
8. Run k6 launch simulations and tune replica/database settings.

Designs can evolve during the build, but Angular should keep shared layout, typography, spacing, buttons, cards, and page sections reusable so visual changes are not painful.
