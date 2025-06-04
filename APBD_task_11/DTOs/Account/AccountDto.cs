using System.ComponentModel.DataAnnotations;

namespace APBD_task_11.DTOs.Account;

public class AccountDto
{
    [MaxLength(60)]
    [Required]
    [RegularExpression(@"^[^\d].*", ErrorMessage = "Username must not start with a digit")]

    public string Username { get; set; }
    [Required]
    [MaxLength(32)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must have at least one lowercase letter, one uppercase letter, one number, and one special character.")]
    public string Password { get; set; }
    
}