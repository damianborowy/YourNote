using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using YourNote.Server.Services.DatabaseService;
using YourNote.Shared.Models;

namespace YourNote.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharesController : ControllerBase
    {

        private readonly IDatabaseService<Note> databaseNote;
        private readonly IDatabaseService<User> databaseUser;

        private readonly IDatabaseService<Lecture> databaseLecture;
        public IMongoDatabase Database { get; }

        public SharesController(ILogger<SharesController> logger,
           IDatabaseService<Note> dataBaseNote, IDatabaseService<User> databaseUser,
           IDatabaseService<Lecture> databaseLecture,
           MongoClient client)
        {
            this.databaseNote = dataBaseNote;
            this.databaseUser = databaseUser;
            this.databaseLecture = databaseLecture;
            Database = client.GetDatabase("YourNote");


        }




        // GET: api/Shares/5
        [HttpGet("{userId}")]
        public IEnumerable<NotePost> Get(int userId)
        {
            var collection = GetSharedNoteCollection();

            var filter = Builders<Note>.Filter.AnyEq("sharesTo", userId);

            var sharedNotes = collection.Find(filter).ToList();

            return ParseToNotePost(sharedNotes);
        }

        // POST: api/Shares
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Shares
        [HttpPut]
        public IActionResult Put([FromBody] NotePost obj)
        {

            var note = ParseToNewNote(obj);
            var isNew = true;

            var collectionSharedNote = GetSharedNoteCollection();
            var collectionUsers = Database.GetCollection<User>("Users");

            var EmailAddress = obj.SharesTo[0];

            var userFilter = Builders<User>.Filter.Eq("email", EmailAddress);
            var userQuerry = collectionUsers.Find(userFilter);

            if (!userQuerry.Any())
                return BadRequest(new { error = "User doesn't exist" });

            var user = userQuerry.First();


            var noteFilter = Builders<Note>.Filter.Eq("_id", obj.Id);
            var noteQuerry = collectionSharedNote.Find(noteFilter);

            if (noteQuerry.Any())
            {
                isNew = false;
                note = noteQuerry.First();
            }

            note.SharesTo.Add(user.Id);


            //var noteToShareIndex = user.OwnedNotes.IndexOf(new Note { Id = obj.Id });
            //var noteToShare = user.OwnedNotes[noteToShareIndex];

            if (isNew)
            {
                DeleteNote(note.Id);
                collectionSharedNote.InsertOne(note);

            }
            else
            {
                var filter = Builders<Note>.Filter.Eq("_id", note.Id);
                collectionSharedNote.DeleteOne(filter);
                collectionSharedNote.InsertOne(note);


            }







            return Ok(new { obj, EmailAddress });






        }

        // DELETE: api/Shares/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        public bool DeleteNote(string noteId)
        {
            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);
            var filterOne = Builders<User>.Filter.Eq("ownedNotes._id", noteId);

            var user = collection.Find<User>(filterOne).First();

            var filter = new BsonDocument("_id", user.Id);


            var pull = Builders<User>.Update.PullFilter("ownedNotes",
                Builders<Note>.Filter.Eq("_id", noteId));

            var result = collection.UpdateOne(filter, pull);


            return result.IsAcknowledged;


        }


        #region private methods
        private IMongoCollection<Note> GetSharedNoteCollection()
        {
            var collectionName = "SharedNotes";
            return Database.GetCollection<Note>(collectionName);

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

        private Note ParseToNewNote(NotePost notePost)
        {
            Note note = new Note()
            {
                Id = notePost.Id,
                Title = notePost.Title,
                Content = notePost.Content,
                Color = notePost.Color,
                Tags = new List<Shared.Models.Tag>(),
                Lectures = new List<Lecture>(),
                OwnerId = notePost.OwnerId,



                Date = DateTime.Now
            };

            if (note.SharesTo != null)
            {
                note.SharesTo = notePost.SharesTo;
            }


            if (notePost.Tags is null)
                notePost.Tags = new List<string>();

            if (notePost.Lectures is null)
                notePost.Lectures = new List<string>();

            foreach (var item in notePost.Tags)
            {
                note.Tags.Add(new Shared.Models.Tag(item));
            }

            foreach (var item in notePost.Lectures)
            {
                note.Lectures.Add(new Lecture(item));
            }



            return note;
        }


        #endregion

    }
}