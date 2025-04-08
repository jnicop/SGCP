namespace SGCP.DTOs.InventoryMovement
{
    public class InventoryMovementUpdateDto
    {
        public string MovementType { get; set; }
        public decimal Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public long LocationId { get; set; }
        public bool Enable { get; set; }
    }
}
