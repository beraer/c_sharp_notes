using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupB.Models;

[Table("Ticket")]
public class Ticket
{
    [Key]
    public int TicketId { get; set; }
    
    [StringLength(50)]
    [Required]
    public string SerialNumber { get; set; }
    
    [Required]
    public int SeatNumber { get; set; }

    public ICollection<TicketConcert> TicketConcerts { get; set; } = new HashSet<TicketConcert>();
}