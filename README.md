# Conference Room Booking API

A REST API for managing conference rooms, bookings, additional services, pricing, and business reports.

The project was developed as a take-home assignment with a focus on clean architecture, maintainability, validation, transactional consistency, and extensibility.

## Business Tasks

The system supports the following business operations:

- Create, update, retrieve, and delete conference rooms.
- Search for available rooms by required capacity and time period.
- Create room bookings for a specific date and time.
- Select additional services for a booking.
- Calculate the total booking price based on the room rate, booking duration, time-of-day pricing rules, and selected services.
- Prevent overlapping bookings for the same room.
- Prevent deletion of rooms that already have bookings.
- Manage additional services.
- Generate business reports for a selected period.

### Initial Example Data

The assignment defines the following example rooms:

| Room | Capacity | Base hourly price |
|---|---:|---:|
| Room A | 50 | 2000 |
| Room B | 100 | 3500 |
| Room C | 30 | 1500 |

Example additional services:

| Service | Price |
|---|---:|
| Projector | 500 |
| Wi-Fi | 300 |
| Sound | 700 |

## API

### Rooms

- `GET /api/Room` — get all rooms
- `GET /api/Room/{id}` — get a room by ID
- `POST /api/Room` — create a room
- `PUT /api/Room/{id}` — update a room
- `DELETE /api/Room/{id}` — delete a room
- `GET /api/Room/available` — search available rooms

### Bookings

- `GET /api/Booking` — get bookings
- `POST /api/Booking` — create a booking

### Services

- `GET /api/Service` — get all services
- `GET /api/Service/{id}` — get a service
- `POST /api/Service` — create a service

### Reports

- `GET /api/Reports/summary` — generate a business summary for a selected period

Swagger/OpenAPI documentation is available when the application is running in the Development environment.

## Pricing Rules

The hourly room price depends on the booking time:

| Period | Price adjustment |
|---|---:|
| 06:00–09:00 | -10% |
| 09:00–18:00 | Standard price |
| 12:00–14:00 | +15% |
| 18:00–23:00 | -20% |

The pricing implementation is based on independent pricing rules.

When multiple rules apply to the same time interval, the rule with the highest priority is selected. This allows overlapping pricing rules such as the 12:00–14:00 peak period to override the standard daytime rate.

Bookings are split into pricing intervals when they cross pricing boundaries, so different parts of the same booking can use different rates.

The pricing logic is isolated from the booking workflow, making it easier to add or change pricing rules without modifying booking logic.

## Architecture & Technical Decisions

The project uses a layered architecture:

```text
Controllers
    ↓
Services
    ↓
Repositories
    ↓
Entity Framework Core
    ↓
PostgreSQL
```

### Controllers

Controllers expose the REST API and translate HTTP requests and business results into HTTP responses.

### Services

The service layer contains business logic and validation that should not be handled directly by controllers or database access code.

Examples:

- booking validation;
- room availability checks;
- overlapping booking detection;
- price calculation;
- service validation;
- report validation.

### Repositories

Repositories encapsulate database access and keep Entity Framework Core details out of the service layer.

### DTOs

API requests and responses use dedicated DTOs instead of exposing database entities directly.

This keeps the public API contract independent from the persistence model.

### Pricing Rules

Pricing is implemented using the Strategy-like rule approach through `IPricingRule`.

Each rule defines:

- when it applies;
- its priority;
- its price multiplier.

This makes the pricing system extensible without creating a large conditional block inside the booking service.

### Dependency Injection

Application components are registered through the built-in ASP.NET Core dependency injection container.

Repositories and services use scoped lifetimes, matching the lifetime of the Entity Framework Core `DbContext`.

## Database

The application uses:

- PostgreSQL 16
- Entity Framework Core 8
- Npgsql provider

PostgreSQL can be started using Docker Compose.

The database schema is managed through Entity Framework Core migrations.

Current migrations include:

- initial database schema;
- booking total price;
- booking service price snapshot.

### Service Price Snapshot

When a booking is created, the current price of each selected service is stored in `BookingServiceLink.Price`.

This preserves the historical booking price even if the service price is changed later.

## Reports & Analytics

The project includes a business summary report:

`GET /api/Reports/summary?from=...&to=...`

The report provides:

- total number of bookings;
- total revenue;
- total booked hours;
- booking count and booked hours per room;
- revenue per room;
- service usage statistics;
- revenue generated by services.

The report is intended to provide basic operational information for evaluating room utilization and revenue.

## Validation & Business Rules

The API validates both request data and business rules.

Examples:

- room capacity and prices must be positive;
- booking end time must be after start time;
- bookings must be within the supported 06:00–23:00 working period;
- a room cannot have overlapping bookings;
- requested services must exist;
- duplicate services in a booking are rejected;
- a room with existing bookings cannot be deleted.

ASP.NET Core model validation is used for request-level validation, while business rules are handled in the service layer.

## Transactional Consistency

Booking creation consists of several database operations:

1. Create the booking.
2. Save the booking to obtain its ID.
3. Create booking/service links with price snapshots.
4. Commit the transaction.

These operations are executed inside a database transaction so that a partially created booking is not left in the database if one of the operations fails.

## Testing

The project contains automated unit tests for the pricing domain.

Current test coverage includes:

- standard pricing;
- morning pricing;
- evening pricing;
- peak pricing;
- pricing rule boundaries;
- overlapping pricing rules;
- pricing rule priorities;
- multi-interval bookings.

Current test result:

```text
20 passed, 0 failed
```

The project also includes manual API verification for room management, booking conflicts, service validation, price calculation, reports, and database integration.

## Project Structure

```text
abp-test-project/
├── src/
│   └── ConferenceRoomApi/
│       ├── Controllers/
│       ├── DTOs/
│       ├── Data/
│       ├── Domain/
│       │   └── Pricing/
│       ├── Migrations/
│       ├── Repositories/
│       ├── Results/
│       ├── Services/
│       ├── Program.cs
│       └── appsettings.json
│
├── tests/
│   └── ConferenceRoomApi.Tests/
│
├── compose.yaml
└── abp-test-project.sln
```

## How to Run

### Prerequisites

- .NET 8 SDK
- Docker Desktop
- PostgreSQL can be run through the included Docker Compose configuration

### 1. Start PostgreSQL

```bash
docker compose up -d
```

### 2. Apply database migrations

```bash
dotnet ef database update --project ./src/ConferenceRoomApi/ConferenceRoomApi.csproj
```

### 3. Run the API

```bash
dotnet run --project ./src/ConferenceRoomApi/ConferenceRoomApi.csproj
```

The API can then be accessed through the configured HTTP endpoint.

Swagger UI is available in the Development environment.

### 4. Run tests

```bash
dotnet test
```

## Configuration and Secrets

The PostgreSQL connection string is configured through .NET User Secrets for local development.

The repository does not contain the database password.

For a local setup, configure:

```text
ConnectionStrings:DefaultConnection
```

using the .NET User Secrets mechanism.

## Technical Stack

- C# / .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- PostgreSQL 16
- Npgsql
- Docker / Docker Compose
- Swagger / OpenAPI
- xUnit
- Git / GitHub
