using System.ComponentModel.DataAnnotations;

namespace KristenCookiesMvc.Infrastructure.Dtos;

public class CustomerDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = string.Empty;

    [ Required( ErrorMessage = "Please enter the customer's email address." ) ]
    public string Email { get; set; } = string.Empty;
}