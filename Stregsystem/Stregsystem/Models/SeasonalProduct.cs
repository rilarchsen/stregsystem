namespace Stregsystem.Models;

public class SeasonalProduct : Product
{
	public DateTime SeasonStartDate { get; set; }
    public DateTime SeasonEndDate { get; set; }
    new public bool Active { get => DateTime.Now.CompareTo(SeasonStartDate) >= 0 && DateTime.Now.CompareTo(SeasonEndDate) < 0; } 

	public SeasonalProduct(int id, string name, double price, DateTime seasonStartDate, DateTime seasonEndDate, bool canBeBoughtOnCredit = false) : base(id, name, price, true, canBeBoughtOnCredit)
	{
		SeasonStartDate = seasonStartDate;
		SeasonEndDate = seasonEndDate;
	}
}
