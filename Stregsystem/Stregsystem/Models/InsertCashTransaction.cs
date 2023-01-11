namespace Stregsystem.Models;

public class InsertCashTransaction : Transaction
{
    public InsertCashTransaction(int id, User user, DateTime date, double amount) : base(id, user, date, amount)
    {
    }

    public override void Execute()
    {
        User.Balance += Amount;

        //log transaction
    }

    public override string ToString() => $"Cash insert: {base.ToString()}";
}