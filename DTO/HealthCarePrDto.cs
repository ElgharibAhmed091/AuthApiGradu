using AuthenticationFirst.Models;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationFirst.DTO
{
    public class HealthCarePrDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsHealthcareProvider { get; set; }
        public HealthcareProviderInfo? ProviderInfo { get; set; }

        [Required]
        public bool TermsAccepted { get; set; }
    }
}
