// ============================================================================
// Author: Olwethu Qiniso Zondi
// Module: PROG6212 - Programming 2B (Part 2)
// Assessment: Portfolio of Evidence (Part 2 - RESTful API Development)
// References:
// - Troelsen, A. and Japikse, P. 2021. Pro C# 10 with .NET 6. 11th ed. Apress.
// - Microsoft. 2026. ASP.NET Core Web API Documentation. [Online]
//   Available at: https://docs.microsoft.com/en-us/aspnet/core/web-api/
// ============================================================================

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Controllers;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;
using Xunit;

namespace RaceDay.Tests
{
    public class EventAndEnrolmentTests
    {
        private RaceDayDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<RaceDayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new RaceDayDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task CreateEvent_AsParticipantRole_ReturnsForbidden403()
        {
            // Arrange
            var context = GetDatabaseContext();
            var controller = new EventsController(context);

            // Simulate Session as Participant (RoleId = 2)
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            httpContext.Session.SetInt32("UserId", 10);
            httpContext.Session.SetInt32("RoleId", 2); // Participant
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var eventDto = new CreateEventDto
            {
                Title = "Unauthorized Event",
                Location = "Durban North",
                EventDate = DateTime.Now.AddDays(10),
                RegistrationDeadline = DateTime.Now.AddDays(5)
            };

            // Act
            var result = await controller.CreateEvent(eventDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status403Forbidden, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateEvent_AsOrganiserRole_ReturnsCreatedAtAction()
        {
            // Arrange
            var context = GetDatabaseContext();
            var controller = new EventsController(context);

            // Simulate Session as Organiser (RoleId = 1)
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            httpContext.Session.SetInt32("UserId", 1);
            httpContext.Session.SetInt32("RoleId", 1); // Organiser
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            var eventDto = new CreateEventDto
            {
                Title = "Durban 10K City Run",
                Description = "Annual road race",
                Location = "Durban Beachfront",
                EventDate = DateTime.Now.AddDays(20),
                RegistrationDeadline = DateTime.Now.AddDays(15),
                Categories = new List<CreateCategoryDto>
                {
                    new CreateCategoryDto { CategoryName = "10K Open", DistanceKm = 10, EntryFee = 150, MaxParticipants = 500 }
                }
            };

            // Act
            var result = await controller.CreateEvent(eventDto);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(1, await context.Events.CountAsync());
        }
    }

    // Helper Mock Class for In-Memory Session State
    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new();

        public bool IsAvailable => true;
        public string Id => "TestSessionId";
        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public void Clear() => _sessionStorage.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _sessionStorage.Remove(key);
        public void Set(string key, byte[] value) => _sessionStorage[key] = value;
        public bool TryGetValue(string key, out byte[]? value) => _sessionStorage.TryGetValue(key, out value);
    }
}