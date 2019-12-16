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

        public NotePost() 
        {
            Color = 1;
        }
    }
}
