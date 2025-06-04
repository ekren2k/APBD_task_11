using APBD_task_11.Models;

namespace APBD_task_11.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<bool> UsernameExistsAsync(string username);
    
    Task<Account?> GetAccountByUsernameAsync(string username);
    
    Task<Account> AddAccountAsync(Account account);
    Task<Account?> GetAccountByIdAsync(int id);
    Task UpdateAccountAsync(Account account); 
}