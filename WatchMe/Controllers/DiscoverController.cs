using Microsoft.AspNetCore.Mvc;

public class DiscoverController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}