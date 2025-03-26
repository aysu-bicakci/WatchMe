// Path: /Users/dilaraselin/Desktop/WatchMe/ViewModels/SearchResultsViewModel.cs

using WatchMe.Models;

namespace WatchMe.ViewModels
{
    public class SearchResultsViewModel
    {
        public List<Movie> Movies { get; set; } = new List<Movie>();  // Varsayılan olarak boş bir liste
        public List<TVShow> TVShows { get; set; } = new List<TVShow>();  // Varsayılan olarak boş bir liste
    }
}