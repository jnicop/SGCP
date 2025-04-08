namespace SGCP.DTOs.Currency
{
    public class CurrencyDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal TasaCambio { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Enable { get; set; }
    }
}
