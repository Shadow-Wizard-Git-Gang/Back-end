using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using VC.Data;

namespace VC.Models
{
    [CollectionName(SettingsStorage.CollectionNameForCompany)]
    public class Company
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Employees { get; set; }
    }
}
