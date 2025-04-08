namespace SGCP.DTOs.Currency
{
    public class CurrencyCreateDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal TasaCambio { get; set; }
    }
}
