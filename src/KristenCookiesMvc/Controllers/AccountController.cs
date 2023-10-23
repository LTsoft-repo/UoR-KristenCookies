using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KristenCookiesMvc.Controllers;

[ AllowAnonymous ] [ Route( "account" ) ]
public class AccountController : Controller
{
    private readonly ILogger<AccountController> logger;

    public AccountController( ILogger<AccountController> logger )
    {
        this.logger = logger;
    }

    public IActionResult Index() => NotFound();
    //public IActionResult Index() => View();

    [ Route( "google-login" ) ]
    public IActionResult GoogleLogin( string? returnUrl = null )
    {
        //var properties = new AuthenticationProperties { RedirectUri = Url.Action( "GoogleResponse" ) };
        var properties = new AuthenticationProperties { RedirectUri = returnUrl };

        return Challenge( properties, GoogleDefaults.AuthenticationScheme );
    }

    [ Route( "google-response" ) ]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync( CookieAuthenticationDefaults.AuthenticationScheme );

        var claims = result.Principal.Identities
            .FirstOrDefault()
            .Claims.Select( claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            } );

        return Json( claims );
    }
}