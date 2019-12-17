using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using System.Collections.Generic;
using YourNote.Server.Services;
using YourNote.Server.Services.DatabaseService;
using YourNote.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using System;

namespace YourNote.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly IDatabaseService<Note> databaseNote;
        private readonly IDatabaseService<User> databaseUser;
        private readonly IDatabaseService<Tag> databaseTag;
        private readonly IDatabaseService<Lecture> databaseLecture;

        public NotesController(ILogger<NotesController> logger,
           IDatabaseService<Note> dataBaseNote, IDatabaseService<User> databaseUser,
           IDatabaseService<Tag> databaseTag, IDatabaseService<Lecture> databaseLecture)
        {
            this.databaseNote = dataBaseNote;
            this.databaseUser = databaseUser;
            this.databaseLecture = databaseLecture;
            this.databaseTag = databaseTag;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Note> GetAllRecords()
        {
           return databaseNote.Read();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public IEnumerable<Note> GetAllRecordsById(int id)
        {
            return databaseUser.Read(id)?.Notes ?? Array.Empty<Note>();
        }

        // POST: api/Notes
        [HttpPost]
        public IActionResult Post([FromBody] NotePost obj)
        {
            var tag = new List<Tag>(databaseTag.Read()).Find(x => x.Name == obj.Tag);
            var lecture = new List<Lecture>(databaseLecture.Read()).Find(x => x.Name == obj.Lecture);

            if (tag == null)
                tag = new Tag() { Name = obj.Tag };

            if (lecture == null)
                lecture = new Lecture() { Name = obj.Lecture };

            var note = new Note()
            {
                Title = obj.Title,
                Content = obj.Content,
                Color = obj.Color,
                Tag = tag,
                Lecture = lecture
            };

            tag.Notes.Add(note);
            lecture.Notes.Add(note);

            databaseTag.Create(tag);
            databaseLecture.Create(lecture);

            var user = databaseUser.Read(obj.OwnerId);
            user.AddNote(note);

            var result = databaseUser.Update(user);

            if (result != null)
                return Ok(note);
            else
                return BadRequest(new { error = "User doesn't exist" });
        }

        // PUT: api/Notes
        [HttpPut("{userId}")]
        public IActionResult Put(int userId, [FromBody] Note obj)
        {
            var user = databaseUser.Read(userId);
            obj.Owner = user;

            var result = databaseNote.Update(obj);

            if (result != null)
                return Ok(result);
            else
                return BadRequest(new { error = "Note doesn't exist" });
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public bool DeleteById(int id)
        {
            return databaseNote.Delete(id);
        }

        #region Private methods
        #endregion 
    }
}