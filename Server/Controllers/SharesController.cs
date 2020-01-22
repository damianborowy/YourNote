using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        // PUT: api/Shares/5
        
        //[HttpPut]
        //public IActionResult Put([FromBody] NotePost obj)
        //{

        //    var note = ParseToNewNote(obj);

        //    var collection = GetSharedNoteCollection();

        //    var filter = Builders<Note>.Filter.Eq("_id", obj.OwnerId);



        //    var userQuerry = collection.Find<Note>(filter);
        //    if (!userQuerry.Any())
        //        return BadRequest(new { error = "User doesn't exist" });

        //    var user = userQuerry.First();
        //    var newTags = note.Tags.Except(user.AllTags);
        //    var newLectures = note.Lectures.Except(user.AllLectures);

        //    filter = Builders<Note>.Filter.Eq("_id", obj.OwnerId)
        //               & Builders<Note>.Filter.Eq("ownedNotes._id", obj.Id);

        //    var updateNote = Builders<User>.Update.Set(model => model.OwnedNotes[-1], note);

        //    UpdateDefinition<User> update; 

        //    if (obj.Tags != null & obj.Lectures != null)
        //    {

        //        var addTags = Builders<User>.Update.AddToSetEach("allTags", newLectures);

        //        var addLectures = Builders<User>.Update.AddToSetEach("allLectures", newTags);



        //        update = Builders<User>.Update.Combine(updateNote, addTags, addLectures);

        //    }
        //    else
        //    {

        //        update = Builders<User>.Update.Combine(updateNote);

        //        if (obj.Tags != null)
        //        {
        //            var addTags = Builders<User>.Update.AddToSetEach("allTags", newTags);
        //            update = Builders<User>.Update.Combine(updateNote, addTags);

        //        }


        //        if (obj.Tags != null)
        //        {
        //            var addLectures = Builders<User>.Update.AddToSetEach("allLectures", newLectures);

        //            update = Builders<User>.Update.Combine(updateNote, addLectures);
        //        }


        //    }








        //    var result = collection.UpdateOne(filter, update);


        //    if (result.MatchedCount > 0)
        //        return Ok(note);
        //    else
        //        return BadRequest(new { error = "Couldn't update the note" });





        //}

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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