using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YourNote.Server.Services;

namespace YourNote.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private FluentMigratorService migratorService;
        private ILogger<AdminController> logger;

        public AdminController(ILogger<AdminController> logger, FluentMigratorService migratorService)
        {

            this.logger = logger;
            this.migratorService = migratorService;

        }


        [HttpPut("{version}")]
        public IActionResult RestoreVersion(long? version)
        {
            var result = migratorService.MigrateTo(version);

            if (result)
                return Ok(result);
            else
                return BadRequest(new { error = "Version doesn't exist" });


        }



    }
}