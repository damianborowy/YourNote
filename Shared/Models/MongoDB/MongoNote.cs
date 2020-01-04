using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models.MongoDB
{
    public class MongoNote
    {


        public MongoNote()
        {
            Date = DateTime.Now;
        }

        [BsonId]

        public ObjectId Id { get; set; }

        [BsonElement("title")]
        [BsonRepresentation(BsonType.String)]
        public string Title { get; set; }

        [BsonElement("content")]
        [BsonRepresentation(BsonType.String)]
        public string Content { get; set; }

        [BsonElement("content")]
        [BsonRepresentation(BsonType.String)]
        public byte Color { get; set; }

        [BsonElement("tag")]
        public Tag Tag { get; set; }

        [BsonElement("lecture")]
        public Lecture Lecture { get; set; }

        [BsonElement("date")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }
    }
}
