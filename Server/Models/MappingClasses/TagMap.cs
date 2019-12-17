using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Shared.Models;


namespace YourNote.Server.Models.MappingClasses
{
    public class TagMap : ClassMap<Tag>
    {

        public TagMap()
        {
            Table("Tag");
            
            Id(t=>t.Id).GeneratedBy.Identity().Not.Nullable();
            Map(t => t.Name);

            HasMany(t => t.Notes)
                .Cascade.All()
                .Not.LazyLoad();

        }
    }
}
