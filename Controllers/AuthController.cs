using AuthenticationFirst.Data;
using AuthenticationFirst.Models;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;

namespace MedicalApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthenticationFirst.Models.IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager; // إضافة UserManager

        public AuthController(ApplicationDbContext context, AuthenticationFirst.Models.IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _emailService = emailService;
            _userManager = userManager; // حقن UserManager
        }
        [HttpPost("Change-password")]
        public async Task<IActionResult> Changepassword([FromBody] ChangePass model)
        {
            // التحقق من صحة الإدخال
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return BadRequest("New password and confirmation password do not match.");
            }

            var user = await _userManager.FindByEmailAsync(model.Email); // الحصول على email الحالي
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ApiResponse
                {
                    Status = "Error",
                    Message = "User does not exist!"
                });
            }
            if (string.Compare(model.NewPassword, model.ConfirmNewPassword) != 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest,new ApiResponse
                {
                    Status = "Error",
                    Message = "Password Confirm and new password does not match!"
                });
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if(!result.Succeeded)
            {
                var errors = new List<string>();    
                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                } 

                return StatusCode(StatusCodes.Status500InternalServerError,new ApiResponse
                {
                    Status = "Error",
                    Message = string.Join(",",errors)
                });
            }
            return Ok(new ApiResponse { Status="Success",Message="  Change Password SuccessFully ! "});
            
        } 

        // Endpoint لطلب Forget Password
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // توليد كود عشوائي
            var code = GenerateRandomCode();
            var resetCode = new PasswordResetCode
            {
                Email = request.Email,
                Code = code,
                ExpiryDate = DateTime.UtcNow.AddMinutes(10) // صلاحية 10 دقايق
            };

            _context.PasswordResetCodes.Add(resetCode);
            await _context.SaveChangesAsync();

            // إرسال الكود عبر الإيميل
            await _emailService.SendEmailAsync(request.Email, "Password reset code", $"Your verification code is: {code}");

            return Ok("Verification code has been sent to your email.");
        }

        // Endpoint لتأكيد الكود وتغيير كلمة السر
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var resetCode = await _context.PasswordResetCodes
                .FirstOrDefaultAsync(c => c.Email == request.Email && c.Code == request.Code && c.ExpiryDate > DateTime.UtcNow);

            if (resetCode == null)
            {
                return BadRequest("The code is invalid or expired.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // تغيير كلمة السر باستخدام PasswordHasher من Identity
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);
            _context.PasswordResetCodes.Remove(resetCode); // حذف الكود بعد الاستخدام
            await _context.SaveChangesAsync();

            return Ok("Password changed successfully");
        }

        // دالة لتوليد كود عشوائي
        private string GenerateRandomCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // كود 6 أرقام
        }
    }

    // نماذج البيانات
    public class ForgetPasswordRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}