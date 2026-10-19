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
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.Api.Models
{
    public class Enrolment
    {
        [Key]
        public int EnrolmentId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ParticipantId { get; set; }

        public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string PaymentStatus { get; set; } = "Completed";

        public int? BibNumber { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public EventCategory? Category { get; set; }

        [ForeignKey(nameof(ParticipantId))]
        public User? Participant { get; set; }

        public Result? Result { get; set; }
    }
}