using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Shared.Models;


namespace YourNote.Server.Models.MappingClasses
{
    public class LectureMap : ClassMap<Lecture>
    {

        public LectureMap()
        {
            Table("Lecture");

            Id(l => l.Name).GeneratedBy.Assigned();

            HasMany(l => l.Notes)
                .Cascade.All()
                .Not.LazyLoad();

        }
    }
}
