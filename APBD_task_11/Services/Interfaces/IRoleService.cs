using APBD_task_11.Models;

namespace APBD_task_11.Repositories.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<Role>> GetAllRolesAsync();
}