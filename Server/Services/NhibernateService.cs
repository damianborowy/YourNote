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
        private static string[] connectionData = GetConnectionData();
        private static string Host = connectionData[0];
        private static string Port = connectionData[1];
        private static string DBname = connectionData[2];
        private static string User = connectionData[3]; 
        private static string Password = connectionData[4]; 
        
        




        public static ISessionFactory SessionFactory { get; set; }

        public NhibernateService()
        {
            SessionFactory = CreateSessionFactory();
        }
       
        public static ISessionFactory CreateSessionFactory()
        {

            bool createNew = false;
            
            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL81
                .ConnectionString($" Host={Host}; Port={Port}; Database={DBname};" +
                $" Username={User}; Password={Password};"))              
                .Mappings(m => m.FluentMappings
                .AddFromAssemblyOf<NoteMap>()
                .AddFromAssemblyOf<UserMap>())
                
                .ExposeConfiguration(cfg => new SchemaExport(cfg)     
                .Create(createNew, createNew))
                
                .BuildSessionFactory();
        }

        public ISession OpenSession() => SessionFactory.OpenSession();

        private static string[] GetConnectionData() => Environment.GetEnvironmentVariable("PGPASSFILE").Split(':');
        

           
    }
}
