using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NHibernate;
using YourNote.Server.Services;
using YourNote.Shared.Models;

namespace YourNote.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        public FluentMigratorService migratorService;
        private ILogger<AdminController> logger;

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

        [HttpPut("up/{version}")]
        public IActionResult RestoreVersionUp(long? version)
        {
            var result = migratorService.MigrateUp(version);

            if (result)
                return Ok(result);
            else
                return BadRequest(new { error = "Version doesn't exist" });


        }

    }
}