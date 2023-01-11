using Stregsystem.Models;

namespace Stregsystem.Exceptions;

public class ProductUnavailableForPurchaseException : Exception
{
    public User User { get; set; }
    public Product Product { get; set; }

    public ProductUnavailableForPurchaseException(User user, Product product) : base()
    {
        User = user;
        Product = product;
    }

    public ProductUnavailableForPurchaseException(User user, Product product, string? message) : base(message)
    {
        User = user;
        Product = product;
    }
}