namespace Stregsystem.Models;

public class Transaction
{
    public int Id { get; }
    public User User { get; }
    public DateTime Date { get; }
    public double Amount { get; }

    public Transaction(int id, User user, DateTime date, double amount)
    {
        Id = id;
        User = user;
        Date = date;
        Amount = amount;
    }

    public virtual void Execute()
    {

    }

    public override string ToString() => $"ID: {Id} Username: {User.Username} Amount: {Amount}kr Date: {Date}";
}