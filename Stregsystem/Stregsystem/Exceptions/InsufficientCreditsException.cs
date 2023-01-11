using Stregsystem.Models;

namespace Stregsystem.Exceptions;

public class InsufficientCreditsException : Exception
{
    public User User { get; }
    public Product Product { get; }
    public InsufficientCreditsException(User user, Product product) : base()
    {
        User = user;
        Product = product;
    }

    public InsufficientCreditsException(User user, Product product, string? message) : base(message)
    {
        User = user;
        Product = product;
    }
}