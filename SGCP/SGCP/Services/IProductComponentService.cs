using SGCP.DTOs.ProductComponent;

namespace SGCP.Services
{
    public interface IProductComponentService
    {
        Task<IEnumerable<ProductComponentDto>> GetAllAsync();
        Task<ProductComponentDto> GetByIdAsync(long id);
        Task<ProductComponentDto> CreateAsync(ProductComponentCreateDto dto);
        Task<bool> UpdateAsync(long id, ProductComponentUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
