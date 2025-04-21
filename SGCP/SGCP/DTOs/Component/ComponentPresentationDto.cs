namespace SGCP.DTOs.Component
{
    public class ComponentPresentationDto
    {
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Measure { get; set; }
        public decimal? QuantityPerBase { get; set; }

        public decimal Price { get; set; }
        public long UnitId { get; set; }

        public decimal? BaseUnitCost { get; set; }
        public decimal? WeightGrams { get; set; }
        public decimal? LengthMeters { get; set; }

        public bool Enable { get; set; } = true;
    }

}
