using DataModel.Model.Entities;
using KristenCookiesMvc.Models;
using Logic.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KristenCookiesMvc.Controllers;

[ Authorize ]
[ Route( "customers" ) ]
public class CustomerController : Controller
{
    private readonly ICustomerManager<Customer> customerManager;

    public CustomerController( ICustomerManager<Customer> customerManager )
    {
        this.customerManager = customerManager;
    }

    [ Route( "" ) ]
    public IActionResult Index() => NotFound();

    [ Route( "list" ) ]
    public async Task<IActionResult> List()
    {
        var result = await customerManager.GetAllAsync();

        var model = new CustomerListViewModel
        {
            Customers = result.Data ?? Enumerable.Empty<Customer>()
        };

        return View( model );
    }
}