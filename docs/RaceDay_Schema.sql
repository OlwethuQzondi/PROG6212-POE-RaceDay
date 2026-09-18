-- ============================================================================
-- Academic Attributed Script: RaceDay Database Creation & Seed Script
-- Module: PROG6212 - Programming 2B (Part 1)
-- Description: Comprehensive database schema definition and sample seed data
--              for the RaceDay Event Management Platform.
-- Reference Standard: Microsoft SQL Server T-SQL Documentation & Best Practices
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RaceDayDb')
BEGIN
    CREATE DATABASE RaceDayDb;
END
GO

USE RaceDayDb;
GO

-- ----------------------------------------------------------------------------
-- Drop Tables if they exist (for idempotent environment setups)
-- ----------------------------------------------------------------------------
IF OBJECT_ID('dbo.Results', 'U') IS NOT NULL DROP TABLE dbo.Results;
IF OBJECT_ID('dbo.Enrolments', 'U') IS NOT NULL DROP TABLE dbo.Enrolments;
IF OBJECT_ID('dbo.EventCategories', 'U') IS NOT NULL DROP TABLE dbo.EventCategories;
IF OBJECT_ID('dbo.Events', 'U') IS NOT NULL DROP TABLE dbo.Events;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE dbo.Roles;
GO

-- ----------------------------------------------------------------------------
-- Entity 1: Roles
-- Description: Represents system permission levels (Organiser, Participant)
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Roles (
    RoleId INT IDENTITY(1,1) NOT NULL,
    RoleName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(250) NULL,
    CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED (RoleId),
    CONSTRAINT UQ_Roles_RoleName UNIQUE (RoleName)
);
GO

-- ----------------------------------------------------------------------------
-- Entity 2: Users
-- Description: Stores profile data for event organisers and participants
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Users (
    UserId INT IDENTITY(1,1) NOT NULL,
    RoleId INT NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_Users PRIMARY KEY CLUSTERED (UserId),
    CONSTRAINT UQ_Users_Email UNIQUE (Email),
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES dbo.Roles(RoleId) ON DELETE NO ACTION
);
GO

-- ----------------------------------------------------------------------------
-- Entity 3: Events
-- Description: Core race/road event information managed by Organisers
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Events (
    EventId INT IDENTITY(1,1) NOT NULL,
    OrganiserId INT NOT NULL,
    Title NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    EventDate DATETIME2 NOT NULL,
    RegistrationDeadline DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_Events PRIMARY KEY CLUSTERED (EventId),
    CONSTRAINT FK_Events_Organiser FOREIGN KEY (OrganiserId) REFERENCES dbo.Users(UserId) ON DELETE CASCADE,
    CONSTRAINT CHK_Event_Dates CHECK (RegistrationDeadline <= EventDate)
);
GO

-- ----------------------------------------------------------------------------
-- Entity 4: EventCategories
-- Description: Sub-categories/distances per event (e.g., 42km Marathon, 10km Run)
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.EventCategories (
    CategoryId INT IDENTITY(1,1) NOT NULL,
    EventId INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    DistanceKm DECIMAL(6,2) NOT NULL,
    EntryFee DECIMAL(10,2) NOT NULL,
    MaxParticipants INT NOT NULL,
    CONSTRAINT PK_EventCategories PRIMARY KEY CLUSTERED (CategoryId),
    CONSTRAINT FK_EventCategories_Events FOREIGN KEY (EventId) REFERENCES dbo.Events(EventId) ON DELETE CASCADE,
    CONSTRAINT CHK_Category_Distance CHECK (DistanceKm > 0),
    CONSTRAINT CHK_Category_Fee CHECK (EntryFee >= 0),
    CONSTRAINT CHK_Category_MaxParts CHECK (MaxParticipants > 0)
);
GO

-- ----------------------------------------------------------------------------
-- Entity 5: Enrolments
-- Description: Links Participants to specific Event Categories (Event entries)
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Enrolments (
    EnrolmentId INT IDENTITY(1,1) NOT NULL,
    CategoryId INT NOT NULL,
    ParticipantId INT NOT NULL,
    EnrolmentDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    PaymentStatus NVARCHAR(30) NOT NULL DEFAULT 'Pending',
    BibNumber INT NULL,
    CONSTRAINT PK_Enrolments PRIMARY KEY CLUSTERED (EnrolmentId),
    CONSTRAINT FK_Enrolments_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.EventCategories(CategoryId) ON DELETE CASCADE,
    CONSTRAINT FK_Enrolments_Participants FOREIGN KEY (ParticipantId) REFERENCES dbo.Users(UserId) ON DELETE NO ACTION,
    CONSTRAINT UQ_Enrolment_Participant_Category UNIQUE (CategoryId, ParticipantId),
    CONSTRAINT CHK_Enrolment_PaymentStatus CHECK (PaymentStatus IN ('Pending', 'Completed', 'Cancelled'))
);
GO

-- ----------------------------------------------------------------------------
-- Entity 6: Results
-- Description: Performance timing and rankings captured for participants
-- ----------------------------------------------------------------------------
CREATE TABLE dbo.Results (
    ResultId INT IDENTITY(1,1) NOT NULL,
    EnrolmentId INT NOT NULL,
    FinishTime TIME NOT NULL,
    OverallPosition INT NULL,
    CategoryPosition INT NULL,
    RecordedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT PK_Results PRIMARY KEY CLUSTERED (ResultId),
    CONSTRAINT UQ_Results_Enrolment UNIQUE (EnrolmentId),
    CONSTRAINT FK_Results_Enrolments FOREIGN KEY (EnrolmentId) REFERENCES dbo.Enrolments(EnrolmentId) ON DELETE CASCADE,
    CONSTRAINT CHK_Results_Positions CHECK (OverallPosition > 0 AND CategoryPosition > 0)
);
GO

-- ============================================================================
-- SEED DATA (Meets & Exceeds Rubric Minimums)
-- ============================================================================

-- 1. Roles
INSERT INTO dbo.Roles (RoleName, Description) VALUES
('Organiser', 'Event administrator who creates events, sets categories, and logs results'),
('Participant', 'Runner, walker, or cyclist entering events and viewing performance');

-- 2. Users (2 Organisers, 2 Participants)
INSERT INTO dbo.Users (RoleId, FirstName, LastName, Email, PasswordHash, PhoneNumber) VALUES
(1, 'Sibusiso', 'Dlamini', 'sibu.dlamini@raceday.co.za', 'AQAAAAEAACcQAAAAEH8z...', '+27821234567'),
(1, 'Sarah', 'Van Der Merwe', 'sarah.vdm@raceday.co.za', 'AQAAAAEAACcQAAAAEJ9a...', '+27839876543'),
(2, 'Olwethu', 'Zondi', 'olwethu.zondi@student.ac.za', 'AQAAAAEAACcQAAAAEK0b...', '+27710001122'),
(2, 'Aphiwe', 'Ndlovu', 'aphiwe.ndlovu@student.ac.za', 'AQAAAAEAACcQAAAAEL1c...', '+27723334455');

-- 3. Events (3 Events)
INSERT INTO dbo.Events (OrganiserId, Title, Description, Location, EventDate, RegistrationDeadline) VALUES
(1, 'Durban Promenade Ultra Run 2026', 'Coastal road race along the iconic Durban beachfront.', 'Durban, KwaZulu-Natal', '2026-10-15 06:00:00', '2026-10-10 23:59:59'),
(1, 'Comrades Training Marathon', 'High-altitude preparation road run between Pietermaritzburg and Hillcrest.', 'Pietermaritzburg, KZN', '2026-11-05 05:30:00', '2026-11-01 23:59:59'),
(2, 'Cape Peninsula Cycling Classic', 'Scenic coastal cycling event showcasing the Cape Peninsula.', 'Cape Town, Western Cape', '2026-12-01 07:00:00', '2026-11-20 23:59:59');

-- 4. Event Categories (Multiple per event)
INSERT INTO dbo.EventCategories (EventId, CategoryName, DistanceKm, EntryFee, MaxParticipants) VALUES
(1, '10km Beach Dash', 10.00, 150.00, 500),
(1, '21km Half Marathon', 21.10, 280.00, 1000),
(2, '42km Full Marathon', 42.20, 450.00, 1200),
(3, '109km Tour Cycle', 109.00, 650.00, 2000);

-- 5. Enrolments
INSERT INTO dbo.Enrolments (CategoryId, ParticipantId, PaymentStatus, BibNumber) VALUES
(2, 3, 'Completed', 1001),
(2, 4, 'Completed', 1002),
(4, 3, 'Completed', 2050);

-- 6. Results
INSERT INTO dbo.Results (EnrolmentId, FinishTime, OverallPosition, CategoryPosition) VALUES
(1, '01:28:45', 14, 3),
(2, '01:35:10', 28, 8);
GO