using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Repository.MongoDb.Models
{
    [CollectionName("messages")]
    [BsonIgnoreExtraElements]
    public class InternalMessage
    {
        [BsonRequired]
        [BsonElement("receipient")]
        public string Recipient { get; set; }

        [BsonRequired]
        [BsonElement("message")]
        public string Message { get; set; }

        [BsonRequired]
        [BsonElement("create_date")]
        public DateTime CreateDate { get; set; }

        [BsonRequired]
        [BsonElement("conversationFinished")]
        public bool ConversationFinished { get; set; }
    }
}