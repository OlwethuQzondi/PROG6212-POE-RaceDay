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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RaceDay.Api.Controllers;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;
using RaceDay.Api.Models;
using Xunit;

namespace RaceDay.Tests
{
    public class AuthTests
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
        public async Task Register_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var context = GetDatabaseContext();
            var passwordHasher = new PasswordHasher<User>();
            var controller = new AuthController(context, passwordHasher);

            var registerDto = new RegisterDto
            {
                FirstName = "Test",
                LastName = "Participant",
                Email = "test@raceday.com",
                Password = "Password123!",
                RoleId = 2
            };

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(1, await context.Users.CountAsync(u => u.Email == "test@raceday.com"));
        }

        [Fact]
        public async Task Register_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var context = GetDatabaseContext();
            var passwordHasher = new PasswordHasher<User>();
            context.Users.Add(new User
            {
                FirstName = "Existing",
                LastName = "User",
                Email = "duplicate@raceday.com",
                PasswordHash = "hash",
                RoleId = 2
            });
            await context.SaveChangesAsync();

            var controller = new AuthController(context, passwordHasher);
            var registerDto = new RegisterDto
            {
                FirstName = "New",
                LastName = "User",
                Email = "duplicate@raceday.com",
                Password = "Password123!",
                RoleId = 2
            };

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}