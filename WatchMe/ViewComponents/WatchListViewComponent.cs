using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchMe.Data;
using WatchMe.Models; // Model'lerinizi içeren namespace

namespace WatchMe.ViewComponents
{
    public class WatchListViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public WatchListViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int userId)
        {
            // userId'ye göre izlenenler listesini çek
            var watchList = await _context.MovieWatchLists
                .Where(mwl => mwl.UserId == userId)
                .Include(mwl => mwl.Movie) // Movie tablosuyla ilişkili veriyi de getir
                .Select(mwl => mwl.Movie)  // Movie nesnelerini seçiyoruz
                .ToListAsync();

            return View(watchList);
        }
    }
}
