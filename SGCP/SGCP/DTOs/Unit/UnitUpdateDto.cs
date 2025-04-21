namespace SGCP.DTOs.Unit
{
    public class UnitUpdateDto: UnitCreateDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal? ConversionBase { get; set; }
        public bool Enable { get; set; }
    }
}
