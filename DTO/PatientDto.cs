using System.ComponentModel.DataAnnotations;

namespace AuthenticationFirst.DTO
{
    public class PatientDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public bool TermsAccepted { get; set; }
    }
}
