using DataModel.Model.Entities;

namespace KristenCookiesMvc.Infrastructure.Dtos;

public class CookieItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }

    public static CookieItemDto FromCookie( Cookie cookie ) => new()
    {
        Id = cookie.Id,
        Name = cookie.Name,
        Quantity = 0
    };

    public static IEnumerable<OrderItem> ToOderItems( IEnumerable<CookieItemDto> cookies )
    {
        return cookies
            .Where( c => c.Quantity > 0 )
            .Select( c => new OrderItem
            {
                CookieId = c.Id,
                Quantity = c.Quantity
            } );
    }
}