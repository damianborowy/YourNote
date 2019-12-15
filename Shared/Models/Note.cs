using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace YourNote.Shared.Models
{
    public class Note
    {

        public virtual int  Id      { get; protected set; } 
        [DataType(DataType.Date)]
        public virtual DateTime Date    { get; set; }
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual byte Color   { get; set; }
        public virtual User Owner { get; set; }
        public virtual IList<User> SharedTo { get; protected set; }

        public Note()
        {
            SharedTo = new List<User>();
        }

        public virtual void AddListener(User user)
        {
            user.SharedNotes.Add(this);
            SharedTo.Add(user);

        }

        public virtual void DeleteListener(User user)
        {
            user.SharedNotes.Remove(this);
            SharedTo.Remove(user);

        }
    }
}
