using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YourNote.Server.Services;
using YourNote.Shared.Models;
using NHibernate;
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
            session = nhibernateService.OpenSession();

        }

        // GET: api/Note
        [HttpGet]
        public IEnumerable<Note> Get()
        {

            //var completeList = session.CreateCriteria<Object>().List();
            Note testNote = new Note();
            testNote.Owner = "Notatka na serwerze";
            //session.Save(testNote);

            var completeList = session.Statistics;

            IEnumerable<Note> notes = Enumerable.Range(1, 5).Select(index => new Note
            {
                ID = index,
                Owner = session.IsConnected + "",
                Title = completeList.ToString(),
                Color = 0,
                Content = session.ToString(),
                Date = DateTime.Now,
                
            }).ToArray();
            //notes.Append<Note>((Note) completeList[0]);
            
            return notes;
        }

        // GET: api/Note/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
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
        public void Delete(int id)
        {
        }
    }
}
