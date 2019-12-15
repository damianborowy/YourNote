using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;
using YourNote.Shared.Models;

namespace YourNote.Shared.Models.MappingClasses
{
    class NoteMap : ClassMap<Note>
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
            HasManyToMany(n => n.SharedTo)
                .Cascade.All()
                .Table("UserNote");


        }
    }
}
