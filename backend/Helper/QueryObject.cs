namespace Helper
{
    public class QueryObject
    {
        public DateTime? minCreatedDateTime { get; set; } = null;
        public DateTime? maxCreatedDateTime { get; set; } = null;

        public DateTime? minUpdatedDateTime { get; set; } = null;
        public DateTime? maxUpdatedDateTime { get; set; } = null;
    }
}