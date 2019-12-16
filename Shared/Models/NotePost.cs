using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models
{
    public class NotePost
    {
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual byte Color { get; set; }
        public virtual int OwnerId { get; set; }

        public NotePost() 
        {
            Color = 1;
        }
    }
}
