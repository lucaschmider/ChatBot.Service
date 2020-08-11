using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatBot.Repository.MongoDb.Models
{
    [CollectionName("departments")]
    public class InternalDepartment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DepartmentId { get; set; }

        [BsonElement("departmentName")] public string DepartmentName { get; set; }
    }
}