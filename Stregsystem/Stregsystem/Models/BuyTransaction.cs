using Stregsystem.Exceptions;

namespace Stregsystem.Models;

public class BuyTransaction : Transaction
{
    public Product Product { get; }


    public BuyTransaction(int id, User user, DateTime date, Product product) : base(id, user, date, product.Price)
    {
        Product = product;
    }

    public override void Execute()
    {
        if (!Product.Active)
            throw new ProductUnavailableForPurchaseException(User, Product, "Error: The product you have selected is unavailable for purchase");

        if (Product.Price > User.Balance && !Product.CanBeBoughtOnCredit)
            throw new InsufficientCreditsException(User, Product, "Error: You do not have sufficient credits available for this purchase");

        User.Balance -= Product.Price;

        //log transaction
    }

    public override string ToString() => $"Purchase: ID: {Id} Username: {User.Username} Amount: {Amount}kr Date: {Date} Product ID: {Product.Id}";
}