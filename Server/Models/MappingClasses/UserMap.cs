using FluentNHibernate.Mapping;
using YourNote.Shared.Models;

namespace YourNote.Server.Models.MappingClasses
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Not.LazyLoad();
            Table("Users");
            Id(u => u.Id).GeneratedBy.Identity().Not.Nullable();
            Map(u => u.Username).Unique();
            Map(u => u.Password);
            Map(u => u.Role).CustomType<GenericEnumMapper<User.Permission>>();
            Map(u => u.EmailAddress).Unique();
            Map(u => u.Token);
            Map(u => u.Date).Default("CURRENT_TIMESTAMP(2)").Not.Nullable();
            Map(u => u.Name);

            HasMany(u => u.Notes)
                .Cascade.All()
                .Inverse()
                .Table("usernote")
                .Not.LazyLoad();
        }
    }
}