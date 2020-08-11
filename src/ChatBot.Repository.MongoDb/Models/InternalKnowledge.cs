using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Repository.MongoDb.Models
{
    [BsonIgnoreExtraElements]
    [CollectionName("knowledge")]
    public class InternalKnowledge
    {
        [BsonRequired]
        [BsonElement("definitionType")]
        public string DefinitionType { get; set; }

        [BsonRequired]
        [BsonElement("keyword")]
        public string Keyword { get; set; }

        [BsonRequired]
        [BsonElement("description")]
        public string Description { get; set; }
    }
}