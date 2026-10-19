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
    /// Captures and publishes official race results.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Records finish time and rank for an enrolment (Organiser role required).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RecordResult([FromBody] CreateResultDto dto)
        {
            var roleId = HttpContext.Session.GetInt32("RoleId");
            if (roleId != 1) return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied. Organiser role required." });

            var result = new Result
            {
                EnrolmentId = dto.EnrolmentId,
                FinishTime = dto.FinishTime,
                OverallPosition = dto.OverallPosition,
                CategoryPosition = dto.CategoryPosition
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return Ok(result);
        }

        /// <summary>
        /// Retrieves the personal race results for the logged-in participant.
        /// </summary>
        [HttpGet("my-results")]
        public async Task<IActionResult> GetMyResults()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue) return Unauthorized(new { message = "Authentication required." });

            var results = await _context.Results
                .Include(r => r.Enrolment)
                .ThenInclude(e => e!.Category)
                .Where(r => r.Enrolment != null && r.Enrolment.ParticipantId == userId.Value)
                .ToListAsync();

            return Ok(results);
        }
    }
}