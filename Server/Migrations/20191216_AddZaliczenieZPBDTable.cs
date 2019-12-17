using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;

namespace YourNote.Server.Migrations
{
    [Migration(20191217022800)]
    public class AddZaliczenieZPBDTable : Migration
    {
        public override void Up()
        {
            Create.Table("ZaliczenieZPBD")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Text").AsString();
        }

        public override void Down()
        {
            Delete.Table("ZaliczenieZPBD");
        }
    }
}
