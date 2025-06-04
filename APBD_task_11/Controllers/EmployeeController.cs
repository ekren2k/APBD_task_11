using APBD_task_11.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APBD_task_11.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeesAsync()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
            return NotFound($"Employee with ID {id} not found");
        
        return Ok(employee);
    }
}