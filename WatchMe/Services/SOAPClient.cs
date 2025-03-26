//SOAPClient.cs

using System.Collections.Generic;
using System.Linq;
using WatchMe.Models;
using WatchMe.Data;

namespace WatchMe.Services
{
    public class SOAPClient
    {
        private readonly AppDbContext _context;

        public SOAPClient(AppDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetMovies(string searchTerm)
        {
            return _context.Movies
                .Where(m => m.Title.Contains(searchTerm))
                .ToList();
        }

        public List<TVShow> GetTVShows(string searchTerm)
        {
            return _context.TVShows
                .Where(tv => tv.Title.Contains(searchTerm))
                .ToList();
        }
    }
}