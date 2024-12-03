using System.ComponentModel.DataAnnotations;

public class RegisterRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    public string UserName { get; set; }
    public string TCKimlik { get; set; }

}