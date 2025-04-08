namespace SGCP.DTOs.LaborCost
{
    public class LaborCostCreateDto
    {
        public long UnitId { get; set; }
        public long LaborTypeId { get; set; }
        public decimal Quantity { get; set; }
    }
}
