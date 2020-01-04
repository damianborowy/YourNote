using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using YourNote.Shared.Models.CustomAttribute;

namespace YourNote.Shared.Models
{
    [BsonCollection("Notes")]
    public class Note 
    {


        public Note()
        {
            Date = DateTime.Now;
        }

        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.String)]
        public String Id { get; set; }

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
        public MongoDBRef Tag { get; set; }

        [BsonElement("lecture")]
        public MongoDBRef Lecture { get; set; }

        [BsonElement("date")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }

        public void SetTag(string Id)
        {

            Tag = new MongoDBRef("Tags", Id); 

        }

        public void SetLecture(string Id)
        {

            Tag = new MongoDBRef("Lectures", Id);

        }
    }
}
