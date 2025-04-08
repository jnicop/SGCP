namespace SGCP.DTOs.ProductComponent
{
    public class ProductComponentDto
    {

        public long ComponentId { get; set; }
        public decimal Quantity { get; set; }
        public long UnitId { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Enable { get; set; }
    }
}
