using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using YourNote.Shared.Models.CustomAttribute;

namespace YourNote.Shared.Models
{
    
    public class Note : IComparable<Note>
    {


        public Note()
        {
            Date = DateTime.Now;
            Tags = new List<string>();
            Lectures = new List<string>();
        }

        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

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
        //[BsonRepresentation(BsonType.Array)]
        public IList<string> Tags { get; set; }

        [BsonElement("lecture")]
        //[BsonRepresentation(BsonType.Array)]
        public IList<string> Lectures { get; set; }

        [BsonElement("date")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }

        public void AddTag(string tag)
        {

            Tags.Add(tag);

        }

        public void AddLecture(string tag)
        {

            Lectures.Add(tag);

        }

        public virtual int CompareTo(Note other)
        {
            if (other == null)
                return 1;
            else
                return Id.CompareTo(other.Id);
        }


    }
}
