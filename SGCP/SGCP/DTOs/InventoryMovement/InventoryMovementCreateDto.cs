namespace SGCP.DTOs.InventoryMovement
{
    public class InventoryMovementCreateDto
    {
        public long InventoryId { get; set; }
        public string MovementType { get; set; } = null!; // "In" o "Out"
        public decimal Quantity { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public long LocationId { get; set; }
    }
}
