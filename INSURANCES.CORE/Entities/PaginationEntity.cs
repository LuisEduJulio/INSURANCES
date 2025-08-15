namespace INSURANCES.CORE.Entities
{
    public class PaginationEntity(int page, int count)
    {
        public int Page { get; set; } = page;
        public int Count { get; set; } = count;
    }
}