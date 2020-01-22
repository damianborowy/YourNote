﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YourNote.Shared.Models.CustomAttribute;

namespace YourNote.Shared.Models
{
    [MyBsonCollection("Users")]
    public class User
    {

        public User()
        {
            
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

        [Required]

        [BsonElement("role")]
        [BsonRepresentation(BsonType.String)]
        public Permission Role { get; set; } = Permission.Default;

        
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("ownedNotes")]
        //[BsonRepresentation(BsonType.Array)]
        public IList<Note> OwnedNotes { get; set; }

        [BsonElement("sharedNotes")]
        //[BsonRepresentation(BsonType.Array)]
        public IList<MongoDBRef> SharedNotesIds { get; set; }


        [BsonElement("allTags")]
        //[BsonRepresentation(BsonType.Array)]
        public IList<Tag> AllTags { get; set; }

        [BsonElement("allLectures")]
        //[BsonRepresentation(BsonType.Array)]
        public IList<Lecture> AllLectures { get; set; }


        

        public enum Permission
        {
            Default,
            Moderator,
            Admin
        }
    }
}
