using APBD_task_11.Contexts;
using APBD_task_11.Models;
using APBD_task_11.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APBD_task_11.Repositories.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly MasterContext _context;

    public EmployeeRepository(MasterContext context)
    {
        _context = context;
    }
    
    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
            .Include(p => p.Person)
            .Include(p => p.Position)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await _context.Employees
            .Include(p => p.Person)
            .Include(p => p.Position)
            .ToListAsync();
    }
}