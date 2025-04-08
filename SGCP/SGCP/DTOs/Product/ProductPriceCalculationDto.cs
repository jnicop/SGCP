namespace SGCP.DTOs.Product
{
    public class ProductPriceCalculationDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        public decimal CostComponents { get; set; }
        public decimal CostLabor { get; set; }
        public decimal TotalCost => CostComponents + CostLabor;

        public decimal ProfitMargin { get; set; } // Ej: 0.3 = 30%
        public decimal SuggestedPrice => TotalCost * (1 + ProfitMargin);
    }
}
