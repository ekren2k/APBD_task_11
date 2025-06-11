using APBD_task_11.Models;

namespace APBD_task_11.Services.Interfaces;

public interface IPositionService
{
    Task<IEnumerable<Position>> GetAllPositionsAsync();
}