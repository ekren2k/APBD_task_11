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

    public async Task RegisterAsync(AccountDto accountDto)
    {
        if (string.IsNullOrWhiteSpace(accountDto.Username))
            throw new ArgumentException("Username cannot be empty", nameof(accountDto.Username));

        if (await _accountRepository.UsernameExistsAsync(accountDto.Username))
            throw new Exception("Username is already taken");

        var account = new Account
        {
            Username = accountDto.Username,
            Password = accountDto.Password,
            RoleId = 1 //1 for user, 2 for admin
        };

        account.Password = _passwordHasher.HashPassword(account, accountDto.Password);
        await _accountRepository.AddAccountAsync(account);
    }
}