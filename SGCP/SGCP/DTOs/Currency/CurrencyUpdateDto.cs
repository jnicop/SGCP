namespace SGCP.DTOs.Currency
{
    public class CurrencyUpdateDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal TasaCambio { get; set; }
        public bool Enable { get; set; }
    }
}
