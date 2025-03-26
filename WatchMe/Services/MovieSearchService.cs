//MovieSearchService.cs
using System.Linq;
using WatchMe.Models;
using System.Collections.Generic;
using WatchMe.Data;

namespace WatchMe.Services
{
    public class MovieSearchService : IMovieSearchService
    {
        private readonly AppDbContext _dbContext;

        public MovieSearchService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Movie> SearchMovies(string title)
        {
            return _dbContext.Movies
                             .Where(m => m.Title.Contains(title))
                             .ToList();
        }

        public List<TVShow> SearchTVShows(string title)
        {
            return _dbContext.TVShows
                             .Where(tv => tv.Title.Contains(title))
                             .ToList();
        }
    }
}