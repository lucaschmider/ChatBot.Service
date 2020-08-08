using ChatBot.Repository.Contracts.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Repository.MongoDb.Models
{
    /// <inheritdoc cref="IUserDetails" />
    [BsonIgnoreExtraElements]
    public class UserDetails :IUserDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DataSetId { get; set; }

        [BsonElement("uid")] public string Uid { get; set; }

        [BsonElement("isAdmin")] public bool IsAdmin { get; set; }

        [BsonElement("name")] public string Name { get; set; }

        [BsonElement("department")] public string Department { get; set; }

        [BsonElement("email")] public string Email { get; set; }
        public string CollectionName => "users";
    }
}