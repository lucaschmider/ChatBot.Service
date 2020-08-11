using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Repository.MongoDb.Models
{
    [BsonIgnoreExtraElements]
    [CollectionName("users")]
    public class InternalUser
    {
        [BsonRequired] [BsonElement("uid")] public string Uid { get; set; }

        [BsonRequired]
        [BsonElement("isAdmin")]
        public bool IsAdmin { get; set; }

        [BsonRequired] [BsonElement("name")] public string Name { get; set; }

        [BsonRequired]
        [BsonElement("department")]
        public string Department { get; set; }

        [BsonRequired] [BsonElement("email")] public string Email { get; set; }
    }
}