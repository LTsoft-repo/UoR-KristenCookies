using DataModel.Model.Entities;

namespace KristenCookiesMvc.Infrastructure.Dtos;

public class OrderListItemDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public int Total { get; set; }
    public List<CookieItemDto> Cookies { get; set; } = new();

    public static OrderListItemDto FromOrder( Order order )
    {
        return new()
        {
            Id = order.Id,
            OrderDate = order.OrderedAtUtc,
            CustomerName = order.Customer.Name,
            Total = order.Cookies.Sum( c => c.Quantity ),
            Cookies = order.Cookies
                .Select( c => new CookieItemDto
                {
                    Id = c.CookieId,
                    Name = c.Cookie.Name,
                    Quantity = c.Quantity
                } )
                .ToList()
        };
    }
}