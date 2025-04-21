namespace SGCP.DTOs.Component
{
    public class ComponentProcessDto
    {
        public int ProcessTypeId { get; set; }
        public int ScopeTypeId { get; set; }
        public decimal QuantityApplied { get; set; }
        public decimal CostPerUnit { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string? Notes { get; set; }
    }
}
