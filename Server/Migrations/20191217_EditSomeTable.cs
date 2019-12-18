using FluentMigrator;

namespace YourNote.Server.Migrations
{
    [Migration(20191217065600)]
    public class EditSomeTable : Migration
    {
        public override void Up()
        {
            Alter.Table("TestTable")
                 .AddColumn("SomeData").AsDateTime().Nullable();
            Alter.Table("TestTable")
                .AddColumn("SomeData2").AsString();
        }

        public override void Down()
        {
            Delete.Column("SomeData").Column("SomeData2").FromTable("TestTable");
        }
    }
}