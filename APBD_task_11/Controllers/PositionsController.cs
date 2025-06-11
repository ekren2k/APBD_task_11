using APBD_task_11.Services.Interfaces;
using APBD_task_11.Services.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD_task_11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionsController(IPositionService positionService)
        {
            _positionService = positionService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _positionService.GetAllPositionsAsync();
            return Ok(roles);
        }
    }
}
