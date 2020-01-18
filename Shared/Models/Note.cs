using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YourNote.Shared.Models
{
    public class Note : IComparable<Note>, IEquatable<Note>
    {
        public virtual int Id { get; set; }

        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }

        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual byte Color { get; set; }


        public virtual IList<UserNote> Users { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Lecture Lecture { get; set; }

        public Note()
        {
            Users = new List<UserNote>();
            Date = DateTime.Now;
        }

        public virtual void AddTag(Tag tag)
        {
            Tag = tag;
            //tag.Notes.Remove(this);
            tag.Notes.Clear();

            tag.Notes.Add(this);
        }

        public virtual void AddLecture(Lecture lecture)
        {
            Lecture = lecture;
            //lecture.Notes.Remove(this);
            lecture.Notes.Clear();

            lecture.Notes.Add(this);
        }

        public virtual int CompareTo(Note other)
        {
            if (other == null)
                return 1;
            else
                return Id.CompareTo(other.Id);
        }

        public bool Equals(Note other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id));
        }
    }
}