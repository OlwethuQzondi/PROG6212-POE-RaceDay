# RaceDay Event Management Platform (RESTful API)

**Portfolio of Evidence: PROG6212 (Programming 2B) - Part 2**

## Student Details

- **Name:** Olwethu Qiniso Zondi
- **Program:** Bachelor of Computer and Information Sciences in Application Development
- **Campus:** Emeris Durban North Campus
- **Module:** PROG6212 (Programming 2B)

---

## 1. Executive Summary & Architecture Overview

**RaceDay** is an enterprise-grade RESTful web API developed using ASP.NET Core 8.0, Entity Framework Core, and SQL Server. The system manages sporting event creation, distance category management, competitor enrolments, and official result publishing.

Access control is enforced server-side using secure password hashing through `IPasswordHasher` and session-based identity management through `HttpContext.Session`.

## 2. Video Presentation

A walkthrough of the running API, code structure, Swagger UI, session authentication, and xUnit test execution can be viewed here.

**YouTube Link:** https://youtu.be/2wen0PkEUCw(#)

## 3. System Roles & Security Model

The system enforces Role-Based Access Control (RBAC) to restrict access to protected operations.

- **Organiser (RoleId = 1):** Full CRUD permissions over Events and Event Categories, including publishing official results.
- **Participant (RoleId = 2):** Authenticated access to browse upcoming events, register for event categories, and view personal race performance results.

## 4. API Endpoint Specifications

| Resource Group | HTTP Method | Endpoint | Description | Permission Required |
|---|---|---|---|---|
| Authentication | POST | `/api/auth/register` | Registers a new Organiser or Participant account with a hashed password. | Public |
| Authentication | POST | `/api/auth/login` | Authenticates credentials and establishes a server session. | Public |
| Authentication | GET | `/api/auth/profile` | Retrieves the current session user's identity profile. | Authenticated User |
| Events | GET | `/api/events` | Retrieves all scheduled events and distance categories. | Public |
| Events | POST | `/api/events` | Creates a new event with distance tier categories. | Organiser Only |
| Events | DELETE | `/api/events/{id}` | Deletes an event and its associated categories. | Organiser Only |
| Categories | POST | `/api/categories/event/{eventId}` | Adds a new distance category to an existing event. | Organiser Only |
| Enrolments | POST | `/api/enrolments` | Enrols the logged-in participant into an event category. | Participant Only |
| Enrolments | GET | `/api/enrolments/event/{eventId}` | Retrieves all participant enrolments for an event. | Organiser Only |
| Results | POST | `/api/results` | Publishes official finish times and overall rankings. | Organiser Only |
| Results | GET | `/api/results/my-results` | Retrieves personal race completion records. | Authenticated User |

## 5. Local Setup & Execution Guide

### Step 1: Clone the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/OlwethuQzondi/PROG6212-POE-RaceDay.git
```

### Step 2: Database Initialization

- Ensure LocalDB or SQL Server Express (`localhost\SQLEXPRESS`) is running.
- Entity Framework Core Code-First is configured to create the `RaceDayDb` database and seed the default roles during initial startup.

### Step 3: Run the API

From the repository's root directory, run:

```bash
dotnet run --project RaceDay.Api
```

### Step 4: Interactive API Verification (Swagger)

Once the API is running, open the following URL in your browser:

[http://localhost:5283/swagger](http://localhost:5283/swagger)

> **Note:** The Swagger URL and port may differ depending on your local application configuration.

## 6. Unit Testing Suite

The project includes an xUnit testing suite covering registration, login, event creation, and role-based access rejection.

Run the tests from the repository's root directory:

```bash
dotnet test
```

Review the test output to confirm whether the tests pass.

## 7. Version Control & Continuous Integration

- **Repository:** [PROG6212-POE-RaceDay](https://github.com/OlwethuQzondi/PROG6212-POE-RaceDay)
- **Workflow Status:** Check the repository's **Actions** tab for the current Continuous Integration (CI) workflow status.

---

**RaceDay Event Management Platform**  
*PROG6212 (Programming 2B) - Portfolio of Evidence*
