using APBD_task_11.Models;

namespace APBD_task_11.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<IEnumerable<Employee>> GetAllAsync();

}