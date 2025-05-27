using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using salalal.Models;

namespace salalal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //Logger koji se koristi za praćenje grešaka, informacija  i tome slicno
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Vraca na index stranicu
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //Akcija za prikazivanje stranice greške
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Blog()
        {
            return View();
        }
    }
}
