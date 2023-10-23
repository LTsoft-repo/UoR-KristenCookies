using Microsoft.AspNetCore.Mvc;

namespace KristenCookiesMvc.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> logger;

    public AccountController( ILogger<AccountController> logger )
    {
        this.logger = logger;
    }

    public IActionResult Index() => throw new NotImplementedException();
    //public IActionResult Index() => View();
}