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
            using (var session = GetSession())
                return session.QueryOver<User>().List<User>();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public IEnumerable<User> GetUserById(int id)
        {
            using (var session = GetSession())
                return session.QueryOver<User>().Where(n => n.ID == id).List<User>();
        }

        // POST: api/User
        [HttpPost]
        public void Post([FromBody] User user)
        {
            AddUser(user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User user)
        {
            AddUser(user, id);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteUserById(int id)
        {
            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Delete("User", id);
                session.Flush();
                tx.Commit();
            }
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

        private NHibernate.ISession GetSession() => nhibernateService.OpenSession();

        private void AddUser(User user, int id = -1)
        {
            using (var session = GetSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    if (id == -1)
                        session.SaveOrUpdate(user);
                    else
                        session.SaveOrUpdate("User", user, id);
                    session.Flush();
                    tx.Commit();
                }
                catch (NHibernate.HibernateException)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        #endregion 
    }
}
