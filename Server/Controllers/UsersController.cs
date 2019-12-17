using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YourNote.Server.Services;
using YourNote.Shared.Models;
using NHibernate;
using Microsoft.AspNetCore.Authorization;
using YourNote.Server.Services.DatabaseService;
using static YourNote.Shared.Models.User;

namespace YourNote.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDatabaseService<User> databaseUser;

        private readonly IUserAuthenticateService userService;

        public UsersController(ILogger<UsersController> logger,
            IDatabaseService<User> databaseUser,
            IUserAuthenticateService userService)
        {
            this.databaseUser = databaseUser;
            this.userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return databaseUser.Read();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public User GetUserById(int id)
        {
            return databaseUser.Read(id);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            var result = databaseUser.Update(user);

            if (result != null)
                return Ok();
            else
                return BadRequest(new { error = "User doesn't exist" });
        }

        
        // DELETE: api/User
        [HttpDelete("{id}")]
        public bool DeleteUserById(int id)
        {
            return databaseUser.Delete(id);
        }

        // POST: api/User
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] RegisterModel registerModel)
        {
            User user = new User
            {
                Username = registerModel.Username,
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
        public IActionResult Authenticate([FromBody] LoginModel user)
        {
            var userFromDb = databaseUser.Read().FirstOrDefault(u => u.Username == user.Username);

            if (userFromDb == null || !BCrypt.Net.BCrypt.EnhancedVerify(user.Password, userFromDb.Password))
                return BadRequest(new LoginResult { Successful = false, Error = "Podano niepoprawny login lub has³o." });

            userFromDb.Token = userService.Authenticate(userFromDb);

            databaseUser.Update(userFromDb);

            return Ok(new LoginResult { Successful = true, Token = userFromDb.Token });
        }

        [HttpPatch("role/{userId}/{roleValue}")]
        public IActionResult UpdateRole(int id, int role)
        {
            var helper = databaseUser.Read(id);
            helper.Role = (Permission)role;

            databaseUser.Update(helper);

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
        #endregion 
    }
}
