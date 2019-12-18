using FluentMigrator;

namespace YourNote.Server.Migrations
{
    [Migration(20191217043600)]
    public class StartingVersion : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("users").Row(new { username = "DamianB", password = "$2a$11$f4nmxJR4F4gMa0TFQ5Q4iOwuHCMU/gvuXH4aiYaAWucWLAJGe8lQK", role = "Admin", name = "Damian" });
            Insert.IntoTable("users").Row(new { username = "MichalL", password = "$2a$11$0zLpGGcHX3oSqyLOnvR4beafx49KmdKQq/JCN2Nph.7FMfCQw/iz.", role = "Admin", name = "Michal" });
            Insert.IntoTable("users").Row(new { username = "DominikI", password = "$2a$11$KeB.hvMzejcn2nTQigGbNeIb7lU2fImByj6sRzwEWrZ6/sbHx6TVK", role = "Admin", name = "Dominik" });
        }

        public override void Down()
        {
            Insert.IntoTable("users").Row(new { username = "DamianB", password = "$2a$11$f4nmxJR4F4gMa0TFQ5Q4iOwuHCMU/gvuXH4aiYaAWucWLAJGe8lQK", role = "Admin", name = "Damian" });
            Insert.IntoTable("users").Row(new { username = "MichalL", password = "$2a$11$0zLpGGcHX3oSqyLOnvR4beafx49KmdKQq/JCN2Nph.7FMfCQw/iz.", role = "Admin", name = "Michal" });
            Insert.IntoTable("users").Row(new { username = "DominikI", password = "$2a$11$KeB.hvMzejcn2nTQigGbNeIb7lU2fImByj6sRzwEWrZ6/sbHx6TVK", role = "Admin", name = "Dominik" });
        }
    }
}