using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tutorial12.Models;

public class Client
{
    [Key]
    public int IdClient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Pesel { get; set; }

    public ICollection<ClientTrip> ClientTrips { get; set; }
} 