using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using YourNote.Server.Services;

namespace YourNote.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        
        private ILogger<AdminController> logger;
        FluentMigratorService migratorService;
        public AdminController(ILogger<AdminController> logger, FluentMigratorService migratorService)
        {
            this.logger = logger;
            this.migratorService = migratorService;
        }

        [HttpGet]
        public IList<Object> Get()
        {
            var list = migratorService.OpenSession().CreateSQLQuery("SELECT * FROM public.\"VersionInfo\"").List<Object>();

            return list;
        }

        [HttpPut("down/{version}")]
        public IActionResult RestoreVersionDown(long? version)
        {
            var result = migratorService.MigrateDown(version);

            if (result)
                return Ok(result);
            else
                return BadRequest(new { error = "Version doesn't exist" });
        }

        [HttpPut("down/")]
        public IActionResult RestoreVersionDown()
        {
            var result = migratorService.MigrateDown(null);

            if (result)
                return Ok(result);
            else
                return BadRequest(new { error = "Version doesn't exist" });
        }

        [HttpPut("up/{version}")]
        public IActionResult RestoreVersionUp(long? version)
        {
            var result = migratorService.MigrateUp(version);

            return Ok(true);
        }

        [HttpPut("up/")]
        public IActionResult RestoreVersionUp()
        {
            var result = migratorService.MigrateUp(null);

            return Ok(true);
        }
    }
}