using System;

namespace ChatBot.Repository.MongoDb
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionNameAttribute : Attribute
    {
        public CollectionNameAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }

        public string CollectionName { get; }
    }
}