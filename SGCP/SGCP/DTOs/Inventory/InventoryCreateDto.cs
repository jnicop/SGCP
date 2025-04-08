namespace SGCP.DTOs.Inventory
{
    public class InventoryCreateDto
    {
    public long EntityID { get; set; }
        public string EntityTipe { get; set; }  // "Product" o "Component"
        public decimal Quantity { get; set; }
        public long UnitId { get; set; }
        public long LocationId { get; set; }
        public decimal Min { get; set; }
    }
}
