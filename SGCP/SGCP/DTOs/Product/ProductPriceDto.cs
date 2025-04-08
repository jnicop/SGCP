namespace SGCP.DTOs.Product
{
    public class ProductPriceDto
    {
        public long ProductId { get; set; }
        public decimal Cost { get; set; }
        public decimal WholesaleSuggestedPrice { get; set; }
        public decimal WholesaleRealPrice { get; set; }
        public decimal RetailSuggestedPrice { get; set; }
        public decimal RetailRealPrice { get; set; }
    }
}
