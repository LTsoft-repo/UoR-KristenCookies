using System.ComponentModel.DataAnnotations;
using KristenCookiesMvc.Infrastructure.Dtos;

namespace KristenCookiesMvc.Models;

public class OrderAddViewModel
{
    [ Required( ErrorMessage = "Please enter the customer's name." ) ]
    [ Display( Name = "Customer Name" ) ]
    public string CustomerName { get; set; } = string.Empty;

    public List<CookieItemDto> Cookies { get; set; } = new();
}