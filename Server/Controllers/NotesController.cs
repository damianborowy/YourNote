using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using System.Collections.Generic;
using YourNote.Server.Services;
using YourNote.Server.Services.DatabaseService;
using YourNote.Shared.Models;

namespace YourNote.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly ILogger<NotesController> logger;
        private readonly IDatabaseService iDbService;

        public NotesController(ILogger<NotesController> logger,
            IDatabaseService iDbService)
        {
            this.logger = logger;
            this.iDbService = iDbService;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Note> GetAllNotes()
        {
           return iDbService.ReadNote();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public IEnumerable<Note> GetNoteById(int id)
        {
            return iDbService.ReadNote(id);
        }

        // POST: api/Notes
        [HttpPost]
        public bool Post([FromBody] Note note)
        {
            return iDbService.CreateNote(note);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] Note note)
        {
            return iDbService.UpdateNote(note, id);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteNoteById(int id)
        {
            iDbService.DeleteNote(id);
        }



        #region Private methods
        #endregion 
    }

    //enum UpdateOrSave{ UPDATE, SAVE};
}