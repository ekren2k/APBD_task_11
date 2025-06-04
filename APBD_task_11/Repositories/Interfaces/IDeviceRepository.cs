using APBD_task_11.Models;

namespace APBD_task_11.Repositories.Interfaces;

public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(int id);
    Task<IEnumerable<Device>> GetAllAsync();
    Task<Device> AddAsync(Device device);
    Task Update(Device device);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<DeviceType?> GetDeviceTypeByNameAsync(string name);
    Task<DeviceEmployee?> GetCurrentAssignmentAsync(int deviceId);
}