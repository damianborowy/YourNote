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
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;
        private readonly IDatabaseService iDbService;
        private readonly IUserService userService;

        public UsersController(ILogger<UsersController> logger,
            IDatabaseService iDbService,
            IUserService userService)
        {
            this.logger = logger;
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

        // POST: api/User
        [HttpPost]
        public bool Post([FromBody] User user)
        {
            user = HashPassword(user);
            return iDbService.CreateUser(user);
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

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User user)
        {
            var userFromDb = iDbService.ReadUser().FirstOrDefault(u => u.Username == user.Username);

            if (userFromDb == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var token = userService.Authenticate(userFromDb);

            return Ok(token);
        }

        #region Private methods

      

        public User HashPassword(User user)
        {
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password);
            return user;
        }
        #endregion 
    }
}
