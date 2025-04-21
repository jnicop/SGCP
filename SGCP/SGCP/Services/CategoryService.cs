using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.Category;
using SGCP.Models;

namespace SGCP.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(SGCP_DbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _context.Categories.Where(c => c.Enable == true).ToListAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<IEnumerable<CategoryDto>> GetByTypeAsync(int type)
        {
            var categories = await _context.Categories.Where(c => c.Enable == true && c.CategoryTypeId == type).ToListAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetByIdAsync(long id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.Enable == true);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            category.InsertDate = DateTime.UtcNow;
            category.Enable = true;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> UpdateAsync(long id, CategoryUpdateDto dto)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            _mapper.Map(dto, existingCategory);
            existingCategory.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existingCategory = await _context.Categories.FindAsync(id);
            if (existingCategory == null || existingCategory.Enable == false)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            existingCategory.Enable = false;
            existingCategory.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
