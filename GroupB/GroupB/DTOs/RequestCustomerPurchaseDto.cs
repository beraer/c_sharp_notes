using System.ComponentModel.DataAnnotations.Schema;

namespace GroupB.DTOs;

public class RequestCustomerPurchaseDto
{
    public CustomerDto Customer { get; set; }
    public List <AddPurchaseDto> Purchases { get; set; } = new();
}

public class CustomerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
}

public class AddPurchaseDto
{
    public int SeatNumber { get; set; }
    public string ConcertName { get; set; }
    public Decimal Price { get; set; }
}