namespace SGCP.DTOs.Unit
{
    public class UnitDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal? ConversionBase { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Enable { get; set; }
    }
}
