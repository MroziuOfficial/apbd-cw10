using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models;

public class Login
{
    [Required]
    public string Username { get; set; }
    public string Password { get; set; }
}