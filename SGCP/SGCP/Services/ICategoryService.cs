using SGCP.DTOs.Category;

namespace SGCP.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(long id);
        Task<IEnumerable<CategoryDto>> GetByTypeAsync(int id);
        Task<CategoryDto> CreateAsync(CategoryCreateDto dto);
        Task<bool> UpdateAsync(long id, CategoryUpdateDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
