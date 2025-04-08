namespace SGCP.DTOs.Product
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int ProductTypeId { get; set; }
        public bool Enable { get; set; }
    }
}
