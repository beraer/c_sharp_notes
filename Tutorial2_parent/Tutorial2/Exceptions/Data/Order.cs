namespace Tutorial2.Exceptions.Data;

public class Order
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = null!;
}