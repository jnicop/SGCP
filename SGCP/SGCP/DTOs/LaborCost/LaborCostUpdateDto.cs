namespace SGCP.DTOs.LaborCost
{
    public class LaborCostUpdateDto
    {
        public string Type { get; set; }
        public long UnitId { get; set; }

        public decimal Quantity { get; set; }
        public bool Enable { get; set; }
    }
}
