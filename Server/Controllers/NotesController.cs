using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using System.Collections.Generic;
using YourNote.Server.Services;
using YourNote.Server.Services.DatabaseService;
using YourNote.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace YourNote.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly IDatabaseService<Note> databaseNote;
        private readonly IDatabaseService<User> databaseUser;

        public NotesController(ILogger<NotesController> logger,
           IDatabaseService<Note> dataBaseNote, IDatabaseService<User> databaseUser)
        {
            this.databaseNote = dataBaseNote;
            this.databaseUser = databaseUser;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Note> GetAllRecords()
        {
           return databaseNote.Read();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public IEnumerable<Note> GetAllRecordsById(int id)
        {
            return databaseUser.Read(id).Notes ?? System.Array.Empty<Note>();
        }

        // POST: api/Notes
        [HttpPost]
        public bool Post([FromBody] Note obj)
        {
            return databaseNote.Create(obj);
        }

        // PUT: api/Notes
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] Note obj)
        {
            return databaseNote.Update(obj);

        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public void DeleteById(int id)
        {
            databaseNote.Delete(id);
        }



        #region Private methods
        #endregion 
    }

    //enum UpdateOrSave{ UPDATE, SAVE};
}