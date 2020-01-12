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
            Table("usernote");
            CompositeId()
                .KeyReference(x => x.UserId, "user_id")
                .KeyReference(x => x.NoteId, "note_id");

            Map(x => x.IsOwner);


        }

    }
}
