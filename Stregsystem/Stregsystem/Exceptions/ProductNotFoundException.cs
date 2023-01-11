using Stregsystem.Models;

namespace Stregsystem.Exceptions;

public class ProductNotFoundException : Exception
{
	public int ProductId { get; }
	public ProductNotFoundException(int productId) : base()
	{
		ProductId = productId;
	}

    public ProductNotFoundException(int productId, string? message) : base(message)
    {
        ProductId = productId;
    }
}