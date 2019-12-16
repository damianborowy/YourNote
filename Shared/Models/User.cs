using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace YourNote.Shared.Models
{
    public class User
    {


        public virtual int Id { get; set; }
        [Required]
        public virtual string Username { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public virtual string Password { get; set; }
        public virtual Permission Role { get; set; } = Permission.Default;
        public virtual string EmailAddress { get; set; }
        public virtual string Token { get; set; }




        [DataType(DataType.Date)]
        public virtual DateTime Date { get; set; }
        public virtual string Name { get; set; }


        public virtual IList<Note> Notes  { get; set; }
        public virtual IList<Note> SharedNotes { get; set; }

        public User()
        {
            Notes = new List<Note>();
            SharedNotes = new List<Note>();
            Date = DateTime.Now;
        }

        public virtual void AddNote(Note note)
        {

            note.Owner = this;
            Notes.Add(note);

        }

        public virtual void DeleteNote(Note note)
        {

            Notes.Remove(note);

        }

        public enum Permission
        {
            Default,
            Moderator,
            Admin
        }
        
    }
}
