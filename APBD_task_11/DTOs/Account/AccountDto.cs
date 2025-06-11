using System.ComponentModel.DataAnnotations;

namespace APBD_task_11.DTOs.Account;

public class AccountDto
{
    [Required]

    public string Username { get; set; }
    [Required] 
    public string Password { get; set; }
    
}