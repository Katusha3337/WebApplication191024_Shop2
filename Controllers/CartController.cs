using Microsoft.AspNetCore.Mvc;

namespace WebApplication191024_Shop.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
