using Microsoft.AspNetCore.Mvc;

namespace webowka1.Controllers
{
    public class CoController : Controller
    {
        public int Id { get; internal set; }

        public IActionResult Index()
        {
            return View();
        }
        public string Welcome()
        {
            return "Co Ty tutaj robisz?";
        }
    }
}
