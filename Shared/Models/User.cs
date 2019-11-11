using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace YourNote.Shared.Models
{
    public class User
    {

        public virtual decimal ID { get; set; }
        public virtual String Name { get; set; }
        public virtual String EmailAddress { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }
        public virtual Dictionary<decimal, Note> Notes { get; set; }



    }
}
