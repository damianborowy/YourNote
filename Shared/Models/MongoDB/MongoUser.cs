using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace YourNote.Shared.Models.MongoDB
{
    internal class MongoUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [BsonElement("Name")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8)]
        public  string Password { get; set; }

        public  Permission Role { get; set; } = Permission.Default;

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }



        public enum Permission
        {
            Default,
            Moderator,
            Admin
        }
    }
}
