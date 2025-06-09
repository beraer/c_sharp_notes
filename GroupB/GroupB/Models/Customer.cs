using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupB.Models;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int CustomerId { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
    
    [MaxLength(100)]
    public string? PhoneNumber { get; set; }
    
    public ICollection<PurchasedTicket> PurchasedTickets { get; set; } = new HashSet<PurchasedTicket>();
}