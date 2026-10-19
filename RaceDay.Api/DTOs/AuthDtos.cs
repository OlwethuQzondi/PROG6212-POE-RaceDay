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
    public class RegisterDto
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        [Required]
        public int RoleId { get; set; } // 1 = Organiser, 2 = Participant
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}