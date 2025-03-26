using Microsoft.AspNetCore.Mvc;
using WatchMe.Data;
using WatchMe.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WatchMe.Controllers
{
    public class TVShowController : Controller
    {
        private readonly CommentRepository _commentRepository;
        private readonly TVShowService _tvShowService;
        private readonly AppDbContext _context;
        private readonly ILogger<TVShowController> _logger;

        public TVShowController(TVShowService tvShowService, AppDbContext context, ILogger<TVShowController> logger, CommentRepository commentRepository)
        {
            _tvShowService = tvShowService;
            _context = context;
            _logger = logger;
            _commentRepository = commentRepository;
        }

        // Dizi beğenme işlemi
        public async Task<IActionResult> LikeTVShow(int tvShowId)
{
    int userId = GetCurrentUserId();
    bool isLiked = await _tvShowService.LikeTVShowAsync(userId, tvShowId);

    // Film zaten beğenilmişse mesajı gösterme
    if (isLiked)
    {
        TempData["Message"] = "Dizi beğenildi.";
    }
    else
    {
        TempData["Message"] = "Dizi zaten beğenilmiş.";
    }

    return RedirectToAction("Details", new { id = tvShowId });
}


// Dizi beğenmeme işlemi
public async Task<IActionResult> DislikeTVShow(int tvShowId)
{
    int userId = GetCurrentUserId();
    bool isDisliked = await _tvShowService.DislikeTVShowAsync(userId, tvShowId);

    // Dizi zaten beğenilmemişse mesajı gösterme
    if (isDisliked)
    {
        TempData["Message"] = "Dizi beğenilmedi.";
    }
    else
    {
        TempData["Message"] = "Dizi zaten beğenilmemiş.";
    }

    return RedirectToAction("Details", new { id = tvShowId });
}


// İzleme listesine ekleme işlemi
public async Task<IActionResult> AddToWatchList(int tvShowId)
{
    int userId = GetCurrentUserId();
    bool isAdded = await _tvShowService.AddToWatchListAsync(userId, tvShowId);

    // Dizi zaten izleme listesinde mi kontrol et
    if (isAdded)
    {
        TempData["Message"] = "Dizi izleme listesine eklendi.";
    }
    else
    {
        TempData["Message"] = "Dizi zaten izleme listenizde.";
    }

    return RedirectToAction("Details", new { id = tvShowId });
}


        // Dizi detay sayfası
        public async Task<IActionResult> Details(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    // Diziyi ve ilişkili verileri getiriyoruz
    var tvShow = await _context.TVShows
        .Include(ts => ts.TVShowGenres!)
            .ThenInclude(tg => tg.Genre)
        .Include(ts => ts.TVShowLikes)
        .Include(ts => ts.TVShowDislikes)
        .Include(ts => ts.TVShowComments!)
            .ThenInclude(tsc => tsc.User)
        .AsNoTracking()
        .FirstOrDefaultAsync(ts => ts.TVShowId == id);

    if (tvShow == null)
    {
        return NotFound();
    }
    ViewData["Comments"] = tvShow.TVShowComments.Count;

    int userId = GetCurrentUserId();
    ViewData["UserId"] = userId;

    // Null kontrolü yapıyoruz
    tvShow.TVShowComments ??= new List<TVShowComment>();
    tvShow.TVShowGenres ??= new List<TVShowGenre>();

    var totalTVShowComments = _commentRepository.GetTotalMovieComments(tvShow.TVShowId);
    ViewData["TotalComments"] = totalTVShowComments;

    return View(tvShow);
}

        // Yorum eklemek
        [HttpPost]
        public async Task<IActionResult> AddComment(int tvShowId, string comment)
        {
            int userId = GetCurrentUserId();
            await _tvShowService.AddCommentAsync(tvShowId, userId, comment);
            return RedirectToAction("Details", new { id = tvShowId });
        }

        // Yorum güncellemek
        [HttpPost]
        public async Task<IActionResult> UpdateComment(int commentId, string newComment, int tvShowId)
        {
            var comment = await _context.TVShowComments
                .FirstOrDefaultAsync(c => c.TVShowCommentId == commentId);

            if (comment == null || comment.UserId != 1) // Kullanıcı sadece kendi yorumunu güncelleyebilir
            {
                return Unauthorized(); // Kullanıcı yetkisi yok
            }

            comment.Comment = newComment; // Yorum metnini güncelle
            _context.TVShowComments.Update(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = tvShowId });
        }

        // Yorum silmek
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId, int tvShowId)
        {
            var comment = await _context.TVShowComments
                .FirstOrDefaultAsync(c => c.TVShowCommentId == commentId);

            if (comment == null || comment.UserId != 1) // Kullanıcı sadece kendi yorumunu silebilir
            {
                return Unauthorized(); // Kullanıcı yetkisi yok
            }

            _context.TVShowComments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = tvShowId });
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

        public async Task<IActionResult> RemoveTVShow(int tvShowId)
{
    int userId = GetCurrentUserId();
    await _tvShowService.RemoveTVShowFromWatchlistAsync(userId, tvShowId);
    return RedirectToAction("Details", new { id = tvShowId });
}


    }
}
