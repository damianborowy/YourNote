using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
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

        private readonly IDatabaseService<Lecture> databaseLecture;
        public IMongoDatabase Database { get; }

        public NotesController(ILogger<NotesController> logger,
           IDatabaseService<Note> dataBaseNote, IDatabaseService<User> databaseUser
           , IDatabaseService<Lecture> databaseLecture, IMongoClient client)
        {
            this.databaseNote = dataBaseNote;
            this.databaseUser = databaseUser;
            this.databaseLecture = databaseLecture;
            Database = client.GetDatabase("YourNote");


        }

        // POST: api/Notes
        [HttpPost("{userId}/Notes")]
        public IActionResult PostNote(string userId, [FromBody] Note note)
        {

            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);

            var filter = Builders<User>.Filter.Eq("id", userId);
            //                & Builders<BsonDocument>.Filter.Eq("scores.type", "quiz");

            var update = Builders<User>.Update.Push("notes", note);

            var result = collection.FindOneAndUpdate(filter, update);


            if (result is null)
                return Ok(note);
            else
                return BadRequest(new { error = "User doesn't exist" });


        }

        // POST: api/Notes
        [HttpPut("{userId}/Notes")]
        public IActionResult PutNote(string userId, [FromBody] Note note)
        {

            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);

            var filter = Builders<User>.Filter.Eq("id", userId)
                       & Builders<User>.Filter.Eq("notes.id", note.Id);

            var update = Builders<User>.Update.Push("notes", note);

            var result = collection.FindOneAndUpdate(filter, update);


            if (result is null)
                return Ok(note);
            else
                return BadRequest(new { error = "User doesn't exist" });


        }

        // POST: api/Notes
        [HttpDelete("{userId}/Notes")]
        public IActionResult DeleteNote(string userId, [FromBody] Note note)
        {

            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);

            var filter = Builders<User>.Filter.Eq("id", userId)
                       & Builders<User>.Filter.Eq("notes.id", note.Id);

            var result = collection.DeleteOne(filter);


            if (result.IsAcknowledged)
                return Ok(note);
            else
                return BadRequest(new { error = "User doesn't exist" });

        }
    }
}