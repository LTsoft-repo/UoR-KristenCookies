using KristenCookiesMvc.Infrastructure.Dtos;

namespace KristenCookiesMvc.Models;

public class OrderListViewModel
{
    public IEnumerable<OrderListItemDto> Orders { get; init; } = Enumerable.Empty<OrderListItemDto>();
}