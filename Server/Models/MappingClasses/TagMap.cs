using FluentNHibernate.Mapping;
using YourNote.Shared.Models;

namespace YourNote.Server.Models.MappingClasses
{
    public class TagMap : ClassMap<Tag>
    {
        public TagMap()
        {
            Table("Tag");

            Id(t => t.Id).GeneratedBy.Identity().Not.Nullable();
            Map(t => t.Name);

            HasMany(t => t.Notes)
                .Cascade.All()
                .Inverse()
                .Not.LazyLoad();
        }
    }
}