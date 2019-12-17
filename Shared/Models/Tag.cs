using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models
{
    public class Tag
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Note> Notes { get; set; }

        public Tag()
        {
            Notes = new List<Note>();
        }

        public virtual void AddNote(Note note)
        {
            note.Tag = this;
            Notes.Add(note);
        }
    }

    
}
