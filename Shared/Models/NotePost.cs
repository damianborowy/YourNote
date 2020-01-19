using System;
using System.Collections.Generic;

namespace YourNote.Shared.Models
{
    public class NotePost : IComparable<NotePost>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public byte Color { get; set; }
        

        public List<string> Tags { get; set; }
        public List<string> Lectures { get; set; }
        

        public NotePost()
        {
            Color = 1;
            Tags = new List<string>();
            Lectures = new List<string>();
            
        }

        public NotePost(Note note)
        {
            Id = note.Id;
            Title = note.Title;
            Content = note.Content;
            Color = note.Color;

            foreach (var item in note.Tags)
            {
                this.Tags.Add(item);
            }

            foreach (var item in note.Lectures)
            {
                this.Lectures.Add(item);
            }        
           
        }

        public virtual int CompareTo(NotePost other)
        {
            if (other == null)
                return 1;
            if (String.IsNullOrEmpty(other.Id))
                return 1;
            else
                return Id.CompareTo(other.Id);
        }
    }
}