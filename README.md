# PROG6212 Portfolio of Evidence (Part 1): RaceDay Event Management Platform

## Project Overview
**RaceDay** is an enterprise-grade event management web application developed for local motorsport and athletic events. The system streamlines event scheduling, competitor registrations, category assignments, and real-time result publishing.

## Directory Structure
```text
PROG6212-POE-RaceDay/
├── .github/
│   └── workflows/
│       └── build-validation.yml    # CI/CD GitHub Actions pipeline definition
├── docs/
│   └── RaceDay_Schema.sql         # Idempotent T-SQL database creation & seed script
├── .gitignore                      # Visual Studio environment ignore configuration
└── README.md                       # Project documentation and architecture summary
```

---

## Database Architecture
The platform utilizes a relational SQL Server database ('RaceDayDb') configured with third-normal-form (3NF) relational integrity and proper cascade rules.

* **`Roles`**: System user permissions ('Administrator', 'Organizer', 'Competitor').
* **`Users`**: Account profiles linked to security roles.
* **`Events`**: Scheduled race events with start dates, locations, and status flags.
* **`EventCategories`**: Sub-divisions (e.g., *10K Run*, *500cc Sprint*).
* **`Enrolments`**: Junction mapping competitors to specific event categories.
* **`Results`**: Timing outcomes, finish ranks, and completion metrics.

---

## CI/CD Pipeline Integration
This repository integrates **GitHub Actions** (`build-validation.yml`) to ensure continuous validation:
1. **Directory Structure Verification**: Ensures the mandatory `/docs` folder exists on every push.
2. **Schema Script Validation**: Asserts that `RaceDay_Schema.sql` is present and trackable.
3. **Automated Build Checks**: Prepares the build environment for .NET 8 solution compilation.

---

## Execution Instructions
1. Clone the repository:
```
   git clone https://github.com/OlwethuQzondi/PROG6212-POE-RaceDay.git
```

2. Run `docs/RaceDay_Schema.sql` inside **SQL Server Management Studio (SSMS)** or **Azure Data Studio** to instantiate `RaceDayDb` and seed default administrative records.