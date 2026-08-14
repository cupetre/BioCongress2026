# BioCongress (ICOF)

Website for ICOF — a student medical congress — built as a clean modular monolith: one Angular frontend, one ASP.NET Core Web API, one PostgreSQL database.

## Tech stack

- **Frontend:** Angular 22 (standalone, SSR-ready via `@angular/ssr`), TypeScript
- **Backend:** ASP.NET Core Web API (.NET 10), EF Core, ASP.NET Core Identity
- **Database:** PostgreSQL 18 (via Docker Compose locally, Azure Database for PostgreSQL in production)
- **Infra target:** Cloudflare → Angular / ASP.NET API → PostgreSQL, deployed on Azure Container Apps

See [`docs/architecture.md`](docs/architecture.md) for the full architecture and build plan.

## Repository structure

```
BioCongress/
├── backend/
│   └── Icof.Api/          ASP.NET Core Web API (entities, DTOs, services, controllers)
├── frontend/
│   └── icof-web/          Angular application
├── docs/
│   └── architecture.md    Architecture, domain boundaries, build direction
├── infrastructure/        Deployment / infra config
└── docker-compose.yaml    Local PostgreSQL
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) 20+ and npm
- [Docker](https://www.docker.com/) (for local PostgreSQL)
- [Angular CLI](https://angular.dev/tools/cli): `npm install -g @angular/cli`

## Local setup

### 1. Database

```bash
docker compose up -d
```

Starts PostgreSQL on `localhost:5433` (db `icof`, user `icof`, password `icof123` — local dev only, not for production).

### 2. Backend

```bash
cd backend/Icof.Api
dotnet tool restore
dotnet ef database update
dotnet run
```

API runs at `http://localhost:5245` (`https://localhost:7262` for HTTPS). OpenAPI docs are available at `/openapi` in development.

Connection string lives in `appsettings.Development.json` (`ConnectionStrings:DefaultConnection`) — already pointed at the Docker Compose database above.

### 3. Frontend

```bash
cd frontend/icof-web
npm install
ng serve
```

App runs at `http://localhost:4200` and proxies/connects to the API above.

## Database migrations

Migrations live in `backend/Icof.Api/Data/Migrations`. After changing an entity:

```bash
cd backend/Icof.Api
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

## Current status

- Backend domain entities, EF Core config, and initial migrations are in place: `Event`, `EventAgendaItem`, `EventPerson`, `EventRegistration`, `TeamMember`, `PeopleGroup`, `Organization`, `PageContent`, `SiteSetting`, plus ASP.NET Identity for users/roles.
- Event registration is implemented end-to-end (atomic capacity handling, one registration per user/event).
- Remaining backend work: DTOs and CRUD controllers/services for the rest of the domain (Events, TeamMembers, PageContent, Organizations), admin endpoints.
- Frontend is currently the default Angular CLI scaffold. Page designs (Home, About, History, Members, Partners & Sponsors, Contact, Fees & Payments, Timetable, etc.) exist as static HTML/CSS mockups and are being ported into Angular components, then connected to the API above.

## Roadmap

1. Lock backend entities, migrations, Identity, and registration rules ✅
2. Convert HTML/CSS designs into Angular layouts and reusable components — *in progress*
3. Build public pages: Home, About, Team, Events, Event Details, Contact
4. Add auth/profile pages and connect them to the API
5. Build admin CRUD screens, protected by role
6. Add media storage integration (Azure Blob Storage)
7. Staging environment, monitoring, load tests
8. Launch simulations and replica/database tuning
