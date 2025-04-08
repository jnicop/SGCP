namespace SGCP.DTOs.Region
{
    public class RegionDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Enable { get; set; }
    }
}
