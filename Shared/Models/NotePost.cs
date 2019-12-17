using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models
{
    public class NotePost
    {
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

            Title = note.Title;
            Content = note.Content;
            Color = note.Color;
            OwnerId = note.Owner.Id;
            Tag = note.Tag.Id + "";
            Lecture = note.Lecture.Id + "";


        }
    }
}
