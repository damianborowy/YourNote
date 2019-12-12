using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using System.Collections.Generic;
using YourNote.Server.Services;
using YourNote.Shared.Models;

namespace YourNote.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly ILogger<NotesController> logger;
        private readonly NhibernateService nhibernateService;

        public NotesController(ILogger<NotesController> logger,
            NhibernateService nhibernateService)
        {
            this.logger = logger;
            this.nhibernateService = nhibernateService;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Note> GetAllNotes()
        {
           return nhibernateService.ReadNote();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public IEnumerable<Note> GetNoteById(int id)
        {
            return nhibernateService.ReadNote(id);
        }

        // POST: api/Notes
        [HttpPost]
        public bool Post([FromBody] Note note)
        {
            return nhibernateService.CreateNote(note);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody] Note note)
        {
            return nhibernateService.UpdateNote(note, id);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteNoteById(int id)
        {
            nhibernateService.DeleteNote(id);
        }



        #region Private methods
        #endregion 
    }

    //enum UpdateOrSave{ UPDATE, SAVE};
}