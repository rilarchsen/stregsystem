using Stregsystem.Interfaces;

namespace Stregsystem.Models;

public class InsertCashTransaction : Transaction
{
    public InsertCashTransaction(int id, User user, DateTime date, double amount, ILogger<Transaction> logger) : base(id, user, date, amount, logger)
    {
    }

    public void Execute()
    {
        base.Execute(BalanceOperator.Add);
    }

    public override string ToString() => $"Cash insert: {base.ToString()}";
}