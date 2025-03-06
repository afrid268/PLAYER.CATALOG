namespace Play.Catalog.Service.Settings
{
    public class MongoDBSettings
    {
        public string Host { get; init; }
        public int Port { get; init; }//init doesnt allow modification of values after initialised
        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }

}