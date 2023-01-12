using Stregsystem.Interfaces;
using Stregsystem.Models;
using System.Text;

namespace Stregsystem;

public delegate void StregsystemEvent(string command);

public class StregsystemCLI : IStregsystemUI
{
    private readonly IStregsystem _stregsystem;

    public StregsystemCLI(IStregsystem stregsystem)
    {
        _stregsystem = stregsystem;
    }

    public event StregsystemEvent CommandEntered;

    public void Close()
    {
        Environment.Exit(0);
    }

    public void DisplayAdminCommandNotFoundMessage(string adminCommand)
    {
        Console.WriteLine($"Error: Admin command \"{adminCommand}\"not found");
    }

    public void DisplayGeneralError(string errorString)
    {
        Console.WriteLine($"Error: {errorString}");
    }

    public void DisplayInsufficientCash(BuyTransaction transaction)
    {
        Console.WriteLine($"Unable to purchase {transaction.Count} x \"{transaction.Product.Name}\" for {transaction.Amount}kr. User {transaction.User.Username} has insufficient credits ({transaction.User.Balance}kr).");
    }

    public void DisplayUserBalanceNotification(User user, double balance)
    {
        Console.WriteLine($"Warning: User {user.Username} has only {balance}kr left!");
    }

    public void DisplayProductNotFound(string product)
    {
        Console.WriteLine($"Product\"{product}\" could not be found");
    }

    public void DisplayTooManyArgumentsError(string command)
    {
        DisplayGeneralError($"Too many arguments");
    }

    public void DisplayTooFewArgumentsError(string command)
    {
        DisplayGeneralError($"Too few arguments");
    }

    public void DisplayUserBuysProduct(BuyTransaction transaction)
    {
        Console.WriteLine($"{transaction.User.Username} has purchased \"{transaction.Product.Name}\" for {transaction.Amount}kr");
    }

    public void DisplayUserBuysProduct(int count, BuyTransaction transaction)
    {
        Console.WriteLine($"{transaction.User.Username} has purchased {count} x \"{transaction.Product.Name}\" for a total of {transaction.Amount}kr");
    }

    public void DisplayUserInfo(User user)
    {
        Console.WriteLine("User info:");
        Console.WriteLine("-------------------------");
        Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
        Console.WriteLine($"Username: {user.Username}");
        Console.WriteLine($"Email: {user.Email}");
        Console.WriteLine($"Balance: {user.Balance}");
    }

    public void DisplayUserNotFound(string username)
    {
        DisplayGeneralError($"User with username \"{username}\" could not be found");
    }

    public void Start()
    {
        List<Product> activeProducts = _stregsystem.ActiveProducts.ToList();
        Console.Clear();
        ConsoleWriteLineCentered("STREGSYSTEM");
        Console.WriteLine();

        //write table header
        StringBuilder sb = new StringBuilder();
        sb.Append('=', Console.BufferWidth);
        Console.WriteLine(sb.ToString());
        Console.WriteLine();
        
        int maxProductNameLength = FindMaxProductNameLength(activeProducts);
        int idLength = 4;
        int priceLength = 6;
        
        sb.Clear();
        sb.Append('-', maxProductNameLength + idLength + priceLength + 4);
        ConsoleWriteLineCentered(sb.ToString());

        sb.Clear();
        int leftPadding = (Console.WindowWidth - maxProductNameLength - idLength - priceLength - 4) / 2;
        sb.Append(' ', leftPadding);
        Console.Write(sb.ToString());

        Console.Write("|");

        Console.Write(" ID ");
        Console.Write("|");

        sb.Clear();
        sb.Append(' ', (maxProductNameLength - "Product".Length) / 2);
        sb.Append("Product");
        Console.Write(sb.ToString().PadRight(maxProductNameLength, ' '));
        Console.Write("|");

        sb.Clear();
        sb.Append(' ', (priceLength - "Price".Length) / 2);
        sb.Append("Price");
        Console.Write(sb.ToString().PadRight(priceLength, ' '));
        Console.Write("|");
        Console.WriteLine();

        sb.Clear();
        sb.Append(' ', leftPadding);
        sb.Append('-', maxProductNameLength + idLength + priceLength + 4);
        Console.WriteLine(sb.ToString());

        //write table body
        foreach (Product product in activeProducts)
        {
            sb.Clear();
            sb.Append(' ', leftPadding);
            sb.Append("|");
            Console.Write(sb.ToString());
            sb.Clear();
            
            int idLeftPadding = (idLength - product.Id.ToString().Length) / 2;
            sb.Append(' ', idLeftPadding);
            sb.Append(product.Id.ToString());
            Console.Write(sb.ToString().PadRight(idLength, ' '));
            Console.Write("|");

            sb.Clear();
            int productLeftPadding = (maxProductNameLength - product.Name.ToString().Length) / 2;
            sb.Append(' ', productLeftPadding);
            sb.Append(product.Name.ToString());
            Console.Write(sb.ToString().PadRight(maxProductNameLength, ' '));
            Console.Write("|");

            sb.Clear();
            string priceString = product.Price.ToString() + "kr";
            int priceLeftPadding = (priceLength - priceString.Length) / 2;
            sb.Append(' ', priceLeftPadding);
            sb.Append(priceString);
            Console.Write(sb.ToString().PadRight(priceLength, ' '));
            Console.Write("|");

            Console.WriteLine();
        }

        sb.Clear();
        sb.Append('-', maxProductNameLength + idLength + priceLength + 4);
        ConsoleWriteLineCentered(sb.ToString());

        Console.WriteLine();
        Console.Write(" > ");

        string command = Console.ReadLine();
        CommandEntered.Invoke(command);
    }

    private void ConsoleWriteLineCentered(string message)
    {
        StringBuilder s = new StringBuilder();
        s.Append(' ', (Console.WindowWidth - message.Length) / 2);
        s.Append(message);
        string m = s.ToString();
        Console.WriteLine(m);
    }

    private int FindMaxProductNameLength(IEnumerable<Product> products)
    {
        int length = 0;
        foreach (Product product in products)
        {
            if (product.Name.Length > length)
                length = product.Name.Length;
        }
        return length;
    }
}