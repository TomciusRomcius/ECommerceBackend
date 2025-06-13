namespace EventSystemHelper.Utils
{
    public class KafkaConfiguration
    {
        public string Servers { get; }

        public KafkaConfiguration(string servers)
        {
            Servers = servers;
        }
    }
}
