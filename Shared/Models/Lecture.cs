using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using YourNote.Shared.Models.CustomAttribute;

namespace YourNote.Shared.Models
{
    [BsonCollection("Lectures")]
    public class Lecture : IComparable<Lecture>
    {
        

            [BsonElement("name")]
            public string Name { get; set; }


            public int CompareTo(Lecture other)
            {
                if (other == null)
                    return 1;
                else
                    return Name.CompareTo(other.Name);
            }


        
    }
}