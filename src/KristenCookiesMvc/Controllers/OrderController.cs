using DataModel.Model.Entities;
using KristenCookiesMvc.Infrastructure.Dtos;
using KristenCookiesMvc.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KristenCookiesMvc.Controllers;

[ Authorize ]
[ Route( "orders" ) ]
public class OrderController : Controller
{
    private readonly ICookieManager<Cookie> cookieManager;
    private readonly ILogger<AccountController> logger;
    private readonly IOrderManager<Order> orderManager;

    public OrderController(
        ICookieManager<Cookie> cookieManager,
        IOrderManager<Order> orderManager,
        ILogger<AccountController> logger )
    {
        this.cookieManager = cookieManager;
        this.orderManager = orderManager;
        this.logger = logger;
    }

    [ Route( "" ) ]
    public IActionResult Index() => NotFound();

    [ Route( "list" ) ]
    public async Task<IActionResult> List()
    {
        var result = await orderManager.GetHistoryAsync();

        var model = new OrderListViewModel
        {
            Orders = result.Data?.Select( OrderListItemDto.FromOrder ) ??
                     new List<OrderListItemDto>()
        };

        return View( model );
    }

    [ Route( "add" ) ]
    public async Task<IActionResult> Add()
    {
        var resultCookies = await cookieManager.GetAllAsync();

        var model = new OrderAddViewModel
        {
            Customer = new(),
            Cookies = resultCookies.Data?.Select( CookieItemDto.FromCookie ).ToList() ??
                      new()
        };

        return View( model );
    }

    [ Route( "add" ) ]
    [ HttpPost ]
    [ ValidateAntiForgeryToken ]
    public async Task<IActionResult> AddPost( OrderAddViewModel model )
    {
        model.Customer.Name = model.Customer.Name?.Trim() ?? "";
        model.Customer.Email = model.Customer.Email?.Trim() ?? "";

        var order = new Order
        {
            Customer = new()
            {
                Email = model.Customer.Email,
                Name = model.Customer.Name
            },
            Cookies = CookieItemDto.ToOderItems( model.Cookies ).ToList()
        };

        var result = await orderManager.PlaceOrderAsync( order );

        if( result.Success )
        {
            return RedirectToAction( nameof( Add ), "Order" );
        }

        ModelState.AddModelError( "", result.Error );

        return View( nameof( Add ), model );
    }
}