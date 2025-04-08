namespace SGCP.DTOs.ProductComponent
{
    public class ProductComponentCreateDto
    {
        public long UnitId { get; set; }
        public long ComponentId { get; set; }
        public decimal Quantity { get; set; }
    }
}
