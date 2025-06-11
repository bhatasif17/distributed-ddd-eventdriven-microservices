# 📊 DDD - Microservices - Distributed

This is a simple .NET 8 microservice that demonstrates core concepts of **Domain-Driven Design (DDD)** and **event-driven architecture**.

## ✅ Features

- Add a position
- Remove a position
- Background services:
    - Azure Service Bus consumer
    - Rate simulator (simulates external rate feed)

## 🧰 Tech Stack

- ASP.NET Core
- EF Core with In-Memory Database
- MediatR
- Azure Service Bus
- Background Services
- Unit Tests (Demo purposes only)

## 📂 Project Structure

The project follows DDD principles and is organized into:

- `Domain`: Contains entities and core business logic
- `Application`: Application logic, DTOs, and MediatR handlers
- `Infrastructure`: Persistence and messaging integrations
- `API`: Entry point with controller endpoints
- `BackgroundServices`: Hosted services for rate simulation and bus message consumption
- `Tests`: Unit test project (not fully implemented, serves as a demo)

## 📬 API Endpoints

- `POST /api/positions` — Add a new position
- `DELETE api/positions/{id}` — Remove a position

## 🧪 Testing

Basic unit tests are included to demonstrate structure. Full test coverage is not implemented.

## 🚀 Getting Started

1. Clone the repository
2. Open the solution in Rider or Visual Studio
3. Run the API project
4. Use Swagger or Postman to interact with the endpoints

---

This is a demo project meant to showcase architectural patterns and design—not intended for production use.