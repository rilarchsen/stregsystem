using Stregsystem.Models;

namespace Stregsystem.Exceptions;

public class InsufficientCreditsException : Exception
{
    public BuyTransaction Transaction { get; }
    public InsufficientCreditsException(BuyTransaction transaction) : base()
    {
        Transaction = transaction;
    }

    public InsufficientCreditsException(BuyTransaction transaction, string? message) : base(message)
    {
        Transaction = transaction;
    }
}