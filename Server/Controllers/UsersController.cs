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

        public UsersController(ILogger<UsersController> logger,
            NhibernateService nhibernateService)
        {
            this.logger = logger;
            this.nhibernateService = nhibernateService;
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

        //JWT lehovitz modification 11.12.2019
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        //end of my mod ~lehovitz 

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
