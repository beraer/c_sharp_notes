using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tutorial12.Models;

public class Country
{
    [Key]
    public int IdCountry { get; set; }
    public string Name { get; set; }

    public ICollection<CountryTrip> CountryTrips { get; set; }
} 