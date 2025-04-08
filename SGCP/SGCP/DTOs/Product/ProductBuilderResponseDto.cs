using SGCP.DTOs.LaborCost;
using SGCP.DTOs.ProductComponent;

namespace SGCP.DTOs.Product
{
    public class ProductBuilderResponseDto
    {
        public long ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public long CategoryId { get; set; }

        public List<ProductComponentDto> Components { get; set; } = new();
        public List<LaborCostDto> LaborCosts { get; set; } = new();
    }
}
