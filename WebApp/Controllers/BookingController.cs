using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class BookingController : Controller
{
    // GET
    public IActionResult ChooseSessions()
    {
        ViewData["Message"] = "Hopefully this works";

        return View();
    }
}