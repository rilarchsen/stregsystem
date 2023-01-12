using Stregsystem.Models;

namespace Stregsystem.Interfaces;

public interface IStregsystem
{
    IEnumerable<Product> ActiveProducts { get; }
    InsertCashTransaction AddCreditsToAccount(User user, int amount);
    BuyTransaction BuyProduct(User user, Product product, int count = 1);
    Product GetProductByID(int id);
    IEnumerable<Transaction> GetTransactions(User user, int count);
    User GetUsers(Func<User, bool> predicate);
    User GetUserByUsername(string username);
    void SetProductStatus(int id, bool active);
    void SetProductCreditStatus(int id, bool allowed);
    event UserBalanceNotification UserBalanceWarning;
}
