using DataModel.Model.Entities;

namespace KristenCookiesMvc.Models;

public class CustomerListViewModel
{
    public IEnumerable<Customer> Customers { get; set; } = Enumerable.Empty<Customer>();
}