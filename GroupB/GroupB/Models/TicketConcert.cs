using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupB.Models;

[Table("TicketConcert")]
public class TicketConcert
{
    [Key]
    public int TicketConcertId { get; set; }
    
    [Required]
    public int TicketId { get; set; }
    
    [Required]
    public int ConcertId { get; set; }
    
    [Required]
    [Range(typeof(decimal), "0.01", "99999999.99")]
    public decimal Price { get; set; }

    [ForeignKey(nameof(ConcertId))] public Concert Concert { get; set; } = null!;
    [ForeignKey(nameof(TicketId))] public Ticket Ticket { get; set; } = null!;

    public ICollection<PurchasedTicket> PurchaseTickets { get; set; } = new HashSet<PurchasedTicket>();
}