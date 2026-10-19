// ============================================================================
// Author: Olwethu Qiniso Zondi
// Module: PROG6212 - Programming 2B (Part 2)
// Assessment: Portfolio of Evidence (Part 2 - RESTful API Development)
// References:
// - Troelsen, A. and Japikse, P. 2021. Pro C# 10 with .NET 6. 11th ed. Apress.
// - Microsoft. 2026. ASP.NET Core Web API Documentation. [Online]
//   Available at: https://docs.microsoft.com/en-us/aspnet/core/web-api/
// ============================================================================

using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.DTOs
{
    public class CreateEventDto
    {
        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [StringLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public DateTime RegistrationDeadline { get; set; }

        public List<CreateCategoryDto> Categories { get; set; } = new List<CreateCategoryDto>();
    }

    public class CreateCategoryDto
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [Range(0.1, 1000)]
        public decimal DistanceKm { get; set; }

        [Range(0, 10000)]
        public decimal EntryFee { get; set; }

        [Range(1, 100000)]
        public int MaxParticipants { get; set; }
    }

    public class CreateEnrolmentDto
    {
        [Required]
        public int CategoryId { get; set; }
    }

    public class CreateResultDto
    {
        [Required]
        public int EnrolmentId { get; set; }

        [Required]
        public TimeSpan FinishTime { get; set; }

        public int OverallPosition { get; set; }
        public int CategoryPosition { get; set; }
    }
}