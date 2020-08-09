using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Repository.MongoDb.Models
{
    [BsonIgnoreExtraElements]
    [CollectionName("users")]
    public class InternalUser
    {
        [BsonElement("uid")] public string Uid { get; set; }

        [BsonElement("isAdmin")] public bool IsAdmin { get; set; }

        [BsonElement("name")] public string Name { get; set; }

        [BsonElement("department")] public string Department { get; set; }

        [BsonElement("email")] public string Email { get; set; }
    }
}