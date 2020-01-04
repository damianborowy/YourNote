using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using YourNote.Shared.Models.CustomAttribute;

namespace YourNote.Shared.Models
{
    [BsonCollection("Tags")]
    public class Tag
    {
        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
       
    }
}