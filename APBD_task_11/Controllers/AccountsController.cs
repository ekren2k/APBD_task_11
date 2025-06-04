using APBD_task_11.DTOs.Account;
using APBD_task_11.Models;
using APBD_task_11.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APBD_task_11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {



        private IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AccountDto newAccount)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.RegisterAsync(newAccount);

            return CreatedAtAction(nameof(Register), new { username = newAccount.Username }, null);
        }
    }
}
