using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourNote.Shared.Models
{
    public class Note : IComparable<Note>
    {
        public virtual int Id { get; set; }

        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }

        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual byte Color { get; set; }


        public virtual IList<UserNote> UserNote { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Lecture Lecture { get; set; }

        public Note()
        {
            UserNote = new List<UserNote>();
            Date = DateTime.Now;
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