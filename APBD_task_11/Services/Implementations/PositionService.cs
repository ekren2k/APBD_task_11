using APBD_task_11.Contexts;
using APBD_task_11.Models;
using APBD_task_11.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APBD_task_11.Services.Implementations;

public class PositionService : IPositionService
{
    private readonly MasterContext _context;

    public PositionService(MasterContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Position>> GetAllPositionsAsync()
    {
        return await _context.Positions.ToListAsync();
    }
}