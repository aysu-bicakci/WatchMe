using Microsoft.AspNetCore.Mvc;
using WatchMe.Models;
using WatchMe.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WatchMe.Services;
using System.ServiceModel; // SOAP servisi için gerekli namespace

namespace WatchMe.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserServiceClient _userServiceClient;

        public HomeController(AppDbContext context)
        {
            _context = context;
            // SOAP istemcisini başlatıyoruz
            _userServiceClient = new UserServiceClient(
                new BasicHttpBinding(), 
                new EndpointAddress("http://localhost:5003/userservice")
            );
        }

        // Kullanıcı adını almak için bir metot
  private async Task<string> GetUserNameAsync(int userId = 1)
{
    try
    {
        // SOAP üzerinden kullanıcı adı bilgisi alıyoruz
        return await _userServiceClient.GetUserNameAsync(userId);
    }
    catch (Exception)
    {
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == 1);
        return user?.Nickname ?? "Guest"; // Eğer kullanıcı bulunmazsa "Guest" döner
    }
}

        // Reset Password View
        public async Task<IActionResult> ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid token.");
            }

            ViewData["Token"] = token; // Token'ı view'e gönder
            ViewData["UserName"] = await GetUserNameAsync(1); // Kullanıcı adını al ve viewData'ya ekle
            return View();
        }

        // Privacy Page
        public async Task<IActionResult> Privacy()
        {
            ViewData["ShowHeaderFooter"] = true;
            ViewData["UserName"] = await GetUserNameAsync(1); // Kullanıcı adını al ve viewData'ya ekle
            return View();
        }

        // Login Success View
        [HttpGet("LoginSuccess")]
        public async Task<IActionResult> LoginSuccess()
        {
            ViewData["UserName"] = await GetUserNameAsync(1); // Kullanıcı adını al ve viewData'ya ekle
            return View();
        }

        // Index - List all movies
        public async Task<IActionResult> Index()
        {
            var allMovies = _context.Movies.ToList(); // Veritabanındaki tüm filmleri al
            ViewData["UserName"] = await GetUserNameAsync(1); // Kullanıcı adını al ve viewData'ya ekle
            return View(allMovies); // Filmleri view'a gönder
        }

        // Welcome Page (without header/footer)
        public async Task<IActionResult> WelcomePage()
        {
            ViewData["ShowHeaderFooter"] = false;
            ViewData["UserName"] = await GetUserNameAsync(1); // Kullanıcı adını al ve viewData'ya ekle
            return View();
        }

        // Login Page (without header/footer)
        public async Task<IActionResult> Login()
        {
            ViewData["ShowHeaderFooter"] = false;
            ViewData["UserName"] = await GetUserNameAsync(1); // Kullanıcı adını al ve viewData'ya ekle
            return View();
        }

        // Register Page (without header/footer)
        public async Task<IActionResult> Register()
        {
            ViewData["ShowHeaderFooter"] = false;
            ViewData["UserName"] = await GetUserNameAsync(1); // Kullanıcı adını al ve viewData'ya ekle
            return View();
        }

        // Forgot Password Page (without header/footer)
        public async Task<IActionResult> ForgotPassword()
        {
            ViewData["ShowHeaderFooter"] = false;
            ViewData["UserName"] = await GetUserNameAsync(1); // Kullanıcı adını al ve viewData'ya ekle
            return View();
        }

        // Profile Page (with header/footer)
        public async Task<IActionResult> Profile()
        {
            ViewData["ShowHeaderFooter"] = true;
            ViewData["UserName"] = await GetUserNameAsync(1); // Kullanıcı adını al ve viewData'ya ekle
            return View();
        }

        // Netflix Page (list all movies)
        public async Task<IActionResult> Netflix()
        {
            var allMovies = _context.Movies.ToList();
            ViewData["UserName"] = await GetUserNameAsync(1); // Kullanıcı adını al ve viewData'ya ekle
            return View(allMovies);
        }
    }
}