using Microsoft.AspNetCore.Identity;

namespace AuthenticationFirst.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsHealthcareProvider { get; set; }
        public string? LicenseNumber { get; set; }
        public string? Specialization { get; set; }
        public string? ClinicOrHospital { get; set; }
        public string? PasswordResetCodes { get; set; }
    }
}
