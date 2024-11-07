using Microsoft.AspNetCore.Mvc;

namespace WebXml.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}