namespace VC.Data
{
    public class MongoDbConfig
    {
        public string Host { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;

        public string ConnectionURI => string.Format(Host, UserName, Password, DatabaseName);
    }
}
