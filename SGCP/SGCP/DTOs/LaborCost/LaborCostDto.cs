namespace SGCP.DTOs.LaborCost
{
    public class LaborCostDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }

        public long LaborTypeId { get; set; }       

        public decimal Quantity { get; set; }
        public long UnitId { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Enable { get; set; }
    }

}
