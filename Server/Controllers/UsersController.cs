using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Server.Services;
using YourNote.Server.Services.DatabaseService;
using YourNote.Shared.Models;


namespace YourNote.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDatabaseService<User> databaseUser;

        private readonly IUserAuthenticateService userService;

        public IMongoDatabase Database { get; }

        public UsersController(ILogger<UsersController> logger,
            IDatabaseService<User> databaseUser,
            IUserAuthenticateService userService,
            IMongoClient client)
        {
            this.databaseUser = databaseUser;
            this.userService = userService;
            Database = client.GetDatabase("YourNote");
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return databaseUser.Read();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public User GetUserById(string id)
        {
            return databaseUser.Read(id);
        }

        // GET: api/Users/5/Notes
        [HttpGet("{userid}/Notes")]
        public IEnumerable<NotePost> GetNotesByUserId(string userId)
        {
            
            var userDoc = databaseUser.Read(userId);
            var Notes = userDoc.Notes;
            return ParseToNotePost(Notes);
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

        // PUT: api/User/5
        [HttpPut("{id}")]
        public  IActionResult Put(string id, [FromBody] User user)
        {
            var result = databaseUser.Update(id, user);

            if (result != null)
                return Ok();
            else
                return BadRequest(new { error = "User doesn't exist" });
        }

        // DELETE: api/User
        [HttpDelete("{userId}")]
        public bool DeleteUserById(string userId)
        {
            return databaseUser.Delete(userId);
        }

        // POST: api/User
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RegisterModel registerModel)
        {
            User user = new User
            {
                EmailAddress = registerModel.EmailAddress,
                Password = registerModel.Password
            };

            user = HashPassword(user);
            var result = databaseUser.Create(user);

            if (result != null)
                return Ok(new RegisterResult { Successful = true });
            else
                return BadRequest(new RegisterResult { Successful = false });
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public  IActionResult Authenticate([FromBody] LoginModel user)
        {
            var userFromDb = databaseUser.Read().FirstOrDefault(u => u.EmailAddress == user.Username);

            if (userFromDb == null || !BCrypt.Net.BCrypt.EnhancedVerify(user.Password, userFromDb.Password))
                return BadRequest(new LoginResult { Successful = false, Error = "Podano niepoprawny login lub has³o." });

            
            //Token
            

            return Ok(new LoginResult { Successful = true});
        }
        
        [HttpPut("role/{userId}/{roleValue}")]
        public IActionResult UpdateRole(string userId, int roleValue)
        {
            var helper = databaseUser.Read(userId);
            helper.Role = (User.Permission)roleValue;

            databaseUser.Update(userId, helper);

            if (helper != null)
                return Ok();
            else
                return BadRequest(new { error = "User doesn't exist" });
        }
        
        #region Private methods

        public static User HashPassword(User user)
        {
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password);
            return user;
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






        #endregion Private methods

    }
}