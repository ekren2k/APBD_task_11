using System.Security.Claims;
using APBD_task_11.DTOs.Account;
using APBD_task_11.Models;
using APBD_task_11.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APBD_task_11.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        
        private IAccountService _accountService;
        private IEmployeeService _employeeService;

        public AccountController(IAccountService service, IEmployeeService employeeService)
        {
            _accountService = service;
            _employeeService = employeeService;
            
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateAccountDto newAccount)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _employeeService.GetEmployeeByIdAsync(newAccount.EmployeeId);
            if (employee == null)
                throw new ArgumentException("Employee not found");
            
            await _accountService.RegisterAsync(newAccount);

            return CreatedAtAction(nameof(Register), new { username = newAccount.Username }, null);
        }
    }
}
