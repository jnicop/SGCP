using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SGCP.DTOs.Component;
using SGCP.Models;
using SGCP.Services.Logger;

namespace SGCP.Services
{
    public class ComponentBuilderService : IComponentBuilderService
    {
        private readonly SGCP_DbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService<ComponentBuilderService> _logger;

        public ComponentBuilderService(SGCP_DbContext context, IMapper mapper, ILoggingService<ComponentBuilderService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<long> CreateAsync(ComponentBuilderDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var component = _mapper.Map<Component>(dto);
                component.InsertDate = DateTime.Now;
                component.UserInsert = 1;
                component.Enable = true;
                _context.Components.Add(component);
                await _context.SaveChangesAsync();

                long componentId = component.Id;

                // Presentaciones
                foreach (var presentationDto in dto.Presentations)
                {
                    var presentation = _mapper.Map<ComponentPresentation>(presentationDto);
                    presentation.ComponentId = componentId;
                    presentation.InsertDate = DateTime.Now;
                    _context.ComponentPresentations.Add(presentation);
                }

                // Atributos
                foreach (var attributeDto in dto.Attributes)
                {
                    var attribute = _mapper.Map<ComponentAttribute>(attributeDto);
                    attribute.ComponentId = componentId;
                    attribute.InsertDate = DateTime.Now;
                    _context.ComponentAttributes.Add(attribute);
                }

                // Tratamientos
                foreach (var treatmentDto in dto.Treatments)
                {
                    var treatment = _mapper.Map<ComponentTreatment>(treatmentDto);
                    treatment.ComponentId = componentId;
                    treatment.InsertDate = DateTime.Now;
                    _context.ComponentTreatments.Add(treatment);
                }

                // Procesos
                foreach (var processDto in dto.Processes)
                {
                    var process = _mapper.Map<ComponentProcess>(processDto);
                    process.ComponentId = componentId;
                    _context.ComponentProcesses.Add(process);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return componentId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.Error("Error al crear componente completo", ex);
                throw;
            }
        }

        public async Task UpdateAsync(ComponentBuilderDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var component = await _context.Components
                    .FirstOrDefaultAsync(c => c.Id == dto.Id);

                if (component == null)
                    throw new Exception($"Componente con ID {dto.Id} no encontrado");

                // Actualizar propiedades básicas
                _mapper.Map(dto, component);
                component.UpdateDate = DateTime.Now;
                component.UserUpdate = 1;

                // Eliminar registros existentes (subtablas)
                var componentId = dto.Id;

                _context.ComponentPresentations
                    .RemoveRange(_context.ComponentPresentations.Where(p => p.ComponentId == componentId));

                _context.ComponentAttributes
                    .RemoveRange(_context.ComponentAttributes.Where(a => a.ComponentId == componentId));

                _context.ComponentTreatments
                    .RemoveRange(_context.ComponentTreatments.Where(t => t.ComponentId == componentId));

                _context.ComponentProcesses
                    .RemoveRange(_context.ComponentProcesses.Where(p => p.ComponentId == componentId));

                await _context.SaveChangesAsync();

                // Insertar nuevas presentaciones
                foreach (var presentationDto in dto.Presentations)
                {
                    var presentation = _mapper.Map<ComponentPresentation>(presentationDto);
                    presentation.ComponentId = componentId;
                    presentation.InsertDate = DateTime.Now;
                    _context.ComponentPresentations.Add(presentation);
                }

                // Insertar nuevos atributos
                foreach (var attributeDto in dto.Attributes)
                {
                    var attribute = _mapper.Map<ComponentAttribute>(attributeDto);
                    attribute.ComponentId = componentId;
                    attribute.InsertDate = DateTime.Now;
                    _context.ComponentAttributes.Add(attribute);
                }

                // Insertar nuevos tratamientos
                foreach (var treatmentDto in dto.Treatments)
                {
                    var treatment = _mapper.Map<ComponentTreatment>(treatmentDto);
                    treatment.ComponentId = componentId;
                    treatment.InsertDate = DateTime.Now;
                    _context.ComponentTreatments.Add(treatment);
                }

                // Insertar nuevos procesos
                foreach (var processDto in dto.Processes)
                {
                    var process = _mapper.Map<ComponentProcess>(processDto);
                    process.ComponentId = componentId;
                    _context.ComponentProcesses.Add(process);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.Error("Error al actualizar componente completo", ex);
                throw;
            }
        }

        public async Task<ComponentBuilderDto> GetByIdAsync(long id)
        {
            try
            {
                var component = await _context.Components
                .Include(c => c.ComponentPresentations)
                .Include(c => c.ComponentAttributes)
                .Include(c => c.ComponentTreatments)
                .Include(c => c.ComponentProcesses)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id && c.Enable);

            if (component == null) return null;

            return _mapper.Map<ComponentBuilderDto>(component);
            }
            catch (Exception ex)
            {
                //await transaction.RollbackAsync();
                _logger.Error("Error al actualizar componente completo", ex);
                throw;
            }
        }
    }

}
