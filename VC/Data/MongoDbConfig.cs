namespace VC.Data
{
    public class MongoDbConfig
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        public string ConnectionURI => string.Format(Host, UserName, Password, DatabaseName);
    }
}
