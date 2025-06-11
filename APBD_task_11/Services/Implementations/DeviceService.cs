using System.Text.Json;
using APBD_task_11.DTOs.Device;
using APBD_task_11.Models;
using APBD_task_11.Repositories.Interfaces;
using APBD_task_11.Services.Interfaces;

namespace APBD_task_11.Services.Implementations;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<IEnumerable<DeviceListDto>> GetAllDevicesAsync()
    {
        var devices = await _deviceRepository.GetAllAsync();
        return devices.Select(d => new DeviceListDto
        {
            Id = d.Id,
            Name = d.Name
        });
    }

    public async Task<DeviceDetailDto?> GetDeviceByIdAsync(int id)
    {
        var device = await _deviceRepository.GetByIdAsync(id);
        if (device == null) return null;

        var employee = await _deviceRepository.GetCurrentAssignmentAsync(id);
        var additionalProperties = JsonSerializer.Deserialize<Dictionary<string, object>>(device.AdditionalProperties);

        return new DeviceDetailDto
        {
            Name = device.Name,
            DeviceTypeName = device.DeviceType?.Name,
            IsEnabled = device.IsEnabled,
            AdditionalProperties = additionalProperties,
            CurrentEmployee = employee == null ? null : new DeviceDetailDto.CurrentEmployeeDto
            {
                Id = employee.Employee.Id,
                FullName = $"{employee.Employee.Person.FirstName} {employee.Employee.Person.MiddleName} {employee.Employee.Person.LastName}"
            }
        };
    }

    public async Task<DeviceCreateOrUpdateDto> CreateDeviceAsync(DeviceCreateOrUpdateDto dto)
    {
        var deviceType = await _deviceRepository.GetDeviceTypeByIdAsync(dto.TypeId);
        if (deviceType == null)
            throw new ArgumentException($"Invalid device type: {dto.TypeId}");

        var device = new Device
        {
            Name = dto.Name,
            IsEnabled = dto.IsEnabled,
            AdditionalProperties = JsonSerializer.Serialize(dto.AdditionalProperties),
            DeviceTypeId = deviceType.Id
        };

        var createdDevice = await _deviceRepository.AddAsync(device);

        return new DeviceCreateOrUpdateDto
        {
            Name = createdDevice.Name,
            TypeId = createdDevice.DeviceTypeId,
            IsEnabled = createdDevice.IsEnabled,
            AdditionalProperties = JsonSerializer.Deserialize<JsonElement>(createdDevice.AdditionalProperties)
        };
    }

    public async Task UpdateDeviceAsync(int id, DeviceCreateOrUpdateDto dto)
    {
        var existingDevice = await _deviceRepository.GetByIdAsync(id);
        if (existingDevice == null)
            throw new KeyNotFoundException($"Device with id {id} not found");

        var deviceType = await _deviceRepository.GetDeviceTypeByIdAsync(dto.TypeId);
        if (deviceType == null)
            throw new ArgumentException($"Invalid device type: {dto.TypeId}");

        existingDevice.Name = dto.Name;
        existingDevice.IsEnabled = dto.IsEnabled;
        existingDevice.AdditionalProperties = JsonSerializer.Serialize(dto.AdditionalProperties);
        existingDevice.DeviceTypeId = deviceType.Id;

        await _deviceRepository.Update(existingDevice);
    }

    public async Task DeleteDeviceAsync(int id)
    {
        var exists = await _deviceRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"Device with id {id} not found");

        await _deviceRepository.DeleteAsync(id);
    }
}