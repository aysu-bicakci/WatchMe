using WatchMe.Data;
using System.Security.Cryptography;
using System.Text;

public class ResetPasswordService
{
    private readonly AppDbContext _context;

    public ResetPasswordService(AppDbContext context)
    {
        _context = context;
    }

    public bool ResetPassword(string token, string newPassword)
    {
        // Token doğrulama
        var user = _context.Users.FirstOrDefault(u => u.ResetToken == token);
        if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
        {
            return false;
        }

        // Şifre sıfırlama
        user.Password = HashPassword(newPassword);
        user.ResetToken = null;
        user.ResetTokenExpiry = null;

        _context.Users.Update(user);
        _context.SaveChanges();

        return true;
    }

    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashedBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashedBytes);
        }
    }
}