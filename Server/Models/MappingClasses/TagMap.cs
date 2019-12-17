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
            
            Id(t => t.Name).GeneratedBy.Assigned();

            HasMany(t => t.Notes)
                .Cascade.All()
                .Not.LazyLoad();

        }
    }
}
