using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using YourNote.Server.Services.DatabaseService;
using YourNote.Shared.Models;

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
        private readonly IDatabaseService<UserNote> databaseUserNote;
        public NotesController(ILogger<NotesController> logger,
           IDatabaseService<Note> dataBaseNote, IDatabaseService<User> databaseUser,
           IDatabaseService<Tag> databaseTag, IDatabaseService<Lecture> databaseLecture,
           IDatabaseService<UserNote> databaseUserNote)
        {
            this.databaseNote = dataBaseNote;
            this.databaseUser = databaseUser;
            this.databaseLecture = databaseLecture;
            this.databaseTag = databaseTag;
            this.databaseUserNote = databaseUserNote;
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
            var userNoteList = databaseUser.Read(id)?.Notes ?? Array.Empty<UserNote>();
            var noteList = new List<Note>();

            

            foreach (var item in userNoteList)
            {
                noteList.Add(item.Note);
            }
            return ParseToNotePost(noteList);

            
        }

        // POST: api/Notes
        [HttpPost]
        public IActionResult Post([FromBody] NotePost obj)
        {
            var note = SetTagAndLecture(obj);


            var user = databaseUser.Read(obj.OwnerId);
            var userNote = new UserNote
            {
                User = user,
                Note = note,
                IsOwner = true

            };

            note.Users.Add(userNote);
            user.Notes.Add(userNote);

            var result = databaseNote.Create(note);

            

            if (result != null)
                return Ok(new NotePost(note));
            else
                return BadRequest(new { error = "User doesn't exist" });

            
        }

        // PUT: api/Notes
        [HttpPut("{userId}")]
        public IActionResult Put(int userId, [FromBody] NotePost obj)
        {

            var note = SetTagAndLecture(obj);
            

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
            var note = databaseNote.Read(id);

            foreach (var item in note.Users)
            {

                databaseUserNote.Delete(item);
            }
            
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

       

        private Note Parse(NotePost notePost)
        {
            Note parser = new Note()
            {
                Title = notePost.Title,
                Content = notePost.Content,
                Color = notePost.Color,

                
                Date = DateTime.Now
            };
            return parser;
        }

        

        private List<User> ShareNotes(NotePost notePost)
        {
            IList<User> userList = databaseUser.Read();
            List<User> newUserList = new List<User>();
            foreach (User us in userList)
            {
                for (int i = 0; i < notePost.SharedTo.Count; i++)
                {
                    if (us.Id == notePost.SharedTo[i])
                    {
                        newUserList.Add(us);
                    }
                }
            }
            return newUserList;
        }

        private Note SetTagAndLecture(NotePost obj)
        {

            var tag = new List<Tag>(databaseTag.Read()).Find(x => x.Name == obj.Tag);
            var lecture = new List<Lecture>(databaseLecture.Read()).Find(x => x.Name == obj.Lecture);

            var note = Parse(obj);
            if (obj.Id.HasValue)
                note.Id = obj.Id.Value;

            if (tag == null && !String.IsNullOrWhiteSpace(obj.Tag))
            {
                tag = new Tag() { Name = obj.Tag };
                databaseTag.Create(tag);
                note.Tag = tag;
            }

            if (tag != null && !String.IsNullOrWhiteSpace(obj.Tag))
            {

                note.AddTag(tag);

            }

            if (lecture == null && !String.IsNullOrWhiteSpace(obj.Lecture))
            {
                lecture = new Lecture() { Name = obj.Lecture };
                databaseLecture.Create(lecture);
                note.Lecture = lecture;
            }

            if (tag != null && !String.IsNullOrWhiteSpace(obj.Lecture))
            {
                note.AddLecture(lecture);

            }

            return note;
        }
        #endregion Private methods
    }
}