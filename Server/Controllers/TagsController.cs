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
    public class TagsController
    {
        private readonly IDatabaseService<Tag> databaseTag;
        public TagsController(ILogger<TagsController> logger,
           IDatabaseService<Tag> databaseTag)
        {
            this.databaseTag = databaseTag;
        }
        // GET: api/Tags
        [HttpGet]
        public IEnumerable<Tag> GetAllRecords()
        {
            

           

            return databaseTag.Read();
        }
    }
}
