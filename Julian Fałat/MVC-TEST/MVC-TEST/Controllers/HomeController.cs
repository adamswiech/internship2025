using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_TEST.Models;

namespace MVC_TEST.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Testowy()
        {
            List<Person> lista = new List<Person>();
            lista.Add(new Person { Name = "Alice", Age = 30 });
            lista.Add(new Person { Name = "Bob", Age = 25 });

            return View(lista);
        }
        public IActionResult Formularz()
        {
            List<Person> listaOsob = new List<Person>();
            return View(listaOsob);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
