using System;
using System.Collections.Generic;
using System.Text;

namespace YourNote.Shared.Models
{
    public class UserNote
    {
        public virtual User User { set; get; }
        
        public virtual Note Note { set; get; }

        public virtual bool IsOwner { set; get; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var x = obj as UserNote;

            return ((x.User.Id == this.User.Id)
                 && (x.Note.Id == this.Note.Id));
        }

        public override int GetHashCode()
        {
            return 9999;
        }
    }
}
