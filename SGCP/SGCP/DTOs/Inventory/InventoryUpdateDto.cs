namespace SGCP.DTOs.Inventory
{
    public class InventoryUpdateDto
    {
        public decimal Quantity { get; set; }
        public decimal Min { get; set; }
        public bool Enable { get; set; }
    }
}
