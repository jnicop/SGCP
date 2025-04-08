using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.Unit;
using SGCP.Models;

namespace SGCP.Services
{
    public class UnitService : IUnitService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;

        public UnitService(SGCP_DbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UnitDto>> GetAllAsync()
        {
            var units = await _context.Units.Where(u => u.Enable == true).ToListAsync();
            return _mapper.Map<IEnumerable<UnitDto>>(units);
        }

        public async Task<UnitDto> GetByIdAsync(long id)
        {
            var unit = await _context.Units.FirstOrDefaultAsync(u => u.Id == id && u.Enable == true);
            if (unit == null)
                throw new KeyNotFoundException($"Unit with ID {id} not found.");

            return _mapper.Map<UnitDto>(unit);
        }

        public async Task<UnitDto> CreateAsync(UnitCreateDto dto)
        {
            var unit = _mapper.Map<Unit>(dto);
            unit.InsertDate = DateTime.UtcNow;
            unit.Enable = true;

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            return _mapper.Map<UnitDto>(unit);
        }

        public async Task<bool> UpdateAsync(long id, UnitUpdateDto dto)
        {
            var existingUnit = await _context.Units.FindAsync(id);
            if (existingUnit == null)
                throw new KeyNotFoundException($"Unit with ID {id} not found.");

            _mapper.Map(dto, existingUnit);
            existingUnit.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var existingUnit = await _context.Units.FindAsync(id);
            if (existingUnit == null || existingUnit.Enable == false)
                throw new KeyNotFoundException($"Unit with ID {id} not found.");

            existingUnit.Enable = false;
            existingUnit.UpdateDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}