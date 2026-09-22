# RaceDay Event Management System
**Portfolio of Evidence: PROG6212 (Programming 2B) - Part 1**

**Student Details**
* **Name:** Olwethu Qiniso Zondi
* **Program:** Bachelor of Computer and Information Sciences in Application Development
* **Campus:** Emeris Durban North Campus
* **Module:** PROG6212 (Programming 2B)

---

## 1. Project Overview
RaceDay is a comprehensive web application designed to streamline the management of community sporting events. The system facilitates the creation of events by organizers, tracks participant enrolments across varying distance categories, manages entry fee structures, and records official race results. This documentation covers the foundational database architecture, RESTful API endpoint planning, and Continuous Integration (CI/CD) implementation for Part 1 of the project lifecycle.

## 2. Video Presentation
A comprehensive walkthrough of the system architecture, entity-relationship logic, live SQL execution, and CI/CD workflow validation can be viewed here:
**YouTube Link:** https://youtu.be/kAgDOKlzyks

## 3. System Architecture & Database Design
The relational database, `RaceDayDb`, is designed with strict adherence to normalization and data integrity principles. 

* **Roles & Users (1...1 to 1...*):** Role-based access control defining 'Organisers' and 'Participants'.
* **Events & EventCategories (1...1 to 1...*):** An event contains multiple tiered categories (e.g., 5km, 10km, 21km) with unique entry fees and participant capacity limits.
* **Enrolments (1...1 to 0...*):** A junction mapping that securely links an authenticated User to a specific EventCategory.
* **Results (1...1 to 0...1):** A one-to-one mapping with completed enrolments to capture official finish times and overall positioning.

*See `docs/ERD_Diagram.png` for the visual mapping of these relationships.*

## 4. API Endpoint Plan
The backend architecture is structured around RESTful design principles, segmented into functional resource groups to handle client-server communication.

| Resource Group | HTTP Method | Endpoint | Description | Permission Level |
| :--- | :--- | :--- | :--- | :--- |
| **Authentication** | POST | `/api/auth/register` | Registers a new user account. | Public |
| **Authentication** | POST | `/api/auth/login` | Authenticates a user and returns a token. | Public |
| **Events** | GET | `/api/events` | Retrieves a list of upcoming races. | Public |
| **Events** | POST | `/api/events` | Creates a new sporting event. | Organiser |
| **Enrolments** | POST | `/api/enrolments` | Registers a participant for a category. | Participant |
| **Results** | POST | `/api/results` | Records official race completion times. | Organiser |

*See `docs/Endpoint_Plan.md` for full payload schemas and HTTP status code definitions.*

## 5. Database Setup & Execution Instructions
To initialize the backend environment and populate the seed data, execute the provided T-SQL script in Microsoft SQL Server.

1. Launch **SQL Server Management Studio (SSMS)**.
2. Connect to the local server instance (`localhost\SQLEXPRESS`) using Windows Authentication.
3. Open the `docs/RaceDay_Schema.sql` file.
4. Execute the script (`F5`).
   * *Note: The script is idempotent. It will safely drop existing tables before creating the `RaceDayDb` database, defining all primary/foreign key constraints, and inserting the base testing data.*
5. Verification screenshots proving successful execution and data population are located at:
   * `docs/ssms_schema_creation_proof.png`
   * `docs/ssms_seed_data_proof.png`

## 6. Version Control & Continuous Integration
This project utilizes Git for version control, maintaining a history of meaningful, atomic commits. A GitHub Actions CI pipeline (`build-validation.yml`) is configured to automatically validate the integrity of the documentation directory upon every push to the `main` branch.

* **Repository:** https://github.com/OlwethuQzondi/PROG6212-POE-RaceDay
* **Workflow Status:** ![CI Status](https://github.com/OlwethuQzondi/PROG6212-POE-RaceDay/actions/workflows/build-validation.yml/badge.svg)
* *See `docs/ci_workflow_passing.png` for visual proof of the consistent build history.*