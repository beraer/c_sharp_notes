using System.ComponentModel.DataAnnotations;

namespace GroupB.Models;

public class Concert
{
    [Key]
    public int ConcertId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [Required]
    [DataType(DataType.Date)]   
    public DateTime Date { get; set; }
    
    [Required]
    public int AvailableTickets { get; set; }
    
    public ICollection<TicketConcert> TicketConcerts { get; set; } = new HashSet<TicketConcert>();

}