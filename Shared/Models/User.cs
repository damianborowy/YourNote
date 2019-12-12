using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YourNote.Shared.Models
{
    public class User
    {
        [Required]
        public virtual string Username { get; set; }
        [Required]
        public virtual string Password { get; set; }


        public virtual int ID { get; set; }
        public virtual String Name { get; set; }
        public virtual String EmailAddress { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }
        public virtual Dictionary<decimal, Note> Notes { get; set; }
        public virtual string Token { get; set; }
    }
}
