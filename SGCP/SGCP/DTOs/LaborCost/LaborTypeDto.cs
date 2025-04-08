namespace SGCP.DTOs.LaborCost
{
    public class LaborTypeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal HourlyCost { get; set; }
    }

}
