namespace Ordering.Domain.Models;

public class Customer : Entity<CustomerId>
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;

    public static Customer Create(string name, string email) => Create(CustomerId.Of(Guid.NewGuid()), name, email);
    public static Customer Create(CustomerId id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        var customer = new Customer
        {
            Id = id,
            Name = name,
            Email = email,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };

        return customer;
    }
}
