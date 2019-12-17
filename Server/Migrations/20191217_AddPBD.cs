using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;

namespace YourNote.Server.Migrations
{
    [Migration(20191217043100)]
    public class AddPBD : Migration
    {
        public override void Up()
        {

            Create.Table("PBD")
                 .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("Text").AsString();
        }

        public override void Down()
        {

            Delete.Table("PBD");


        }
    }
}
