//IMovieSearchService interface
using System.ServiceModel;
using WatchMe.Models;
using System.Collections.Generic;

namespace WatchMe.Services
{
    [ServiceContract]
    public interface IMovieSearchService
    {
        [OperationContract]
        List<Movie> SearchMovies(string title);

        [OperationContract]
        List<TVShow> SearchTVShows(string title);
    }
}