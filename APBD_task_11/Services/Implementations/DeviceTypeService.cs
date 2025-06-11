using APBD_task_11.Contexts;
using APBD_task_11.Models;
using APBD_task_11.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APBD_task_11.Services.Implementations;

public class DeviceTypeService : IDeviceTypeService
{
    private readonly MasterContext _context;

    public DeviceTypeService(MasterContext context)
    {
        _context = context;
    }

    public async Task<List<DeviceType>> GetAllDeviceTypesAsync()
    {
        return await _context.DeviceTypes.ToListAsync();
    }
}