using SGCP.DTOs.LaborCost;
using SGCP.DTOs.ProductComponent;

namespace SGCP.DTOs.Product
{
    public class ProductBuilderUpdateDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        public List<ProductComponentCreateDto> Components { get; set; } = new();
        public List<LaborCostCreateDto> LaborCosts { get; set; } = new();
        public decimal? ProfitMargin { get; set; } // margen opcional
    }
}
