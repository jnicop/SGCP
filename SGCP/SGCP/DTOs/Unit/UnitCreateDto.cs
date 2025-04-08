namespace SGCP.DTOs.Unit
{
    public class UnitCreateDto
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal? ConversionBase { get; set; } = 1.0m;
    }
}
