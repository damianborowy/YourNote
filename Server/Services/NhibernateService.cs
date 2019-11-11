using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Hosting;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using YourNote.Shared.Models.MappingClasses;
using System.Data.SqlClient;
using YourNote.Server.Models.MappingClasses;

namespace YourNote.Server.Services
{
    public class NhibernateService
    {


        // Obtain connection string information from the portal
        //
        private static string Host = "pbds.postgres.database.azure.com";
        private static string User = "login@pbds";
        private static string DBname = "postgres";
        private static string Password = "pass@word1";
        private static int Port = 5432;
        
        public static ISessionFactory SessionFactory { get; set; }

        public NhibernateService()
        {
            SessionFactory = CreateSessionFactory();
        }
       
        public static ISessionFactory CreateSessionFactory()
        {

           
            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL81
                .ConnectionString(c => c
                .Host(Host)
                .Database(DBname)
                .Port(Port)
                .Username(User)
                .Password(Password)))
                .Mappings(m => m.FluentMappings
                .AddFromAssemblyOf<NoteMap>()
                .AddFromAssemblyOf<UserMap>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(false, false))
                .BuildSessionFactory();
        }

        public ISession OpenSession() => SessionFactory.OpenSession();


    }
}
