namespace Stregsystem.Models;

public class Product
{
    public int Id { get; }
    public string Name { get; set; }
    public double Price { get; set; }
    public bool Active { get; set; }
    public bool CanBeBoughtOnCredit { get; set; }

    public Product(int id, string name, double price, bool active, bool canBeBoughtOnCredit = false)
    {
        Id = id;
        Name = name;
        Price = price;
        Active = active;
        CanBeBoughtOnCredit = canBeBoughtOnCredit;
    }

    public override string ToString() => $"{Id} {Name} {Price}kr";
}
