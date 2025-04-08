namespace SGCP.DTOs.Component
{
    public class ComponentCreateDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public long UnitId { get; set; }
        public long? CategoryId { get; set; }
        public decimal UnitCost { get; set; }
        public string Description { get; set; }
    }
}
