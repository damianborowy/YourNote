using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models
{
    public class UserNote
    {
        public virtual User UserId { set; get; }
        
        public virtual Note NoteId { set; get; }

        public virtual bool isOwner { set; get; }

    }
}
