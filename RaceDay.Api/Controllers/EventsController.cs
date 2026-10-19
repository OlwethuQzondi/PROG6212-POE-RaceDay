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
    /// Manages sporting event creation, updates, deletion, and public event browsing.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all scheduled sporting events and their category sub-divisions.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _context.Events
                .Include(e => e.Categories)
                .Include(e => e.Organiser)
                .ToListAsync();
            return Ok(events);
        }

        /// <summary>
        /// Retrieves detailed information for a single event by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var raceEvent = await _context.Events
                .Include(e => e.Categories)
                .Include(e => e.Organiser)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (raceEvent == null) return NotFound(new { message = "Event not found." });
            return Ok(raceEvent);
        }

        /// <summary>
        /// Creates a new race event with distance categories (Organiser role required).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var roleId = HttpContext.Session.GetInt32("RoleId");

            if (!userId.HasValue) return Unauthorized(new { message = "Authentication required." });
            if (roleId != 1) return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied. Organiser role required." });

            var raceEvent = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                Location = dto.Location,
                EventDate = dto.EventDate,
                RegistrationDeadline = dto.RegistrationDeadline,
                OrganiserId = userId.Value,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var cat in dto.Categories)
            {
                raceEvent.Categories.Add(new EventCategory
                {
                    CategoryName = cat.CategoryName,
                    DistanceKm = cat.DistanceKm,
                    EntryFee = cat.EntryFee,
                    MaxParticipants = cat.MaxParticipants
                });
            }

            _context.Events.Add(raceEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEventById), new { id = raceEvent.EventId }, raceEvent);
        }

        /// <summary>
        /// Deletes an event (Organiser role required).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var roleId = HttpContext.Session.GetInt32("RoleId");
            if (roleId != 1) return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied. Organiser role required." });

            var raceEvent = await _context.Events.FindAsync(id);
            if (raceEvent == null) return NotFound();

            _context.Events.Remove(raceEvent);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Event successfully deleted." });
        }
    }
}