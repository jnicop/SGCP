namespace SGCP.DTOs.Component
{
    public class ComponentBuilderDto:ComponentCreateDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? Description { get; set; }

        public int? CategoryId { get; set; }
        public int? ComponentTypeId { get; set; }
        public long UnitId { get; set; }
        public string? Type { get; set; }
        public bool Enable { get; set; } = true;
        public decimal UnitCost { get; set; }
        public List<ComponentPresentationDto> Presentations { get; set; } = new();
        public List<ComponentAttributeDto> Attributes { get; set; } = new();
        public List<ComponentTreatmentDto> Treatments { get; set; } = new();
        public List<ComponentProcessDto> Processes { get; set; } = new();
    }
}
