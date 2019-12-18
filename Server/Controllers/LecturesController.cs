using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
        public IEnumerable<Lecture> GetAllRecords()
        {
            return databaseLecture.Read();
        }
    }
}