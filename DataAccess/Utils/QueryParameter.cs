namespace ECommerce.DataAccess.Utils
{
    public class QueryParameter
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public QueryParameter(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}