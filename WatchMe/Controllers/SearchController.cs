using Microsoft.AspNetCore.Mvc;
using WatchMe.Models;
using WatchMe.Services;
using System.Linq;
using System.Collections.Generic;

public class SearchController : Controller
{
    private readonly SOAPClient _soapClient;

    public SearchController(SOAPClient soapClient)
    {
        _soapClient = soapClient;
    }

    [Route("search")]
    public IActionResult Results(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term cannot be empty.");
        }
         ViewData["ShowHeaderFooter"] = true;

        // SOAPClient üzerinden film ve dizi arama
        var movies = _soapClient.GetMovies(searchTerm) as List<WatchMe.Models.Movie>;
        var tvShows = _soapClient.GetTVShows(searchTerm) as List<WatchMe.Models.TVShow>;

        // ViewData'ya sonuçları gönder
        ViewData["SearchTerm"] = searchTerm;
        ViewData["Movies"] = movies;
        ViewData["TVShows"] = tvShows;

        return View("Results");
    }
}