using APBD_task_11.Contexts;
using APBD_task_11.Models;
using APBD_task_11.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APBD_task_11.Repositories.Implementations;

public class AccountRepository : IAccountRepository
{
    
    MasterContext _context;

    public AccountRepository(MasterContext context)
    {
        _context = context;
    }
    
    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Accounts.AnyAsync(a => a.Username == username);
    }

    public async Task<Account?> GetAccountByUsernameAsync(string username)
    {
        return await _context.Accounts
            .Include(a => a.Role)
            .FirstOrDefaultAsync(a => a.Username == username);
    }

    public async Task<Account> AddAccountAsync(Account account)
    {
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return account;
    }
}