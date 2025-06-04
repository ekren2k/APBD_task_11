using APBD_task_11.DTOs.Employee;

namespace APBD_task_11.Services.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeListDto>> GetAllEmployeesAsync();
    Task<EmployeeDetailDto?> GetEmployeeByIdAsync(int id);
}