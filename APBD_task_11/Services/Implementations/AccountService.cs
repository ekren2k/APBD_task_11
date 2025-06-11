using APBD_task_11.DTOs.Account;
using APBD_task_11.Models;
using APBD_task_11.Repositories.Interfaces;
using APBD_task_11.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace APBD_task_11.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly PasswordHasher<Account> _passwordHasher = new();

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task RegisterAsync(CreateAccountDto accountDto)
    {
        if (string.IsNullOrWhiteSpace(accountDto.Username))
            throw new ArgumentException("Username cannot be empty", nameof(accountDto.Username));

        if (await _accountRepository.UsernameExistsAsync(accountDto.Username))
            throw new Exception("Username is already taken");

        var account = new Account
        {
            Username = accountDto.Username,
            Password = accountDto.Password,
            RoleId = accountDto.RoleId, //1 for user, 2 for admin
            EmployeeId = accountDto.EmployeeId
        };

        account.Password = _passwordHasher.HashPassword(account, accountDto.Password);
        await _accountRepository.AddAccountAsync(account);
    }
    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _accountRepository.UsernameExistsAsync(username);
    }

    public async Task<Account?> GetAccountByUsernameAsync(string username)
    {
        return await _accountRepository.GetAccountByUsernameAsync(username);
    }

    public async Task<Account> AddAccountAsync(Account account)
    {
        account.Password = _passwordHasher.HashPassword(account, account.Password);
        return await _accountRepository.AddAccountAsync(account);
    }
    public async Task<Account?> GetAccountByIdAsync(int id)
    {
        return await _accountRepository.GetAccountByIdAsync(id);
    }
    
    public async Task UpdateAccountAsync(int accountId, AccountDto dto)
    {
        var account = await _accountRepository.GetAccountByIdAsync(accountId);
        if (account == null)
            throw new KeyNotFoundException("Account not found");
        
        if (!string.IsNullOrWhiteSpace(dto.Username))
        {
            if (await _accountRepository.UsernameExistsAsync(dto.Username) && dto.Username != account.Username)
                throw new ArgumentException("Username is already taken");

            account.Username = dto.Username;
        }
        
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            account.Password = _passwordHasher.HashPassword(account, dto.Password);
        }
        
        await _accountRepository.UpdateAccountAsync(account);
    }
}