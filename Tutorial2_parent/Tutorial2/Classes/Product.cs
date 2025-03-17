using System.Runtime.InteropServices.JavaScript;

namespace Tutorial2.Classes;

public class Product
{
    private static int _idCounter = 1;
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Type { get; set; }
    public string SerialNumber => $"PROD-{Type}-{Id}";
    
    public Product(string name, double price, bool isActive, string type)
    {
        Id = _idCounter++;
        Name = name;
        Price = price;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        Type = type;
        
    }
    
}