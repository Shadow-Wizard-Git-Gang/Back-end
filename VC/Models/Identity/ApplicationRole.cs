using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;
using VC.Data;

namespace VC.Models.Identity
{
    [CollectionName(SettingsStorage.CollectionNameForApplicationRole)]
    public class ApplicationRole : MongoIdentityRole<ObjectId>
    {
    }
}
