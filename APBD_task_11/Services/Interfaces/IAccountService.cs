using APBD_task_11.DTOs.Account;

namespace APBD_task_11.Services.Interfaces;

public interface IAccountService
{
    public Task RegisterAsync(AccountDto accountDto);
}