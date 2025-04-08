namespace SGCP.DTOs.RegionalsPrice
{
    public class RegionalsPriceDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long RegionId { get; set; }
        public long CurrencyId { get; set; }
        public decimal Price { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Enable { get; set; }
    }
}
