namespace SGCP.DTOs.Component
{
    public class ComponentDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public string? Code { get; set; }
        public decimal UnitCost { get; set; }
        public bool Enable { get; set; }

        public long? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public int? ComponentTypeId { get; set; }
        public string? ComponentTypeName { get; set; }

        public long UnitId { get; set; }
        public string? UnitName { get; set; }
        public string? UnitSymbol { get; set; }
    }


}
