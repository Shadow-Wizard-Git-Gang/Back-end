using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository;
using MongoDbGenericRepository.Attributes;
using VC.Data;

namespace VC.Models.Identity
{
    [CollectionName(SettingsStorage.CollectionNameForApplicationUser)]
    public class ApplicationUser : MongoIdentityUser<ObjectId>
    {
    }
}
