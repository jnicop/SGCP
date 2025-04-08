namespace SGCP.DTOs.Inventory
{
    public class InventoryDto
    {
        public long Id { get; set; }
        public long EntityID { get; set; }
        public string EntityTipe { get; set; }
        public decimal Quantity { get; set; }
        public long UnitId { get; set; }
        public long LocationId { get; set; }
        public decimal Min { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Enable { get; set; }
    }
}
