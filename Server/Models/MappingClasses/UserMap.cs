using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Shared.Models;

namespace YourNote.Server.Models.MappingClasses
{
    public class UserMap : ClassMap<User>
    {

        public UserMap()
        {
            Table("Users");
            Id(u => u.ID).GeneratedBy.Identity().Not.Nullable();
            Map(u => u.Username).Unique();
            Map(u => u.Name);
            Map(u => u.EmailAddress);
            Map(u => u.Date).Default("CURRENT_TIMESTAMP(2)").Not.Nullable();
            Map(u => u.Token);
            Map(u => u.Password);
        }



    }
}
