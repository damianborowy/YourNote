namespace YourNote.Shared.Models
{
    public class MigrationOptions
    {
        public string ConnectionString { get; set; }

        public long? Version { get; set; }
    }
}