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
            Table("Note");
            Id(n => n.ID).GeneratedBy.Identity().Not.Nullable();
            Map(n => n.Owner);
            Map(n => n.Color);
            Map(n => n.Date).Default("CURRENT_TIMESTAMP(2)").Not.Nullable();
            Map(n => n.Title);
            Map(n => n.Content);
            
        }
    }
}
