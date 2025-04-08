namespace SGCP.DTOs.RegionalsPrice
{
    public class RegionalsPriceCreateDto
    {
        public long ProductId { get; set; }
        public long RegionId { get; set; }
        public long CurrencyId { get; set; }
        public decimal Price { get; set; }
    }
}
