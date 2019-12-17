using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models
{
    public class NotePost : IComparable<NotePost>
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte Color { get; set; }
        public int OwnerId { get; set; }

        public string Tag { get; set; }
        public string Lecture { get; set; }
        public List<int> SharedTo { get; set; }


        public NotePost() 
        {
            Color = 1;
            SharedTo = new List<int>();
        }


        public NotePost(Note note)
        {

            Id = note.Id;
            Title = note.Title;
            Content = note.Content;
            Color = note.Color;
            OwnerId = note.Owner.Id;
            if(note.Tag != null)
                Tag = note.Tag.Id + "";
            if (note.Lecture != null)
                Lecture = note.Lecture.Id + "";

        }

        public virtual int CompareTo(NotePost other)
        {
            if (other == null)
                return 1;
            else
                return Id.Value.CompareTo(other.Id);
        }
    }
}
