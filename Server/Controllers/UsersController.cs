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

namespace YourNote.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDatabaseService iDbService;
        private readonly IUserService userService;

        public UsersController(ILogger<UsersController> logger,
            IDatabaseService iDbService,
            IUserService userService)
        {
            this.iDbService = iDbService;
            this.userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return iDbService.ReadUser();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public IEnumerable<User> GetUserById(int id)
        {
            return iDbService.ReadUser(id);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] User user)
        {
            return iDbService.UpdateUser(user, id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteUserById(int id)
        {
            iDbService.DeleteUser(id);
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
            var result = iDbService.CreateUser(user);

            if (result)
                return Ok(new RegisterResult { Successful = true });
            else
                return BadRequest(new RegisterResult { Successful = false });
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel user)
        {
            var userFromDb = iDbService.ReadUser().FirstOrDefault(u => u.Username == user.Username);

            if (userFromDb == null || !BCrypt.Net.BCrypt.EnhancedVerify(user.Password, userFromDb.Password))
                return BadRequest(new LoginResult { Successful = false, Error = "Podano niepoprawny login lub has³o." });

            userFromDb.Token = userService.Authenticate(userFromDb);

            iDbService.UpdateUser(userFromDb, userFromDb.ID);

            return Ok(new LoginResult { Successful = true, Token = userFromDb.Token });
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
