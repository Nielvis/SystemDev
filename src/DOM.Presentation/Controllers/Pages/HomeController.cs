using System.Diagnostics;
using DOM.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DOM.Presentation.Controllers.Pages
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
            
        public IActionResult Test()
        {
            return View();
        }

        public IActionResult Text()
        {
            return View();
        }

        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Post()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }
        public  IActionResult User()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
