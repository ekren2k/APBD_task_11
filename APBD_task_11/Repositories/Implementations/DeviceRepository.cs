using APBD_task_11.Contexts;
using APBD_task_11.Models;
using APBD_task_11.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APBD_task_11.Repositories.Implementations;

public class DeviceRepository : IDeviceRepository
{
    private readonly MasterContext _context;

    public DeviceRepository(MasterContext context)
    {
        _context = context;
    }
    
    
    
    public async Task<Device?> GetByIdAsync(int id)
    {
        return await _context.Devices
            .Include(d => d.DeviceType)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        return await _context.Devices
            .Include(d => d.DeviceType)
            .ToListAsync();
    }

    public async Task<Device> AddAsync(Device device)
    {
        _context.Devices.Add(device);
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task Update(Device device)
    {
        _context.Entry(device).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var dependentAssignments = _context.DeviceEmployees.Where(de => de.DeviceId == id);
        _context.DeviceEmployees.RemoveRange(dependentAssignments);
        var device = await _context.Devices.FindAsync(id);
        if (device != null)
        {
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
        }
        
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Devices.AnyAsync(d => d.Id == id);
    }
    
    public async Task<DeviceType?> GetDeviceTypeByNameAsync(string name)
    {
        return await _context.DeviceTypes.FirstOrDefaultAsync(dt => dt.Name == name);
    }

    public async Task<DeviceEmployee?> GetCurrentAssignmentAsync(int deviceId)
    {
        return await _context.DeviceEmployees
            .Include(de => de.Employee)
            .ThenInclude(e => e.Person)
            .FirstOrDefaultAsync(de => de.DeviceId == deviceId && de.ReturnDate == null);
    }
}