using DataModel.Model.Entities;

namespace KristenCookiesMvc.Models;

public record CookieListViewModel
{
    public string Name;
    public IEnumerable<Cookie> Cookies { get; init; } = Enumerable.Empty<Cookie>();
}