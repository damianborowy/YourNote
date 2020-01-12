using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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


        [JsonIgnore]
        public virtual IList<UserNote> UserNotes { get; set; }

        public User()
        {
 
            UserNotes = new List<UserNote>();
            Date = DateTime.Now;
        }

        

        

        public enum Permission
        {
            Default,
            Moderator,
            Admin
        }
    }
}