using APBD_task_11.DTOs.Account;
using APBD_task_11.Models;
using APBD_task_11.Services.Implementations;
using APBD_task_11.Services.Interfaces;
using APBD_task_11.Services.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace APBD_task_11.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;
        private readonly PasswordHasher<Account> _passwordHasher = new();

        public AuthController(ITokenService tokenService, IAccountService accountService)
        {
            _tokenService = tokenService;
            _accountService = accountService;
        }

        [HttpPost]

        public async Task<IActionResult> Login([FromBody] AccountDto accountDto, CancellationToken cancellationToken)
        {
            var account = await _accountService.GetAccountByUsernameAsync(accountDto.Username);
            if (account == null)
                return Unauthorized("Invalid username or password.");
            
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(account, account.Password, accountDto.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid username or password.");
            
            var token = _tokenService.GenerateToken(account.Username, account.Role.Name);

            return Ok(new { token });
        }
    }
}
