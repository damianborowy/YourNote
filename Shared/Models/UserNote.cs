using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models
{
    public class UserNote
    {
        public virtual User UserId { set; get; }
        
        public virtual Note NoteId { set; get; }

        public virtual bool IsOwner { set; get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var x = obj as UserNote;

            return ((x.UserId.Id == this.UserId.Id)
                 && (x.NoteId.Id == this.NoteId.Id));
        }

        public override int GetHashCode()
        {
            return 9999;
        }
    }
}
