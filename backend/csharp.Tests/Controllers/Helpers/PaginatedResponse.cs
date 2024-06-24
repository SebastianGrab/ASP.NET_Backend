public class PaginatedResponse<T>
{
    public int totalCount { get; set; }
    public int totalPages { get; set; }
    public int currentPage { get; set; }
    public int pageSize { get; set; }
    public IEnumerable<T> items { get; set; }
}
