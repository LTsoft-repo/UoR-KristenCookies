using KristenCookiesMvc.Infrastructure.Dtos;

namespace KristenCookiesMvc.Models;

public class OrderAddViewModel
{
    public CustomerDto Customer { get; set; } = new();

    public List<CookieItemDto> Cookies { get; set; } = new();
}