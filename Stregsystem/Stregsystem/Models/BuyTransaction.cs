using Stregsystem.Exceptions;
using Stregsystem.Interfaces;

namespace Stregsystem.Models;

public class BuyTransaction : Transaction
{
    public Product Product { get; }
    public int Count { get; }

    public BuyTransaction(int id, User user, DateTime date, Product product, ILogger<Transaction> logger, int count = 1) : base(id, user, date, product.Price * count, logger)
    {
        Product = product;
        Count = count;
    }

    public void Execute()
    {
        if (!Product.Active)
            throw new ProductUnavailableForPurchaseException(User, Product, "Error: The product you have selected is unavailable for purchase");

        if (Amount > User.Balance && !Product.CanBeBoughtOnCredit)
            throw new InsufficientCreditsException(this, "Error: You do not have sufficient credits available for this purchase");

        base.Execute(BalanceOperator.Subtract);
    }

    public override string ToString() => $"Date: {Date} Purchase: ID: {Id} Username: {User.Username} Total Amount: {Amount}kr Product ID: {Product.Id} Count: {Count}";
}