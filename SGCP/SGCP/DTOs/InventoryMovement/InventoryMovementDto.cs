namespace SGCP.DTOs.InventoryMovement
{
    public class InventoryMovementDto
    {
        public long Id { get; set; }
        public long InventoryId { get; set; }
        public string MovementType { get; set; }
        public decimal Quantity { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public long LocationId { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Enable { get; set; }
    }
}
