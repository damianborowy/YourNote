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

namespace YourNote.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;
        private readonly NhibernateService nhibernateService;
        private readonly IUserService userService;

        public UsersController(ILogger<UsersController> logger,
            NhibernateService nhibernateService,
            IUserService userService)
        {
            this.logger = logger;
            this.nhibernateService = nhibernateService;
            this.userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return nhibernateService.ReadUser();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public IEnumerable<User> GetUserById(int id)
        {
            return nhibernateService.ReadUser(id);
        }

        // POST: api/User
        [HttpPost]
        public bool Post([FromBody] User user)
        {
            user = HashPassword(user);
            return nhibernateService.CreateUser(user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] User user)
        {
            return nhibernateService.UpdateUser(user, id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteUserById(int id)
        {
            nhibernateService.DeleteUser(id);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            HttpContext.Response.Headers.Add("x-auth-token", user.Token);
            return Ok(user.Username);
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
