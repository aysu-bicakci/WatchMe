using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WatchMe.Services
{
    public class EmailService
    {
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("proje.yapimi1@gmail.com", "spsg diav gvvr ccau"), // App password here
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("project.yapimi1@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(toEmail);

                // E-posta gönderme işlemi asenkron olarak yapılır
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Hata mesajını loglayalım
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}