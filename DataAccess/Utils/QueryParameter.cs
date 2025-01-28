namespace ECommerce.DataAccess.Utils
{
    public class QueryParameter
    {
        public string? Key { get; set; } = null;
        public object Value { get; set; }

        public QueryParameter(object value)
        {
            Value = value;
        }

        public QueryParameter(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}