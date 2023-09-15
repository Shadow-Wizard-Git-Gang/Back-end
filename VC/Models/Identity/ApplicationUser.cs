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
        public string FullName { get; set; } = null!;
        public string CompanyId { get; set; }
        public bool IsOwner { get; set; }
    }
}
