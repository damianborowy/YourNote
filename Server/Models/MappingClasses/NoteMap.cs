using FluentNHibernate.Mapping;

namespace YourNote.Shared.Models.MappingClasses
{
    internal class NoteMap : ClassMap<Note>
    {
        public NoteMap()
        {
            Not.LazyLoad();
            Table("Notes");
            
            Id(n => n.Id).GeneratedBy.Identity().Not.Nullable();
            Map(n => n.Date).Default("CURRENT_TIMESTAMP(2)").Not.Nullable();
            Map(n => n.Color);
            Map(n => n.Title);
            Map(n => n.Content);

            

            References(n => n.Tag).Cascade.All().Nullable();
            References(n => n.Lecture).Cascade.All().Nullable();

            HasMany(n => n.Users)
                .Cascade.All()                
                .Inverse()
                .Table("usernote")
                .Not.LazyLoad(); 


        }
    }
}