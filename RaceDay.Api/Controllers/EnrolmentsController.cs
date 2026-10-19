// ============================================================================
// Author: Olwethu Qiniso Zondi
// Module: PROG6212 - Programming 2B (Part 2)
// Assessment: Portfolio of Evidence (Part 2 - RESTful API Development)
// References:
// - Troelsen, A. and Japikse, P. 2021. Pro C# 10 with .NET 6. 11th ed. Apress.
// - Microsoft. 2026. ASP.NET Core Web API Documentation. [Online]
//   Available at: https://docs.microsoft.com/en-us/aspnet/core/web-api/
// ============================================================================

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers
{
    /// <summary>
    /// Handles participant registrations for race categories.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Enrols an authenticated Participant into an event category.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Enrol([FromBody] CreateEnrolmentDto dto)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Unauthorized(new { message = "Authentication required." });

            var existing = await _context.Enrolments
                .FirstOrDefaultAsync(e => e.ParticipantId == userId.Value && e.CategoryId == dto.CategoryId);

            if (existing != null) return BadRequest(new { message = "You are already enrolled in this category." });

            var enrolment = new Enrolment
            {
                CategoryId = dto.CategoryId,
                ParticipantId = userId.Value,
                EnrolmentDate = DateTime.UtcNow,
                PaymentStatus = "Completed"
            };

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return Ok(enrolment);
        }

        /// <summary>
        /// Retrieves all enrolments for an event (Organiser role required).
        /// </summary>
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetEnrolmentsForEvent(int eventId)
        {
            var roleId = HttpContext.Session.GetInt32("RoleId");
            if (roleId != 1) return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied." });

            var enrolments = await _context.Enrolments
                .Include(e => e.Participant)
                .Include(e => e.Category)
                .Where(e => e.Category != null && e.Category.EventId == eventId)
                .ToListAsync();

            return Ok(enrolments);
        }
    }
}