using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using YourNote.Shared.Models.CustomAttribute;

namespace YourNote.Shared.Models
{
    [MyBsonCollection("SharedNotes")]
    public class Note : IComparable<Note>
    {


        public Note()
        {
            Date = DateTime.Now;
            
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

        [BsonElement("color")]
        [BsonRepresentation(BsonType.String)]
        public byte Color { get; set; }

        [BsonElement("tag")]
        public IList<Tag> Tags { get; set; }

        [BsonElement("lecture")]
        public IList<Lecture> Lectures { get; set; }

        [BsonElement("date")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }

        [BsonElement("sharesTo")]
        public List<string> SharesTo { get; set; }

        [BsonElement("ownerId")]
        public string OwnerId { get; set; }

        public void AddTag(string tag)
        {

            Tags.Add(new Tag(tag));

        }

        public void AddLecture(string tag)
        {

            Lectures.Add(new Lecture(tag));

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
