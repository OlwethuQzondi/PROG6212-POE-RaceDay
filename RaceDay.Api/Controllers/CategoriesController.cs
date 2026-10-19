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
    /// Manages event distance/age categories.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public CategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all categories for a specific event.
        /// </summary>
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetCategoriesByEvent(int eventId)
        {
            var categories = await _context.EventCategories
                .Where(c => c.EventId == eventId)
                .ToListAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Adds a category to an existing event (Organiser role required).
        /// </summary>
        [HttpPost("event/{eventId}")]
        public async Task<IActionResult> AddCategory(int eventId, [FromBody] CreateCategoryDto dto)
        {
            var roleId = HttpContext.Session.GetInt32("RoleId");
            if (roleId != 1) return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied. Organiser role required." });

            var category = new EventCategory
            {
                EventId = eventId,
                CategoryName = dto.CategoryName,
                DistanceKm = dto.DistanceKm,
                EntryFee = dto.EntryFee,
                MaxParticipants = dto.MaxParticipants
            };

            _context.EventCategories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }
    }
}