using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ShopAlp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}