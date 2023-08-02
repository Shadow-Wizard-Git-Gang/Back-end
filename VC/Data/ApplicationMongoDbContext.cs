using AspNetCore.Identity.MongoDbCore.Infrastructure;
using MongoDbGenericRepository;

namespace VC.Data
{
    public class ApplicationMongoDbContext : MongoDbContext
    {
        public ApplicationMongoDbContext(MongoDbConfig mongoDbConfig) : 
            base(mongoDbConfig.ConnectionURI)
        {
        }
    }
}
