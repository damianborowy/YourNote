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

        public static ISessionFactory SessionFactory { get; set; }

        public NhibernateService()
        {
            SessionFactory = CreateSessionFactory();
        }

        public static ISessionFactory CreateSessionFactory()
        {
            bool createNew = false;
            Console.WriteLine(connectionData);

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
            if (id != null)
            {
                using (var session = OpenSession())
                    return session.QueryOver<User>().Where(n => n.ID == id).List<User>();
            }
            else
            {
                using (var session = OpenSession())
                    return session.QueryOver<User>().List<User>();
            }
        }

        public bool UpdateUser(User obj, int id)
        {
            return AddOrUpdateUser(obj, id);
        }

        public void DeleteUser(int id)
        {

            using (var session = OpenSession())
            using (var tx = session.BeginTransaction())
            {
                session.Delete("Users", id);
                session.Flush();
                tx.Commit();
            }

        }

        public bool CreateNote(Note obj)
        {
            return AddOrUpdateNote(obj);
        }

        public IEnumerable<Note> ReadNote(int? id = null)
        {
            if (id != null)
            {
                using (var session = OpenSession())
                    return session.QueryOver<Note>().Where(n => n.ID == id).List<Note>();
            }
            else
            {
                using (var session = OpenSession())
                    return session.QueryOver<Note>().List<Note>();
            }
        }

        public bool UpdateNote(Note obj, int id)
        {
            return AddOrUpdateNote(obj, id);
        }

        public void DeleteNote(int id)
        {
            using (var session = OpenSession())
            using (var tx = session.BeginTransaction())
            {
                session.Delete("Notes", id);
                session.Flush();
                tx.Commit();
            }
        }

        #region privateMethods
        private bool AddOrUpdateUser(User user, int id = -1)
        {
            bool wasSucceeded = true;
            using (var session = OpenSession())
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
                        session.SaveOrUpdate("Users", user, id);
                    }
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
        private bool AddOrUpdateNote(Note note, int id = -1)
        {

            var isUpdated = id > 0 ? true : false;
            bool wasSucceeded = true; ;
            using (var session = OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    if (isUpdated)
                        session.SaveOrUpdate("Notes", note, id);
                    else
                        session.SaveOrUpdate(note);

                    session.Flush();
                    tx.Commit();
                }
                catch (NHibernate.HibernateException)
                {
                    wasSucceeded = false;
                    tx.Rollback();
                    throw;
                }
            }
            return wasSucceeded;
        }

        #endregion
    }
}
