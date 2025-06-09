using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GroupB.Models;

[Table("Purchased_Ticket")]
[PrimaryKey(nameof(TicketConcertId), nameof(CustomerId))]
public class PurchasedTicket
{
    [Required]
    public int TicketConcertId { get; set; }
    
    [Required]
    public int CustomerId { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime PurchaseDate { get; set; }
    
    [ForeignKey(nameof(TicketConcertId))] public TicketConcert TicketConcert { get; set; } = null!;
   
    [ForeignKey(nameof(CustomerId))] public Customer Customer { get; set; } = null!;
}