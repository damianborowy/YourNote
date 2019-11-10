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
            Id(n => n.ID).GeneratedBy.Assigned();
            Map(n => n.Owner);
            Map(n => n.Color);
            Map(n => n.Date);
            Map(n => n.Title);
            
        }
    }
}
