# RaceDay API Endpoint Plan

| HTTP Method | Route | Description | Role Required | Request Body | Expected Response |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **POST** | `/api/auth/register` | Registers a new user account as either an Organiser or Participant. | Public | `{ "firstName", "lastName", "email", "password", "roleId" }` | **201 Created**: `{ "userId", "email" }`<br>**400 Bad Request**: Validation failure<br>**409 Conflict**: Email taken |
| **POST** | `/api/auth/login` | Authenticates user credentials and issues a JWT token. | Public | `{ "email", "password" }` | **200 OK**: `{ "token", "expiration", "role" }`<br>**401 Unauthorized**: Invalid credentials |
| **GET** | `/api/users/profile` | Fetches the current logged-in user's profile details. | Any (Authenticated) | None | **200 OK**: `{ "userId", "firstName", "email", "roleName" }`<br>**401 Unauthorized** |
| **PUT** | `/api/users/profile` | Updates personal profile information for the authenticated user. | Any (Authenticated) | `{ "firstName", "lastName", "phoneNumber" }` | **200 OK**: Profile updated message<br>**400 Bad Request** |
| **GET** | `/api/events` | Retrieves all upcoming events with optional location/keyword filtering. | Public | None | **200 OK**: Array of event summary objects |
| **POST** | `/api/events` | Creates a new road running or cycling event in the platform. | Organiser | `{ "title", "description", "location", "eventDate", "registrationDeadline" }` | **201 Created**: `{ "eventId", "title" }`<br>**403 Forbidden**: Participant user |
| **PUT** | `/api/events/{id}` | Updates specific event details for an event owned by the organiser. | Organiser | `{ "title", "description", "location", "eventDate" }` | **200 OK**: Updated event object<br>**404 Not Found** |
| **DELETE** | `/api/events/{id}` | Cancels and deletes an existing event record. | Organiser | None | **204 No Content**<br>**403 Forbidden** |
| **POST** | `/api/events/{eventId}/categories` | Adds a distance category (e.g., 21km, 42km) to a specific event. | Organiser | `{ "categoryName", "distanceKm", "entryFee", "maxParticipants" }` | **201 Created**: `{ "categoryId", "categoryName" }`<br>**400 Bad Request** |
| **GET** | `/api/events/{eventId}/categories` | Lists all distances/categories available under a specific event. | Public | None | **200 OK**: Array of category details |
| **POST** | `/api/enrolments` | Enrols a participant into a specific event category. | Participant | `{ "categoryId" }` | **201 Created**: `{ "enrolmentId", "paymentStatus" }`<br>**409 Conflict**: Already enrolled |
| **GET** | `/api/enrolments/my-enrolments` | Retrieves all past and upcoming event entries for the logged-in participant. | Participant | None | **200 OK**: Array of enrolment history objects |
| **GET** | `/api/enrolments/event/{eventId}` | Lists all registered participants for an event. | Organiser | None | **200 OK**: List of participant enrolments |
| **POST** | `/api/results` | Records timing and finishing placement positions for an enrolled participant. | Organiser | `{ "enrolmentId", "finishTime", "overallPosition", "categoryPosition" }` | **201 Created**: `{ "resultId" }`<br>**400 Bad Request** |
| **GET** | `/api/results/event/{eventId}` | Fetches leaderboard and timing results for a completed event. | Public | None | **200 OK**: Ranked list of participant results |
| **GET** | `/api/results/my-history` | Fetches personal historical race results and timing records. | Participant | None | **200 OK**: List of user's past race times |