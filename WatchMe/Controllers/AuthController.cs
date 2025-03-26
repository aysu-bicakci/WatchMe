using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Models;
using WatchMe.Dtos;
using WatchMe.Services;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace WatchMe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService; 
       private readonly ResetPasswordService _resetPasswordService;
        private readonly ILogger _logger;


        public AuthController(AppDbContext context, EmailService emailService, ResetPasswordService resetPasswordService,ILogger<AuthController> logger)
        {
            _context = context;
            _emailService = emailService;
            _resetPasswordService = resetPasswordService;
            _logger = logger;
        }


        // E-posta kontrolü
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            var emailExists = await _context.Users.AnyAsync(u => u.Email == email);
            return Ok(new { isEmailTaken = emailExists });
        }

        // Register (kayıt işlemi)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            try
            {
                if (userDto == null)
                    return BadRequest("Invalid user data.");

                // Email adresinin zaten kullanılıp kullanılmadığını kontrol et
                if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                    return BadRequest("User with this email already exists.");

                // Şifreyi hash'le
                var hashedPassword = HashPassword(userDto.Password);

                // Kullanıcıyı veritabanına ekle
                var user = new User
                {
                    Nickname= userDto.Nickname!, // Nickname eklendi
                    Email = userDto.Email,
                    Password = hashedPassword  // Hashlenmiş şifre kaydediliyor
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("User registered successfully.");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Login (giriş işlemi)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid login data.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            // Şifreyi doğrulama işlemi
            if (!VerifyPassword(user.Password, loginDto.Password))
                return Unauthorized("Invalid email or password.");

            return Ok(new { message = "Login successful" });
        }

        // Şifreyi doğrulama metodu (girdiğimiz şifreyi hash'leyip kontrol ediyoruz)
        private bool VerifyPassword(string storedPassword, string enteredPassword)
        {
            var enteredPasswordHash = HashPassword(enteredPassword);
            return storedPassword == enteredPasswordHash;
        }

        // Şifreyi hash'leme metodu
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Şifremi Unuttum (Forgot Password)
[HttpPost("forgot-password")]
public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
{
    if (string.IsNullOrEmpty(request.Email))
    {
        return BadRequest("Email is required.");
    }

    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
    if (user == null)
    {
        return NotFound("Email not found");
    }

    // Eğer mevcut bir reset token varsa, eski token'ı geçersiz hale getir.
    if (user.ResetToken != null)
    {
        user.ResetToken = null; // Mevcut token'ı sıfırla
        user.ResetTokenExpiry = null; // Token'ın süresini de sıfırla
        await _context.SaveChangesAsync();
    }

    // Yeni bir reset token oluştur
    var resetToken = Guid.NewGuid().ToString();
    user.ResetToken = resetToken;
   user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token geçerliliği 1 saat, UTC zamanı kullanılarak ayarlandı.
    await _context.SaveChangesAsync(); // Asenkron şekilde kaydedin

    var resetLink = $"https://localhost:5001/api/auth/reset-password?token={resetToken}";
    var body = $"Reset link for {request.Email}: {resetLink}";

    // Asenkron e-posta gönderimi
    var emailSent = await _emailService.SendEmailAsync(request.Email, "Password Reset Request", body);

    if (emailSent)
    {
        return Ok(new { message = "Password reset link has been sent to your email." });
    }
    else
    {
        return StatusCode(500, "Error sending email.");
    }
}


           // GET: /auth/reset-password?token=...
[HttpGet("reset-password")]
public IActionResult ResetPassword([FromQuery] string token)
{
    _logger.LogInformation($"ResetPassword called with token: {token}");

    if (string.IsNullOrEmpty(token))
    {
        _logger.LogWarning("Token is missing.");
        return BadRequest("Invalid token.");
    }

    // Token'ı ViewData ile view'e gönderiyoruz
    ViewData["Token"] = token;

    // Razor view dosyasını döndürüyoruz
    return View("~/Views/Home/ResetPassword.cshtml");
}


   
     // POST: /api/auth/reset-password

[HttpPost("reset-password")]
public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
{
    if (string.IsNullOrEmpty(resetPasswordDto.Token) || string.IsNullOrEmpty(resetPasswordDto.NewPassword))
        return BadRequest(new { message = "Invalid request. Token or password cannot be empty." });

    var result = _resetPasswordService.ResetPassword(resetPasswordDto.Token, resetPasswordDto.NewPassword);
    if (!result)
    {
        return BadRequest(new { message = "Invalid token or token expired." });
    }

    return Ok(new { message = "Your password has been reset successfully." });
}

    
    }
}