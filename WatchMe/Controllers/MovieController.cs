using Microsoft.AspNetCore.Mvc;
using WatchMe.Data;
using WatchMe.Models;
using WatchMe.Services;
using WatchMe.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WatchMe.Controllers
{
    public class MovieController : Controller
    {

        private readonly CommentRepository _commentRepository;
        private readonly MovieService _movieService;
        private readonly AppDbContext _context;
        private readonly ILogger<MovieController> _logger;

        public MovieController(MovieService movieService, AppDbContext context, ILogger<MovieController> logger, CommentRepository commentRepository)
        {
            _movieService = movieService;
            _context = context;
            _logger = logger;
            _commentRepository = commentRepository;
        }



        // Film detay sayfası
        public async Task<IActionResult> LikeMovie(int movieId)
{
    int userId = GetCurrentUserId();

    bool isLiked = await _movieService.LikeMovieAsync(userId, movieId);

    // Film zaten beğenilmişse mesajı gösterme
    if (isLiked)
    {
        TempData["Message"] = "Film beğenildi.";
    }
    else
    {
        TempData["Message"] = "Film zaten beğenilmiş.";
    }

    return RedirectToAction("Details", new { id = movieId });
}


public async Task<IActionResult> DislikeMovie(int movieId)
{
    int userId = GetCurrentUserId();
    bool isDisliked = await _movieService.DislikeMovieAsync(userId, movieId);

    // Film zaten beğenilmemişse mesajı gösterme
    if (isDisliked)
    {
        TempData["Message"] = "Film beğenilmedi.";
    }
    else
    {
        TempData["Message"] = "Film zaten beğenilmemiş.";
    }

    return RedirectToAction("Details", new { id = movieId });
}


public async Task<IActionResult> AddToWatchList(int movieId)
{
    int userId = GetCurrentUserId();
    bool isAdded = await _movieService.AddToWatchListAsync(userId, movieId);

    // Film zaten izleme listesinde mi kontrol et
    if (isAdded)
    {
        TempData["Message"] = "Film izleme listesine eklendi.";
    }
    else
    {
        TempData["Message"] = "Film zaten izleme listenizde.";
    }

    return RedirectToAction("Details", new { id = movieId });
}


        // Film detay sayfası
        public async Task<IActionResult> Details(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var movie = await _context.Movies
        .Include(m => m.MovieGenres!)
            .ThenInclude(mg => mg.Genre)
        .Include(m => m.MovieLikes)
        .Include(m => m.MovieDislikes)
        .Include(m => m.MovieComments!)
            .ThenInclude(mc => mc.User)
        .AsNoTracking()
        .FirstOrDefaultAsync(m => m.MovieId == id);

    if (movie == null)
    {
        return NotFound();
    }
    ViewData["Comments"] = movie.MovieComments.Count;
    
    
    int userId = GetCurrentUserId();
    ViewData["UserId"] = userId;

    movie.MovieComments ??= new List<MovieComment>();
    movie.MovieGenres ??= new List<MovieGenre>();

    // Burada veritabanındaki prosedür ile yorum sayısını da alalım
    var totalMovieComments = _commentRepository.GetTotalMovieComments(movie.MovieId);
    ViewData["TotalComments"] = totalMovieComments;

    var movieList = new List<Movie> { movie };

    return View(movieList);
}


        // Yorum eklemek
        [HttpPost]
        public async Task<IActionResult> AddComment(int movieId, string comment)
        {
            int userId = 1;
            await _movieService.AddCommentAsync(movieId, userId, comment);
            return RedirectToAction("Details", new { id = movieId });
        }

        // Yorum güncellemek
        [HttpPost]
        public async Task<IActionResult> UpdateComment(int commentId, string newComment, int movieId)
        {
            var comment = await _context.MovieComments
                .FirstOrDefaultAsync(c => c.MovieCommentId == commentId);

            if (comment == null || comment.UserId != 1) // Kullanıcı sadece kendi yorumunu güncelleyebilir
            {
                return Unauthorized(); // Kullanıcı yetkisi yok
            }

            comment.Comment = newComment; // Yorum metnini güncelle
            _context.MovieComments.Update(comment);
            await _context.SaveChangesAsync();

            // Güncellenmiş yorum sonrası film detaylarına yönlendir
    return RedirectToAction("Details", "Movie", new { id = movieId });
        }
    // Yorum güncellemek
    [HttpPost]
  public async Task<IActionResult> UpdateCommentRedirect(int commentId, string newComment, int movieId)
  {
    var comment = await _context.MovieComments
        .FirstOrDefaultAsync(c => c.MovieCommentId == commentId);

            return RedirectToAction("Details", new { id = movieId });
        }

        // Yorum silmek
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId, int movieId)
        {
            var comment = await _context.MovieComments
                .FirstOrDefaultAsync(c => c.MovieCommentId == commentId);

            if (comment == null || comment.UserId != 1) // Kullanıcı sadece kendi yorumunu silebilir
            {
                return Unauthorized(); // Kullanıcı yetkisi yok
            }

            _context.MovieComments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = movieId });
        }

        // Rastgele filmler
        public async Task<IActionResult> RandomMovies()
        {
            var movies = await _context.Movies
                .OrderBy(x => Guid.NewGuid()) // Rastgele sıralama
                .Take(6) // Örnek olarak 6 film al
                .ToListAsync();
            return PartialView("_NetflixCard", movies);
        }

        // Popüler filmler
        public async Task<IActionResult> PopularMovies()
        {
            var movies = await _context.Movies
                .OrderByDescending(m => m.Popularity) // Popülerliğe göre sıralama
                .Take(6) // İlk 6 filmi al
                .ToListAsync();
            return PartialView("_NetflixCard", movies);
        }

        // Yeni çıkan filmler
        public async Task<IActionResult> NewReleases()
        {
            var movies = await _context.Movies
                .OrderByDescending(m => m.ReleaseDate) // Çıkış tarihine göre sıralama
                .Take(6) // İlk 6 filmi al
                .ToListAsync();
            return PartialView("_NetflixCard", movies);
        }

        private int GetCurrentUserId()
        {
            var userIdString = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            return 1; // Varsayılan kullanıcı ID'si (Güvenlik açısından geliştirilebilir)
        }

        public async Task<IActionResult> RemoveMovie(int movieId)
{
    int userId = GetCurrentUserId();
    await _movieService.RemoveMovieFromWatchlistAsync(userId, movieId);
    return RedirectToAction("Details", new { id = movieId });
}
    }
    
}