using FluentNHibernate.Mapping;

namespace YourNote.Shared.Models.MappingClasses
{
    internal class NoteMap : ClassMap<Note>
    {
        public NoteMap()
        {
            Table("Notes");

            Id(n => n.Id).GeneratedBy.Identity().Not.Nullable();
            Map(n => n.Date).Default("CURRENT_TIMESTAMP(2)").Not.Nullable();
            Map(n => n.Color);
            Map(n => n.Title);
            Map(n => n.Content);

            References(n => n.Owner);

            References(n => n.Tag).Cascade.All().Nullable();
            References(n => n.Lecture).Cascade.All().Nullable();

            HasManyToMany(n => n.SharedTo)
                .Cascade.All()
                .Table("UserNote")
                .Not.LazyLoad();
        }
    }
}