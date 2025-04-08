namespace SGCP.DTOs.Component
{
    public class ComponentDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long UnitId { get; set; }
        public long? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal UnitCost { get; set; }
        public string Description { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Enable { get; set; }
    }
}
