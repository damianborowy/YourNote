//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using YourNote.Server.Services;
using YourNote.Shared.Models;

namespace YourNote.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly ILogger<NoteController> logger;
        private readonly NhibernateService nhibernateService;
        private readonly ISession session;

        public NoteController(ILogger<NoteController> logger,
            NhibernateService nhibernateService)
        {
            this.logger = logger;
            this.nhibernateService = nhibernateService;
            
        }

        // GET: api/Note
        [HttpGet]
        public IEnumerable<Note> GetAllNotes()
        {
            #region Dodawanie notatek testowo
            /*
            using (var session = GetSession())
            using (ITransaction tx = session.BeginTransaction())
            {

                var note = new Note();

                note.Title = "DDDDD";
                session.SaveOrUpdate(note);
                session.Flush();
                tx.Commit();
            }
            using (var session = GetSession())
            using (ITransaction tx = session.BeginTransaction())
            {

                var note = new Note();

                note.Title = "ZZZZZ";
                session.Save(note);
                session.Flush();
                tx.Commit();
            }

            */
            #endregion

            using (var session = GetSession())
                return session.QueryOver<Note>().List<Note>();
           

        }

        // GET: api/Note/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<Note> GetNoteById(int id)
        {
            using (var session = GetSession())
                return session.QueryOver<Note>().Where(n => n.ID == id).List<Note>();
        }

        // POST: api/Note
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Note/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteNoteWithId(int id)
        {
            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {

                session.Delete(id);
                session.Flush();
                tx.Commit();
            }
            
                
            
        }






        private ISession GetSession()
        {
            return nhibernateService.OpenSession(); 
        }
    }
}