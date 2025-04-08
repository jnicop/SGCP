using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.ProductComponent;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class ProductComponentService : IProductComponentService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<ProductComponentService> _logger;

        public ProductComponentService(SGCP_DbContext context, IMapper mapper, ILoggingService<ProductComponentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductComponentDto>> GetAllAsync()
        {
            _logger.Info("Getting all product components.");
            var items = await _context.ProductComponents.Where(pc => pc.Enable == true).ToListAsync();
            return _mapper.Map<IEnumerable<ProductComponentDto>>(items);
        }

        public async Task<ProductComponentDto> GetByIdAsync(long id)
        {
            try
            {
                _logger.Info("Getting product component with ID {Id}.", id);
                var item = await _context.ProductComponents.FirstOrDefaultAsync(pc => pc.Id == id && pc.Enable == true);
                if (item == null)
                {
                    _logger.Warn("ProductComponent with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"ProductComponent with ID {id} not found.");
                }

                return _mapper.Map<ProductComponentDto>(item);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting product component with ID {Id}.", id);
                throw;
            }
        }

        public async Task<ProductComponentDto> CreateAsync(ProductComponentCreateDto dto)
        {
            try
            {
                _logger.Info("Creating new product component.");
                var item = _mapper.Map<ProductComponent>(dto);
                item.InsertDate = DateTime.UtcNow;
                item.Enable = true;

                _context.ProductComponents.Add(item);
                await _context.SaveChangesAsync();

                _logger.Info("ProductComponent created with ID {Id}.", item.Id);
                return _mapper.Map<ProductComponentDto>(item);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating product component.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long id, ProductComponentUpdateDto dto)
        {
            try
            {
                _logger.Info("Updating product component with ID {Id}.", id);
                var item = await _context.ProductComponents.FindAsync(id);
                if (item == null)
                {
                    _logger.Warn("ProductComponent with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"ProductComponent with ID {id} not found.");
                }

                _mapper.Map(dto, item);
                item.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.Info("ProductComponent with ID {Id} updated.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating product component with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                _logger.Info("Deleting product component with ID {Id}.", id);
                var item = await _context.ProductComponents.FindAsync(id);
                if (item == null || item.Enable == false)
                {
                    _logger.Warn("ProductComponent with ID {Id} not found or already deleted.", id);
                    throw new KeyNotFoundException($"ProductComponent with ID {id} not found.");
                }

                item.Enable = false;
                item.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.Info("ProductComponent with ID {Id} deleted logically.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting product component with ID {Id}.", id);
                throw;
            }
        }
    }

}
