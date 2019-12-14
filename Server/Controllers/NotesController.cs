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
        private readonly IDatabaseCRUD<Note> DataBaseCRUD;

        public NotesController(ILogger<NotesController> logger,
           IDatabaseCRUD<Note> DatabaseCRUD)
        {
            this.DataBaseCRUD = DatabaseCRUD;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Note> GetAllRecords()
        {
           return DataBaseCRUD.Read();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public Note GetById(int id)
        {
            return DataBaseCRUD.Read(id);
        }

        // POST: api/Notes
        [HttpPost]
        public bool Post([FromBody] Note obj)
        {
            return DataBaseCRUD.Create(obj);
        }

        // PUT: api/Notes
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] Note obj)
        {
            return DataBaseCRUD.Update(id, obj);

        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public void DeleteById(int id)
        {
            DataBaseCRUD.Delete(id);
        }



        #region Private methods
        #endregion 
    }

    //enum UpdateOrSave{ UPDATE, SAVE};
}