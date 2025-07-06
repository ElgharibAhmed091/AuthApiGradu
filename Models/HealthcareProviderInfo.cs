using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuthenticationFirst.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SpecializationType
    {
        GeneralPractitioner,  // طبيب عام
        NursePractitioner,    // ممرض ممارس
        Pharmacist            // صيدلي
    }

    // تعريف أنواع العيادات أو المستشفيات
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ClinicOrHospitalType
    {
        SaudiGermanHospital,  // المستشفى السعودي الألماني
        MilitaryHospital,     // المستشفى العسكري
        PrivateClinic         // عيادة خاصة
    }

    public class HealthcareProviderInfo
    {
        [Required]
        public string LicenseNumber { get; set; }

        [Required]
        public SpecializationType Specialization { get; set; }

        [Required]
        public ClinicOrHospitalType ClinicOrHospital { get; set; }
    }
}
