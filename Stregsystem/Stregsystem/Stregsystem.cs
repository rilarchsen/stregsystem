using Stregsystem.Interfaces;
using Stregsystem.Models;
using Stregsystem.Exceptions;

namespace Stregsystem;

public delegate void UserBalanceNotification(User user, decimal balance);

public class Stregsystem : IStregsystem
{
    public ILogger<Transaction> Logger { get; }

    public Stregsystem(ILogger<Transaction> logger)
    {
        Logger = logger;
    }
    public IEnumerable<Product> ActiveProducts { get => GetAllProducts().Where(p => p.Active); }

    public event UserBalanceNotification UserBalanceWarning;

    public InsertCashTransaction AddCreditsToAccount(User user, int amount)
    {
        int id = 0;

        if (Logger.Backlog.Count > 0)
            id = Logger.Backlog[Logger.Backlog.Count - 1].Id + 1;

        InsertCashTransaction transaction = new(id, user, DateTime.Now, amount, Logger);
        transaction.Execute();
        return transaction;
    }

    public BuyTransaction BuyProduct(User user, Product product)
    {
        int id = 0;

        if (Logger.Backlog.Count > 0)
            id = Logger.Backlog[Logger.Backlog.Count - 1].Id + 1;

        BuyTransaction transaction = new(id, user, DateTime.Now, product, Logger);
        transaction.Execute();
        return transaction;
    }

    public Product GetProductByID(int id)
    {
        Product? product = GetAllProducts().FirstOrDefault(p => p.Id == id);
        
        if (product == default)
            throw new ProductNotFoundException(id, $"Error: Product with ID {id} not found");
        
        return product;
    }

    public IEnumerable<Transaction> GetTransactions(User user, int count)
    {
        List<Transaction> transactions = Logger.Backlog.Where(t => t.User == user).ToList();
        transactions.Sort(new Comparison<Transaction>((t1, t2) => t2.Id.CompareTo(t1.Id)));
        return transactions.Take(count);
    }

    public User GetUserByUsername(string username)
    {
        User? user = GetUsers(p => p.Username == username);

        if (user == default)
            throw new UserNotFoundException(username, $"Error: User with username {username} not found");

        return user;
    }

    public User GetUsers(Func<User, bool> predicate)
    {
        return GetAllUsers().First(predicate);
    }

    public Product[] GetAllProducts()
    {
        string[] lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "products.csv"));
        Product[] products = new Product[lines.Length - 1];
        for (int i = 1; i < lines.Length; i++)
        {
            string[] split = lines[i].Split(';');
            products[i - 1] = new Product(int.Parse(split[0]), split[1], double.Parse(split[2]), split[3] == "1");
        }
        return products;
    }

    public User[] GetAllUsers()
    {
        string[] lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "users.csv"));
        User[] users = new User[lines.Length - 1];
        for (int i = 1; i < lines.Length; i++)
        {
            string[] split = lines[i].Split(';');
            users[i - 1] = new User(int.Parse(split[0]), split[1], split[2], split[3], split[5], double.Parse(split[4]));
        }
        return users;
    }
}