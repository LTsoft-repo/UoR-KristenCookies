using DataModel.Model.Entities;

namespace KristenCookiesMvc.Models;

public class CookieListViewModel
{
    public string Name;
    public IEnumerable<Cookie> Cookies { get; set; } = Enumerable.Empty<Cookie>();
}