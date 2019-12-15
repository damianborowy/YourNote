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
        public virtual User Owner   { get; set; }  
        [DataType(DataType.Date)]
        public virtual DateTime Date    { get; set; }
        public virtual string Title { get; set; }
        public virtual string Content { get; set; }
        public virtual byte Color   { get; set; }





        

        /*public Note(string owner, string contnent, decimal color)
        {
            
            this.Owner = owner;
            this.Content = contnent;
            this.Color = color;
            this.Date = DateTime.Now;
            

        }*/


    }
}
