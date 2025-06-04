using APBD_task_11.DTOs.Account;
using APBD_task_11.Models;

namespace APBD_task_11.Services.Interfaces;

public interface IAccountService
{
    public Task RegisterAsync(CreateAccountDto accountDto);
    Task<bool> UsernameExistsAsync(string username);
    Task<Account?> GetAccountByUsernameAsync(string username);
    Task<Account> AddAccountAsync(Account account);
    Task<Account?> GetAccountByIdAsync(int id);
    Task UpdateAccountAsync(int id, AccountDto dto);
}