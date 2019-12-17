using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Server.Services.DatabaseService;
using YourNote.Shared.Models;

namespace YourNote.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LecturesController : ControllerBase
    {
        private readonly IDatabaseService<Lecture> databaseLecture;
        public LecturesController(ILogger<LecturesController> logger,
           IDatabaseService<Lecture> databaseLecture)
        {
            this.databaseLecture = databaseLecture;
        }
        // GET: api/Lectures
        [HttpGet]
        public IEnumerable<string> GetAllRecords()
        {
            var list = new List<string>();

            foreach (var item in databaseLecture.Read())
            {
                if (item.Name != null)
                    list.Add(item.Name);
            }

            return list;
        }
    }
}
