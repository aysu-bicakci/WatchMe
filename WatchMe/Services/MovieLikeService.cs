using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Protos;
using Microsoft.Extensions.Logging;

   public class MovieLikeServiceImpl : MovieLikeService.MovieLikeServiceBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<MovieLikeServiceImpl> _logger;

    public MovieLikeServiceImpl(AppDbContext context, ILogger<MovieLikeServiceImpl> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override async Task<LikedMoviesResponse> GetLikedMovies(LikedMoviesRequest request, ServerCallContext context)
    {
        _logger.LogInformation($"Fetching liked movies for user with ID: {request.UserId}");

        var likedMovies = await _context.MovieLikes
            .Where(ml => ml.UserId == request.UserId)
            .Include(ml => ml.Movie)
            .Select(ml => new Movie
            {
                Id = ml.Movie.MovieId,
                Title = ml.Movie.Title,
                PosterPath = ml.Movie.PosterPath,
                Popularity = ml.Movie.Popularity ?? 0.0
            })
            .ToListAsync();

        _logger.LogInformation($"Found {likedMovies.Count} liked movies for user with ID: {request.UserId}");

        if (!likedMovies.Any())
        {
            _logger.LogWarning($"No liked movies found for user with ID: {request.UserId}");
        }

        var response = new LikedMoviesResponse();
        response.Movies.AddRange(likedMovies);

        return response;
    }
}
