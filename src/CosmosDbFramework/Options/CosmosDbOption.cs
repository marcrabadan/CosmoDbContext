using Azure.Cosmos;

namespace CosmosDbFramework.Options
{
    public class CosmosDbOption
    {
        public string Endpoint { get; set; }
        public string AuthKeyOrResourceToken { get; set; }
        public CosmosClientOptions Settings { get; set; }
    }
}
