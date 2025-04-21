using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs;
using SGCP.DTOs.Component;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class ComponentService : IComponentService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<ComponentService> _logger;

        public ComponentService(SGCP_DbContext context, IMapper mapper, ILoggingService<ComponentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<ComponentDto>> GetAllAsync()
        {
            _logger.Info("Getting all components.");
            var components = await _context.Components.Include(p => p.Category).Where(c => c.Enable == true).ToListAsync();
            return _mapper.Map<IEnumerable<ComponentDto>>(components);
        }
        public async Task<PagedResult<ComponentDto>> GetPagedAsync(PaginationQueryDto query, long? categoryId = null, int? componentTypeId = null)
        {
            var q = _context.Components
                .Where(c => (query.Enable == null || c.Enable == query.Enable)
                            && (string.IsNullOrEmpty(query.Search) || c.Name.Contains(query.Search))
                            && (!categoryId.HasValue || c.CategoryId == categoryId.Value)
                            && (!componentTypeId.HasValue || c.ComponentTypeId == componentTypeId.Value));

            var total = await q.CountAsync();

            var items = await q
                .OrderBy(c => c.Name)
                .Skip(query.PageIndex * query.PageSize)
                .Take(query.PageSize)
                .Select(c => new ComponentDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Code = c.Code,
                    UnitCost = c.UnitCost,
                    Enable = c.Enable,
                    CategoryId = c.CategoryId,
                    CategoryName = c.Category != null ? c.Category.Name : null,
                    ComponentTypeId = c.ComponentTypeId,
                    ComponentTypeName = c.ComponentType != null ? c.ComponentType.Name : null,
                    UnitId = c.UnitId,
                    UnitName = c.Unit != null ? c.Unit.Name : null,
                    UnitSymbol = c.Unit != null ? c.Unit.Symbol : null
                })
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<ComponentDto>>(items);

            return new PagedResult<ComponentDto>
            {
                TotalItems = total,
                Items = mapped
            };
        }


        public async Task<ComponentDto> GetByIdAsync(long id)
        {
            try
            {
                _logger.Info("Getting component with ID {Id}.", id);
                var component = await _context.Components.FirstOrDefaultAsync(c => c.Id == id && c.Enable == true);
                if (component == null)
                {
                    _logger.Warn("Component with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Component with ID {id} not found.");
                }

                return _mapper.Map<ComponentDto>(component);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting component with ID {Id}.", id);
                throw;
            }
        }

        public async Task<ComponentDto> CreateAsync(ComponentCreateDto dto)
        {
            try
            {
                _logger.Info("Creating new component.");
                var component = _mapper.Map<Component>(dto);
                component.InsertDate = DateTime.UtcNow;
                component.Enable = true;

                _context.Components.Add(component);
                await _context.SaveChangesAsync();

                _logger.Info("Component created with ID {Id}.", component.Id);
                return _mapper.Map<ComponentDto>(component);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating new component.");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(long id, ComponentUpdateDto dto)
        {
            try
            {
                _logger.Info("Updating component with ID {Id}.", id);
                var existingComponent = await _context.Components.FindAsync(id);
                if (existingComponent == null)
                {
                    _logger.Warn("Component with ID {Id} not found.", id);
                    throw new KeyNotFoundException($"Component with ID {id} not found.");
                }

                _mapper.Map(dto, existingComponent);
                existingComponent.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.Info("Component with ID {Id} updated successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating component with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                _logger.Info("Deleting component with ID {Id}.", id);
                var existingComponent = await _context.Components.FindAsync(id);
                if (existingComponent == null || existingComponent.Enable == false)
                {
                    _logger.Warn("Component with ID {Id} not found or already deleted.", id);
                    throw new KeyNotFoundException($"Component with ID {id} not found.");
                }

                existingComponent.Enable = false;
                existingComponent.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.Info("Component with ID {Id} deleted (logical) successfully.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting component with ID {Id}.", id);
                throw;
            }
        }
    }
}
