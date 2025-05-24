using System.ComponentModel.DataAnnotations;

namespace Tut11.Domain.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; }

    public int Age { get; set; }
}