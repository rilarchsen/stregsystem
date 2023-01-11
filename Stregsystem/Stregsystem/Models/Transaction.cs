using Stregsystem.Interfaces;
using System.Numerics;

namespace Stregsystem.Models;

public class Transaction
{
    public int Id { get; }
    public User User { get; }
    public DateTime Date { get; }
    public double Amount { get; }
    
    private readonly ILogger<Transaction> _logger;

    public Transaction(int id, User user, DateTime date, double amount, ILogger<Transaction> logger)
    {
        Id = id;
        User = user;
        Date = date;
        Amount = amount;
        _logger = logger;
    }

    public void Execute(BalanceOperator balanceOperator)
    {
        //perform transaction
        switch(balanceOperator)
        {
            case BalanceOperator.Add:
                User.Balance += Amount;
                break;
            case BalanceOperator.Subtract: 
                User.Balance -= Amount;
                break;
        }

        //log transaction
        _logger.Log(ToString());
        _logger.Backlog.Add(this);
    }

    public override string ToString() => $"Date: {Date} Transaction: ID: {Id} Username: {User.Username} Amount: {Amount}kr";

    public enum BalanceOperator
    {
        Add = 0,
        Subtract = 1
    }
}