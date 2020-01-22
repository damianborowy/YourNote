using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
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
        
        private readonly IDatabaseService<Lecture> databaseLecture;
        public IMongoDatabase Database { get; }

        public NotesController(ILogger<NotesController> logger,
           IDatabaseService<Note> dataBaseNote, IDatabaseService<User> databaseUser,
            IDatabaseService<Lecture> databaseLecture,
           MongoClient client)
        {
            this.databaseNote = dataBaseNote;
            this.databaseUser = databaseUser; 
            this.databaseLecture = databaseLecture;
            Database = client.GetDatabase("YourNote");


        }

        

        // GET: api/Notes/5
        [HttpGet("{userId}")]
        public IEnumerable<NotePost> GetAllRecordsById(string userId)
        {

            var User = GetUser(userId);
            return ParseToNotePost(User.OwnedNotes);
            

        }

        // POST: api/Notes
        [HttpPost]
        public IActionResult Post([FromBody] NotePost obj)
        {
            var note = ParseToNewNote(obj);
            note.Id = ObjectId.GenerateNewId().ToString();

            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);
            var filter = Builders<User>.Filter.Eq("_id", obj.OwnerId);

            //var note = SetTagAndLecture(obj);
            var userQuerry = collection.Find<User>(filter);
            if(!userQuerry.Any())
                return BadRequest(new { error = "User doesn't exist" });

            var user = userQuerry.First();
            var newTags = note.Tags.Except(user.AllTags);
            var newLectures = note.Lectures.Except(user.AllLectures);


            
            
           


            var addNote = Builders<User>.Update.AddToSet("ownedNotes", note);

            UpdateDefinition<User> update;;

            if (obj.Tags != null & obj.Lectures != null)
            {
                
                var addTags = Builders<User>.Update.AddToSetEach("allTags", newTags);
                
                var addLectures = Builders<User>.Update.AddToSetEach("allLectures", newLectures);



               update = Builders<User>.Update.Combine(addNote, addTags, addLectures);

            }
            else
            {

                update = Builders<User>.Update.Combine(addNote);

                if (obj.Tags != null)
                {
                    var addTags = Builders<User>.Update.AddToSetEach("allTags", newTags);
                    update = Builders<User>.Update.Combine(addNote, addTags);

                }
              
               
                if(obj.Tags != null)
                {
                    var addLectures = Builders<User>.Update.AddToSetEach("allLectures", newLectures);

                    update = Builders<User>.Update.Combine(addNote, addLectures);
                }


            }







           
                collection.FindOneAndUpdate(filter, update);
                return Ok(note);
            
            

        }

        // PUT: api/Notes
        [HttpPut]
        public IActionResult Put([FromBody] NotePost obj)
        {

            var note = ParseToNewNote(obj);
            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);

            var filter = Builders<User>.Filter.Eq("_id", obj.OwnerId);

            
          
            var userQuerry = collection.Find<User>(filter);
            if (!userQuerry.Any())
                return BadRequest(new { error = "User doesn't exist" });

            var user = userQuerry.First();
            var newTags = note.Tags.Except(user.AllTags);
            var newLectures = note.Lectures.Except(user.AllLectures);

            filter = Builders<User>.Filter.Eq("_id", obj.OwnerId)
                       & Builders<User>.Filter.Eq("ownedNotes._id", obj.Id);

            var updateNote = Builders<User>.Update.Set(model => model.OwnedNotes[-1], note);

            UpdateDefinition<User> update; ;

            if (obj.Tags.Count != 0 & obj.Lectures.Count != 0)
            {

                var addTags = Builders<User>.Update.AddToSetEach("allTags", newLectures);

                var addLectures = Builders<User>.Update.AddToSetEach("allLectures", newTags);



                update = Builders<User>.Update.Combine(updateNote, addTags, addLectures);

            }
            else
            {

                update = Builders<User>.Update.Combine(updateNote);

                if (obj.Tags.Count != 0 & newTags.Any())
                {
                    var addTags = Builders<User>.Update.AddToSetEach("allTags", newTags);
                    update = Builders<User>.Update.Combine(updateNote, addTags);

                }


                if (obj.Tags.Count != 0 & newLectures.Any())
                {
                    var addLectures = Builders<User>.Update.AddToSetEach("allLectures", newLectures);

                    update = Builders<User>.Update.Combine(updateNote, addLectures);
                }


            }








          var result = collection.UpdateOne(filter, update);


            if(result.MatchedCount > 0 )
                return Ok(obj);
            else
                return BadRequest( new {error = "Couldn't update the note"});


            
            

        }

        // DELETE: api/Notes/5
        [HttpDelete("{noteId}")]
        public IActionResult DeleteNote(string noteId)
        {
            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);
            var filterOne = Builders<User>.Filter.Eq("ownedNotes._id", noteId);

            var user = collection.Find<User>(filterOne).First();

            var filter = new BsonDocument("_id", user.Id);


            var pull = Builders<User>.Update.PullFilter("ownedNotes",
                Builders<Note>.Filter.Eq("_id", noteId));
                
            var result = collection.UpdateOne(filter, pull);
            
            if (result.IsAcknowledged)
                return Ok(result.IsAcknowledged);
            else
                return BadRequest(new { error = "Note doesn't exist" });

        }

        #region Private methods

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


                Date = DateTime.Now
            };

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
   
        private User GetUser(string userId)
        {
            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);

            var filter = Builders<User>.Filter.Eq("_id", userId);
            //var projection = Builders<User>.Projection.Include("ownedNotes");
            return collection.Find(filter).First();
        }
        #endregion Private methods
    }
}