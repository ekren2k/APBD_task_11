using System.Security.Claims;
using APBD_task_11.DTOs.Device;
using APBD_task_11.Services.Implementations;
using APBD_task_11.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_task_11.Controllers;


[ApiController]
[Route("api/devices")]
public class DevicesController : ControllerBase
{
    
    private readonly IDeviceService _deviceService;
    private readonly IDeviceTypeService _deviceTypeService;
    public DevicesController(IDeviceService deviceService, IDeviceTypeService deviceTypeService)
    {
        _deviceService = deviceService;
        _deviceTypeService = deviceTypeService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetDevicesAsync()
    {
        var devices = await _deviceService.GetAllDevicesAsync();
        return Ok(devices);
    }
    
    [Authorize(Roles = "Admin,User")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDeviceByIdAsync(int id)
    {
        var device = await _deviceService.GetDeviceByIdAsync(id);
        if (device == null)
            return NotFound($"Device with ID {id} not found");

        return Ok(device);
    }

    //[Authorize(Roles = "Admin,User")]
    [HttpPost]
    public async Task<IActionResult> AddDeviceAsync([FromBody] DeviceCreateOrUpdateDto device)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var createdDevice = await _deviceService.CreateDeviceAsync(device);
            return Created(string.Empty, createdDevice);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [Authorize(Roles = "Admin,User")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDeviceAsync(int id, [FromBody] DeviceCreateOrUpdateDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userRole = User.FindFirstValue(ClaimTypes.Role)!;
        
        
        var device = await _deviceService.GetDeviceByIdAsync(id);
        
        if (device == null)
            return NotFound($"Device with ID {id} not found");

        var employeeId = device.CurrentEmployee?.Id;
        
        if (employeeId == null)
            return BadRequest("No access to this device, no employee assigned");
        if (userRole == "User" && employeeId != userId)
            return Forbid("You are not allowed to update this device");
        
        try
        {
            await _deviceService.UpdateDeviceAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [Authorize(Roles = "Admin,User")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDeviceAsync(int id)
    {
        try
        {
            await _deviceService.DeleteDeviceAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    [HttpGet("types")]
    public async Task<IActionResult> GetDeviceTypes()
    {
        var types = await _deviceTypeService.GetAllDeviceTypesAsync();
        return Ok(types);
    }
}