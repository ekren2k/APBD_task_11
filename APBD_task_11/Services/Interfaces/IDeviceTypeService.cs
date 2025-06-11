using APBD_task_11.Models;

namespace APBD_task_11.Services.Interfaces;

public interface IDeviceTypeService
{
    Task<List<DeviceType>> GetAllDeviceTypesAsync();
}