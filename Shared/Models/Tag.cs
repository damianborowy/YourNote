using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace YourNote.Shared.Models
{
    public class Tag 
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        [JsonIgnore]
        public virtual IList<Note> Notes { get; set; }

        public Tag()
        {
            Notes = new List<Note>();
        }

       

        

        
    }
}