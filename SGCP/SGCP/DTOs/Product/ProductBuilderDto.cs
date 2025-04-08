using SGCP.DTOs.LaborCost;
using SGCP.DTOs.ProductComponent;

namespace SGCP.DTOs.Product
{
    public class ProductBuilderDto
    {
        public long ProductId { get; set; }  // 0 si es nuevo
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public List<ProductComponentDto> Components { get; set; } = new();
        public List<LaborCostDto> LaborCosts { get; set; } = new();
    }

    //public class ProductComponentInputDto
    //{
    //    public long ComponentId { get; set; }
    //    public decimal Quantity { get; set; }
    //}

    //public class LaborCostInputDto
    //{
    //    public long LaborTypeId { get; set; }
    //    public decimal Hours { get; set; }
    //}
}
