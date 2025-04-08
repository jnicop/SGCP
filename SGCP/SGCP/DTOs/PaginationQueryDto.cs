namespace SGCP.DTOs
{
    public class PaginationQueryDto
    {
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }

        public bool? Enable { get; set; }
    }
}
