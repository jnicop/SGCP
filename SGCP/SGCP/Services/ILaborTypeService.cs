using SGCP.DTOs.LaborCost;

namespace SGCP.Services
{
    public interface ILaborTypeService
    {
        Task<List<LaborTypeDto>> GetAllAsync();
    }
}
