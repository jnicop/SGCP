namespace SGCP.DTOs.Component
{
    public class ComponentTreatmentDto
    {
        public int TreatmentTypeId { get; set; }
        public decimal ExtraCost { get; set; }
        public bool Enable { get; set; } = true;
    }
}
