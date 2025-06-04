using APBD_task_11.DTOs.Device;

namespace APBD_task_11.Services.Interfaces;

public interface IDeviceService
{
    Task<IEnumerable<DeviceListDto>> GetAllDevicesAsync();
    Task<DeviceDetailDto?> GetDeviceByIdAsync(int id);
    Task<DeviceCreateOrUpdateDto> CreateDeviceAsync(DeviceCreateOrUpdateDto dto);
    Task UpdateDeviceAsync(int id, DeviceCreateOrUpdateDto dto);
    Task DeleteDeviceAsync(int id);
}