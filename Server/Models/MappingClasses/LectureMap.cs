using FluentNHibernate.Mapping;
using YourNote.Shared.Models;

namespace YourNote.Server.Models.MappingClasses
{
    public class LectureMap : ClassMap<Lecture>
    {
        public LectureMap()
        {
            Table("Lecture");

            Id(l => l.Id).GeneratedBy.Identity().Not.Nullable();
            Map(l => l.Name);

            HasMany(l => l.Notes)
                .Cascade.All()
                .Not.LazyLoad();
        }
    }
}