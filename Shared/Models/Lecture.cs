﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace YourNote.Shared.Models
{
    public class Lecture
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        [JsonIgnore]
        public virtual IList<Note> Notes { get; set; }

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