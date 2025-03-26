using Microsoft.EntityFrameworkCore;
using WatchMe.Models;
using WatchMe.Data;

public class CommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    // Film yorum sayısını çek
   public int GetTotalMovieComments(int movieId)
{
    var result = _context.Database
                         .ExecuteSqlRaw("SELECT sp_get_total_movie_comments({0})", movieId);
    
    return result;
}

public int GetTotalTVShowComments(int tvShowId)
{
    var result = _context.Database
                         .ExecuteSqlRaw("SELECT sp_get_total_tvshow_comments({0})", tvShowId);
    return result;
}

}
