using Microsoft.AspNetCore.Mvc;
using WatchMe.Services;  // gRPC servisinizi kullandığınız namespace
using Microsoft.Extensions.Logging; // Logger kullanmak için
using System.Threading.Tasks;
using WatchMe.Models; // Veritabanı modelleriniz veya view modelleri
using WatchMe.Protos;
using Grpc.Net.Client;
using WatchMe.Data;
using Microsoft.EntityFrameworkCore;

namespace WatchMe.Controllers

{

    
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly AppDbContext _context;

        // Her iki bağımlılığı tek constructor üzerinden alıyoruz
        public ProfileController(ILogger<ProfileController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Profile()
        {

            int userId = 1; // Sabit kullanıcı ID
        var user = await _context.Users.FindAsync(userId);
        ViewData["UserName"] = user?.Nickname;
        ViewData["UserEmail"] = user?.Email;
        
             var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    // gRPC istemcisi oluştur
    var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
    {
        HttpHandler = handler
    });
            var client = new MovieLikeService.MovieLikeServiceClient(channel);

        

            // gRPC servisine istek gönder
            var request = new LikedMoviesRequest { UserId = userId };
            var response = await client.GetLikedMoviesAsync(request);

            // gRPC'den dönen filmleri model olarak gönder
            var likedMovies = response.Movies
                .Select(m => new WatchMe.Models.Movie
                {
                    MovieId = m.Id,
                    Title = m.Title,
                    PosterPath = m.PosterPath,
                    Popularity = m.Popularity
                }).ToList();

            // Verinin doğru şekilde geldiğini kontrol et
            if (likedMovies == null || !likedMovies.Any())
            {
                _logger.LogWarning("No liked movies found for user with ID: " + userId);
            }

            var dislikedMovies = await _context.MovieDislikes
        .Where(md => md.UserId == userId)
        .Select(md => md.Movie)
        .ToListAsync();


        // ViewData üzerinden iki listeyi gönderiyoruz
    ViewData["LikedMovies"] = likedMovies;
    ViewData["DislikedMovies"] = dislikedMovies;

            return View();
        }

    }
}