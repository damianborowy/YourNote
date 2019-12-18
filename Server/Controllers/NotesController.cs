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
        public IEnumerable<NotePost> GetAllRecords()
        {
           var noteList = databaseNote.Read();          
           return ParseToNotePost(noteList);
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public IEnumerable<NotePost> GetAllRecordsById(int id)
        {
            var noteList = databaseUser.Read(id)?.Notes ?? Array.Empty<Note>();
            var sharedNoteList = databaseUser.Read(id)?.SharedNotes ?? Array.Empty<Note>();

            foreach (var item in sharedNoteList)
            {
                noteList.Add(item);
            }

            return ParseToNotePost(noteList);
        }

        // POST: api/Notes
        [HttpPost]
        public IActionResult Post([FromBody] NotePost obj)
        {

            var tag = new List<Tag>(databaseTag.Read()).Find(x => x.Name == obj.Tag);
            var lecture = new List<Lecture>(databaseLecture.Read()).Find(x => x.Name == obj.Lecture);

            var note = Parse(obj);


            if (tag == null && obj.Tag != null)
            {
                tag = new Tag() { Name = obj.Tag }; 
                databaseTag.Create(tag);
                tag.AddNote(note);

            }


            if (lecture == null && obj.Lecture != null)
            {
                lecture = new Lecture() { Name = obj.Lecture };
                databaseLecture.Create(lecture);
                lecture.AddNote(note);

            }

            var user = databaseUser.Read(obj.OwnerId);
            user.AddNote(note);

            var result = databaseUser.Update(user);

            if (result != null)
                return Ok(new NotePost(note));
            else
                return BadRequest(new { error = "User doesn't exist" });


            Note Parse(NotePost notePost)
            {
                Note parser = new Note()
                {

                    Title = notePost.Title,
                    Content = notePost.Content,
                    Color = notePost.Color,

                    Owner = databaseUser.Read(notePost.OwnerId),
                    Date = DateTime.Now


                };
                return parser;
            }
        }

        // PUT: api/Notes
        [HttpPut("{userId}")]
        public IActionResult Put(int userId, [FromBody] NotePost obj)
        {

            var tag = new List<Tag>(databaseTag.Read()).Find(x => x.Name == obj.Tag);
            var lecture = new List<Lecture>(databaseLecture.Read()).Find(x => x.Name == obj.Lecture);

            var note = Parse(obj);
            if(obj.Id.HasValue)
                note.Id = obj.Id.Value;

            if (tag == null && obj.Tag != null)
            {
                tag = new Tag() { Name = obj.Tag };
                databaseTag.Create(tag);
                tag.AddNote(note);

            }


            if (lecture == null && obj.Lecture != null)
            {
                lecture = new Lecture() { Name = obj.Lecture };
                databaseLecture.Create(lecture);
                lecture.AddNote(note);

            }

            if (obj.SharedTo is null)
                obj.SharedTo = new List<int>();
            else 
            {
                List<User> tempUserList = ShareNotes(obj);
                foreach (User us in tempUserList)
                {
                    note.SharedTo.Add(us);
                }
            }
            var user = databaseUser.Read(obj.OwnerId);
            user.AddNote(note);

            var result = databaseNote.Update(note);

            if (result != null)
                return Ok(new NotePost(note));
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


        private Note ParseToNewNote(NotePost notePost)
        {

            Note note = new Note()
            {

                Title = notePost.Title,
                Content = notePost.Content,
                Color = notePost.Color,
                
                Owner = databaseUser.Read(notePost.OwnerId),
                Date = DateTime.Now
                
                 
            };

            if (notePost.Tag != null)
                note.Tag = databaseTag.Read(Int32.Parse(notePost.Tag));

            if (notePost.Lecture != null)
                note.Lecture = databaseLecture.Read(Int32.Parse(notePost.Lecture));


            if (notePost.Id.HasValue)
                note.Id = notePost.Id.Value;

            return note;
        }

        private static List<NotePost> ParseToNotePost(IList<Note> noteList)
        {

            var notePostList = new List<NotePost>();

            foreach (var note in noteList)
            {

                notePostList.Add(new NotePost(note));

            }
            notePostList.Sort();

            return notePostList;
        }
        #endregion


        public Note Parse(NotePost notePost)
        {
            Note parser = new Note()
            {

                Title = notePost.Title,
                Content = notePost.Content,
                Color = notePost.Color,

                Owner = databaseUser.Read(notePost.OwnerId),
                Date = DateTime.Now


            };
            return parser;
        }
        #region privateMethods
        private List<User> ShareNotes(NotePost notePost)
        {
            IList <User> userList = databaseUser.Read();
            List<User> newUserList = new List<User>();
            foreach(User us in userList)
            {
                for(int i=0; i<notePost.SharedTo.Count; i++)
                {
                    if(us.Id==notePost.SharedTo[i])
                    {
                        newUserList.Add(us);
                    }
                }
            }
            return newUserList;
        }
        #endregion
    }

}