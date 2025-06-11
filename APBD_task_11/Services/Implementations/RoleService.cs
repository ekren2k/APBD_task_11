using APBD_task_11.Contexts;
using APBD_task_11.Models;
using APBD_task_11.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APBD_task_11.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly MasterContext _context;

    public RoleService(MasterContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }
}