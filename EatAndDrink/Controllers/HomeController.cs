using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EatAndDrink.Models;
using Microsoft.AspNetCore.Authorization;

namespace EatAndDrink.Controllers;

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

    [Authorize(Roles = "Admin")]
    public IActionResult OnlyAdmin()
    {
        return View();
    }
}
