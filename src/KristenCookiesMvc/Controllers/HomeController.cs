using System.Diagnostics;
using KristenCookiesMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace KristenCookiesMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    public HomeController( ILogger<HomeController> logger )
    {
        this.logger = logger;
    }

    public IActionResult Index() => View();

    public IActionResult Privacy() => View();

    [ ResponseCache( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true ) ]
    public IActionResult Error() => View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
}