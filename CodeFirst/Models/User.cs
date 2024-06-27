using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
    public string Salt { get; set; }
    public string RefreshToken { get; set; }
}