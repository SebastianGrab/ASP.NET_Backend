using Newtonsoft.Json;

namespace Helper
{
    public class QueryObjectStatistics
    {
        public DateOnly? minDate { get; set; } = null;
        public DateOnly? maxDate { get; set; } = null;
    }
}