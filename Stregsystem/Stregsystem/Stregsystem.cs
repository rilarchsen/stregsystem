using Stregsystem.Interfaces;
using Stregsystem.Models;
using Stregsystem.Exceptions;
using System.Text.RegularExpressions;

namespace Stregsystem;

public delegate void UserBalanceNotification(User user, double balance);

public class Stregsystem : IStregsystem
{
    public ILogger<Transaction> Logger { get; }
    private List<Product> _allProducts;
    private List<User> _allUsers;

    public Stregsystem(ILogger<Transaction> logger)
    {
        Logger = logger;
        _allProducts = LoadAllProducts().ToList();
        _allUsers = LoadAllUsers().ToList();
    }
    public IEnumerable<Product> ActiveProducts { get => _allProducts.Where(p => p.Active); }

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

    public BuyTransaction BuyProduct(User user, Product product, int count = 1)
    {
        int id = 0;

        if (Logger.Backlog.Count > 0)
            id = Logger.Backlog[Logger.Backlog.Count - 1].Id + 1;

        BuyTransaction transaction = new(id, user, DateTime.Now, product, Logger, count);

        transaction.Execute();

        if (user.Balance < 50)
            UserBalanceWarning.Invoke(user, user.Balance);

        return transaction;
    }

    public Product GetProductByID(int id)
    {
        Product? product = _allProducts.FirstOrDefault(p => p.Id == id);
        
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
        try
        {
            return GetUsers(p => p.Username == username);
        }
        catch (UserNotFoundException)
        {
            throw new UserNotFoundException(username, $"Error: User with username {username} not found");
        }
    }

    public User GetUsers(Func<User, bool> predicate)
    {
        try
        {
            return _allUsers.First(predicate);
        }
        catch (Exception e)
        {
            throw new UserNotFoundException(e.Message);
        }
    }

    public Product[] LoadAllProducts()
    {
        string[] lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "products.csv"));
        Product[] products = new Product[lines.Length - 1];
        for (int i = 1; i < lines.Length; i++)
        {
            string[] split = lines[i].Split(';');
            string name = StripHTML(split[1].Replace("\"", "").Trim());
            products[i - 1] = new Product(int.Parse(split[0]), name, double.Parse(split[2]), split[3] == "1");
        }
        return products;
    }

    public User[] LoadAllUsers()
    {
        string[] lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "users.csv"));
        User[] users = new User[lines.Length - 1];
        for (int i = 1; i < lines.Length; i++)
        {
            string[] split = lines[i].Split(',');
            users[i - 1] = new User(int.Parse(split[0]), split[1], split[2], split[3], split[5], double.Parse(split[4]));
        }
        return users;
    }

    public void SetProductStatus(int id, bool active)
    {
        Product product = GetProductByID(id);

        if (product is not SeasonalProduct)
        {
            product.Active = active;
        }

    }

    public void SetProductCreditStatus(int id, bool allowed)
    {
        Product product = GetProductByID(id);
        product.CanBeBoughtOnCredit = allowed;
    }

    private static string StripHTML(string input)
    {
        return Regex.Replace(input, "<.*?>", String.Empty);
    }
}