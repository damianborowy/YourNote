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
            Tags = new List<Tag>();
            Lectures = new List<Lecture>();
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
        [BsonRepresentation(BsonType.Array)]
        public List<Tag> Tags { get; set; }

        [BsonElement("lecture")]
        [BsonRepresentation(BsonType.Array)]
        public List<Lecture> Lectures { get; set; }

        [BsonElement("date")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }

        public void AddTag(Tag tag)
        {

            Tags.Add(tag);

        }

        public void AddLecture(Lecture tag)
        {

            Lectures.Add(tag);

        }
    }
}
