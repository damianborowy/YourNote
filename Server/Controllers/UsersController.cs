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
            MongoClient client)
        {
            this.databaseUser = databaseUser;
            this.userService = userService;
            Database = client.GetDatabase("YourNote");
        }

        

        // GET: api/Users/5
        [HttpGet("{id}")]
        public User GetUserById(string id)
        {
            return databaseUser.Read(id);
        }
              

        // PUT: api/User/5
        [HttpPut]
        public  IActionResult Put([FromBody] User user)
        {

            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);
            collection.InsertOne(user);

            if (user != null)
                return Ok();
            else
                return BadRequest(new { error = "User is null" });
        }

        // DELETE: api/User
        [HttpDelete("{userId}")]
        public bool DeleteUserById(string userId)
        {
            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);
            var filter = Builders<User>.Filter.Eq("id", userId);
            var result = collection.DeleteOne(filter);

            return result.IsAcknowledged;
        }

        // POST: api/User
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RegisterModel registerModel)
        {
            User user = new User
            {
                EmailAddress = registerModel.EmailAddress,
                Password = registerModel.Password,
                Id = ObjectId.GenerateNewId().ToString()

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

            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);
            var filter = Builders<User>.Filter.Eq("email", user.EmailAddress);

            var userFromDb = collection.Find(filter).First();

            if (userFromDb == null || !BCrypt.Net.BCrypt.EnhancedVerify(user.Password, userFromDb.Password))
                return BadRequest(new LoginResult { Successful = false, Error = "Podano niepoprawny login lub has³o." });

            
            //Token
            

            return Ok(new LoginResult { Successful = true});
        }
        
        [HttpPut("role/{userId}/{roleValue}")]
        public IActionResult UpdateRole(string userId, int roleValue)
        {
            var collectionName = "Users";
            var collection = Database.GetCollection<User>(collectionName);
            var filter = Builders<User>.Filter.Eq("id", userId);
            var updateRole = Builders<User>.Update.Set("role", (User.Permission)roleValue);


            var result = collection.UpdateOne(filter, updateRole);

            if (result.IsAcknowledged)
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

        






        #endregion Private methods

    }
}