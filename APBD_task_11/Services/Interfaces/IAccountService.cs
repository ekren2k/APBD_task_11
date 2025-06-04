using APBD_task_11.DTOs.Account;
using APBD_task_11.Models;

namespace APBD_task_11.Services.Interfaces;

public interface IAccountService
{
    public Task RegisterAsync(AccountDto accountDto);
    Task<bool> UsernameExistsAsync(string username);
    Task<Account?> GetAccountByUsernameAsync(string username);
    Task<Account> AddAccountAsync(Account account);
}