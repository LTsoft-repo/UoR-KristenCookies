using DataModel.Model.Entities;
using KristenCookiesMvc.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KristenCookiesMvc.Controllers;

[ Authorize ]
[ Route( "cookies" ) ]
public class CookieController : Controller
{
    private readonly ICookieManager<Cookie> cookieManager;
    private readonly ILogger<AccountController> logger;

    public CookieController(
        ICookieManager<Cookie> cookieManager,
        ILogger<AccountController> logger )
    {
        this.cookieManager = cookieManager;
        this.logger = logger;
    }

    [ Route( "" ) ]
    public IActionResult Index() => NotFound();

    [ Route( "list" ) ]
    public async Task<IActionResult> List()
    {
        if( TempData.ContainsKey( "ErrorMessage" ) )
        {
            var message = TempData["ErrorMessage"]?.ToString() ?? "";

            if( !string.IsNullOrEmpty( message ) )
            {
                ModelState.AddModelError( "", message );
            }
        }

        var result = await cookieManager.GetAllAsync();

        var model = new CookieListViewModel
        {
            Cookies = result.Data ?? Enumerable.Empty<Cookie>()
        };

        return View( model );
    }

    [ Route( "add" ) ]
    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> Add( string name )
    {
        var result = await cookieManager.AddAsync( name.Trim() );

        if( !result.Success )
        {
            TempData["ErrorMessage"] = result.Error;
        }

        return RedirectToAction( nameof( List ), "Cookie" );
    }
}