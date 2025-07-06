using AuthenticationFirst.DTO;
using AuthenticationFirst.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationFirst.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration; // إضافة IConfiguration لـ JWT

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration; // حقن IConfiguration
        }
        [HttpPost("register/healthcare-provider")]
        public async Task<IActionResult> RegisterHealthcareProvider([FromBody] HealthCarePrDto model)
        {
            // التحقق من صحة البيانات المرسلة
            if (!ModelState.IsValid)
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            // التأكد من قبول الشروط
            if (!model.TermsAccepted)
                return BadRequest(new { message = "You must accept the Terms of Service to register." });

            // التأكد من صحة الإدخال
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.EmailAddress))
                return BadRequest(new { message = "Username and Email are required." });

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.EmailAddress,
                IsHealthcareProvider = model.IsHealthcareProvider
            };

            // التحقق من بيانات مقدم الرعاية الصحية
            if (model.IsHealthcareProvider && model.ProviderInfo != null)
            {
                user.LicenseNumber = model.ProviderInfo.LicenseNumber;
                user.Specialization = model.ProviderInfo.Specialization.ToString();
                user.ClinicOrHospital = model.ProviderInfo.ClinicOrHospital.ToString();
            }

            // إنشاء الحساب
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            return Ok(new { message = "User registered successfully" });
        }




        [HttpPost("register/patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!model.TermsAccepted)
                return BadRequest(new { message = "You must accept the Terms of Service to register." });

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.EmailAddress
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { message = "User not found" });

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "The password is incorrect" });

            // توليد JWT Token (اختياري لو بتستخدم JWT)
            var token = GenerateJwtToken(user);
            return Ok(new { message = "You have been logged in successfully", token });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:TokenExpiryInMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpDelete("delete-account-by-email")]
        public async Task<IActionResult> DeleteAccountByEmail([FromBody] DeleteAccount model)
        {
            // 1. ابحث عن المستخدم بواسطة البريد الإلكتروني
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // 2. تحقق من صحة كلمة المرور (اختياري لكن مستحسن للأمان)
            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordCheck)
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }

            // 3. احذف المستخدم
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to delete user." });
            }

            return Ok(new { message = "User account deleted successfully." });
        }
    }
}