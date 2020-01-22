using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using YourNote.Shared.Models.CustomAttribute;

namespace YourNote.Shared.Models
{
    [MyBsonCollection("Tags")]
    public class Tag : IComparable<Tag>
    {
        
        public Tag(string Name)
        {
            this.Name = Name;
        }

        [BsonElement("name")]
        public string Name { get; set; }

        public virtual int CompareTo(Tag other)
        {

            if (other is null)
                return 1;
            else
                return Name.CompareTo(other.Name);
        }

    }
}