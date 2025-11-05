using System.Diagnostics;
using AspMvcDemo.Models;
using AspMvcDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspMvcDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public virtual IMyService _myService { get; set; }

        public HomeController(ILogger<HomeController> logger, IMyService myService)
        {
            _logger = logger;
            _myService = myService;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index");

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
