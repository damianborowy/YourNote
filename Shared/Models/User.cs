using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YourNote.Shared.Models.CustomAttribute;

namespace YourNote.Shared.Models
{
    [BsonCollection("Users")]
    public class User
    {

        public User()
        {
            Notes = new List<Note>();
        }

        [BsonId]
        [BsonElement("id")]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [Required]
        [BsonElement("email")]
        public string EmailAddress { get; set; }

        [Required]
        [BsonElement("password")]
        [StringLength(255, MinimumLength = 8)]
        public  string Password { get; set; }

        [BsonElement("role")]
        [BsonRepresentation(BsonType.String)]
        public Permission Role { get; set; } = Permission.Default;

        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("notes")]
        [BsonRepresentation(BsonType.Array)]
        public List<Note> Notes { get; set; }


        public enum Permission
        {
            Default,
            Moderator,
            Admin
        }
    }
}
