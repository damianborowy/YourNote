using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Shared.Models;

namespace YourNote.Server.Models.MappingClasses
{
    public class UserNoteMap : ClassMap<UserNote>
    {

        public UserNoteMap()
        {
            CompositeId()
                .KeyReference(x => x.UserId)
                .KeyReference(x => x.NoteId);

            Map(x => x.isOwner);


        }

    }
}
