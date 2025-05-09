using System.ComponentModel.DataAnnotations;
namespace Tutorial8.Models.DTOs;

public class ClientCreateDTO
{
    [Required] public string FirstName { get; set; }
    [Required] public string LastName  { get; set; }
    [Required, EmailAddress] public string Email { get; set; }
    public string Telephone { get; set; }
    [Required] public string Pesel { get; set; }
}