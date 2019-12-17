using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace YourNote.Shared.Models
{
    public class Note
    {

        public virtual int Id { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual byte Color { get; set; }
        [JsonIgnore]
        public virtual User Owner { get; set; }
        public virtual IList<User> SharedTo { get; set; }

        public virtual Tag Tag {get; set;}
        public virtual Lecture Lecture {get; set;}

        public Note()
        {
            SharedTo = new List<User>();
            Date = DateTime.Now;
            Tag = new Tag();
            Lecture = new Lecture();
        }

        public virtual void AddListener(User user)
        {
            user.SharedNotes.Add(this);
            SharedTo.Add(user);

        }

        public virtual void DeleteListener(User user)
        {
            user.SharedNotes.Remove(this);
            SharedTo.Remove(user);
        }
    }
}
