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
        private readonly IDatabaseService<User> DataBaseCRUD;

        private readonly IUserAuthenticateService userService;

        public UsersController(ILogger<UsersController> logger,
            IDatabaseService<User> DataBaseCRUD,
            IUserAuthenticateService userService)
        {
            this.DataBaseCRUD = DataBaseCRUD;
            this.userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return DataBaseCRUD.Read();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public User GetUserById(int id)
        {
            return DataBaseCRUD.Read(id);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] User user)
        {
            return DataBaseCRUD.Update(user);
        }

        // DELETE: api/User
        [HttpDelete("{id}")]
        public void DeleteUserById(int id)
        {
            DataBaseCRUD.Delete(id);
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
            var result = DataBaseCRUD.Create(user);

            if (result)
                return Ok(new RegisterResult { Successful = true });
            else
                return BadRequest(new RegisterResult { Successful = false });
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] LoginModel user)
        {
            var userFromDb = DataBaseCRUD.Read().FirstOrDefault(u => u.Username == user.Username);

            if (userFromDb == null || !BCrypt.Net.BCrypt.EnhancedVerify(user.Password, userFromDb.Password))
                return BadRequest(new LoginResult { Successful = false, Error = "Podano niepoprawny login lub has³o." });

            userFromDb.Token = userService.Authenticate(userFromDb);

            DataBaseCRUD.Update(userFromDb);

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
