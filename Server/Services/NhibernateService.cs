using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;

using YourNote.Shared.Models.MappingClasses;
using YourNote.Server.Models.MappingClasses;
using YourNote.Server.Services.DatabaseService;
using YourNote.Shared.Models;
using System.Collections.Generic;

namespace YourNote.Server.Services
{
    public class NhibernateService : IDatabaseService
    {
        // Obtain connection string information from the portal
        //
        private static string[] connectionData = GetConnectionData();
        private static string Host = connectionData[0];
        private static string Port = connectionData[1];
        private static string DBname = connectionData[2];
        private static string User = connectionData[3];
        private static string Password = connectionData[4];

        private readonly NhibernateService nhibernateService;
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
                    .AddFromAssemblyOf<UserMap>()
                )
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(createNew, createNew))
                .BuildSessionFactory();
        }

        public ISession OpenSession() => SessionFactory.OpenSession();

        private static string[] GetConnectionData() => Environment.GetEnvironmentVariable("PGPASSDATA").Split(':');

        public bool CreateUser(User obj)
        {
            return AddOrUpdateUser(obj);
        }

        public IEnumerable<User> ReadUser(int? id = null)
        {
            if(id!=null)
            {
                using (var session = GetSession())
                    return session.QueryOver<User>().Where(n => n.ID == id).List<User>();
            }
            else
            {
                using (var session = GetSession())
                    return session.QueryOver<User>().List<User>();
            }
        }

        public bool UpdateUser(User obj, int id)
        {
            return AddOrUpdateUser(obj, id);
        }

        public bool DeleteUser(int id)
        {
            bool wasSucceeded = false;
            using (var session = GetSession())
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    wasSucceeded = true;
                    session.Delete("User", id);
                    session.Flush();
                    tx.Commit();
                }
                catch (NHibernate.HibernateException)
                {
                    wasSucceeded = false;
                    throw;
                }
                
            }
            return wasSucceeded;
        }

        public bool CreateNote(Note obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> ReadNote(int? id = null)
        {
            throw new NotImplementedException();
        }

        public bool UpdateNote(Note obj, int id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteNote(int id)
        {
            throw new NotImplementedException();
        }

        #region privateMethods
        private NHibernate.ISession GetSession() => nhibernateService.OpenSession();
        private bool AddOrUpdateUser(User user, int id = -1)
        {
            bool wasSucceeded = false;
            using (var session = GetSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    if (id == -1)
                    {
                        session.SaveOrUpdate(user);
                    }
                    else
                    {
                        session.SaveOrUpdate("User", user, id);
                    }
                    wasSucceeded = true;
                    session.Flush();
                    tx.Commit();      
                }
                catch (NHibernate.HibernateException)
                {
                    tx.Rollback();
                    wasSucceeded = false;
                    throw;
                }
            }
            return wasSucceeded;
        }

        #endregion
    }
}
