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

            #endregion Dodawanie notatek testowo

            using (var session = GetSession())
                return session.QueryOver<Note>().List<Note>();
        }

        // GET: api/Notes/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<Note> GetNoteById(int id)
        {
            using (var session = GetSession())
                return session.QueryOver<Note>().Where(n => n.ID == id).List<Note>();
        }

        // POST: api/Notes
        [HttpPost]
        public void Post([FromBody] Note note)
        {
            AddNote(note);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Note note)
        {
            AddNote(note, id);
            
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteNoteById(int id)
        {
            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                session.Delete("Note",id);
                session.Flush();
                tx.Commit();
            }
        }



        #region Private methods

        private NHibernate.ISession GetSession() => nhibernateService.OpenSession();

       


        private void AddNote(Note note, int id = -1)
        {
            using (var session = GetSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    if (id == -1)
                        session.SaveOrUpdate(note);
                    else
                        session.SaveOrUpdate("Note", note, id);
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