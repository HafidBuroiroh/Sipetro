using Microsoft.AspNetCore.Mvc;

namespace Login.Controllers
{
    public class SendController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
