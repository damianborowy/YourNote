using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models
{
    public class Lecture
    {
        public virtual string Name{get; set;}
        public virtual  IList<Note> Notes {get; set;}
        public Lecture()
        {
            Notes = new List<Note>();
        }
        public virtual void AddNote(Note note)
        {
            note.Lecture = this;
            Notes.Add(note);
        }
    }
}
