using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Models;

namespace WatchMe.Controllers
{
    public class GenreController : Controller
{
    private readonly AppDbContext _context;

    public GenreController(AppDbContext context)
    {
        _context = context;
    }

    // Genre sayfasını render et
    public IActionResult MoviesByGenre(int genreId)
{
    var genre = _context.Genres
        .Include(g => g.MovieGenres)
        .ThenInclude(mg => mg.Movie)
        .FirstOrDefault(g => g.GenreId == genreId);

    if (genre == null)
    {
        return NotFound();
    }

    var movies = genre.MovieGenres.Select(mg => mg.Movie).ToList();

    ViewData["GenreName"] = genre.Name;

    Console.WriteLine($"Returned model type: {movies.GetType()}");

    return View(movies.ToList());

}


public IActionResult TVShowsByGenre(int genreId)
{
    var genre = _context.Genres
        .Include(g => g.TVShowGenres)
        .ThenInclude(mg => mg.TVShow)
        .FirstOrDefault(g => g.GenreId == genreId);

    if (genre == null)
    {
        return NotFound();
    }

    var tvshows = genre.TVShowGenres.Select(mg => mg.TVShow).ToList();

    ViewData["GenreName"] = genre.Name;

    return View("TVShowsByGenre", tvshows);  // PartialView
}

}

}
