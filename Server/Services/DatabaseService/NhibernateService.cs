using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using YourNote.Server.Models.MappingClasses;
using YourNote.Server.Services.DatabaseService;
using YourNote.Shared.Models.MappingClasses;

namespace YourNote.Server.Services
{
    public class NhibernateService<T> : IDatabaseService<T> where T : class
    {
        #region Connection to Database

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
                    .AddFromAssemblyOf<TagMap>()
                    .AddFromAssemblyOf<LectureMap>()
                )
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                .Create(createNew, createNew))
                .BuildSessionFactory();
        }

        public ISession OpenSession() => SessionFactory.OpenSession();

        private static string[] GetConnectionData() => Environment.GetEnvironmentVariable("PGPASSDATA").Split(':');

        #endregion Connection to Database

        #region CRUD

        public T Create(T obj)
        {
            return AddRecord(obj);
        }

        public T Read(int id)
        {
            return GetById(id);
        }

        public IList<T> Read()
        {
            return GetAllRecords();
        }

        public T Update(T obj)
        {
            return UpdateRecord(obj);
        }

        public bool Delete(int id)
        {
            return DeleteRecord(id);
        }

        #endregion CRUD

        #region privateMethods

        private T AddRecord(T obj)
        {
            using (var session = OpenSession())
            {
                var tx = session.BeginTransaction();

                try
                {
                    session.Save(obj);
                    tx.Commit();
                }
                catch (NHibernate.HibernateException)
                {
                    tx.Rollback();
                    throw;
                }
            }

            return obj;
        }

        private T UpdateRecord(T obj)
        {
            using (var session = OpenSession())
            {
                var tx = session.BeginTransaction();

                try
                {
                    session.SaveOrUpdate(obj);
                    tx.Commit();
                }
                catch (NHibernate.HibernateException)
                {
                    tx.Rollback();
                    throw;
                }
            }

            return obj;
        }

        private bool DeleteRecord(int id)
        {
            bool wasSucceeded = true;

            using (var session = OpenSession())
            {
                var tx = session.BeginTransaction();
                try
                {
                    session.Delete(session.Get<T>(id));
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

        private T GetById(int id)
        {
            using (var session = OpenSession())
            {
                var result = session.Get<T>(id);
                return result;
            }
        }

        private IList<T> GetAllRecords()
        {
            using (var session = OpenSession())
            {
                var result = session.QueryOver<T>().List<T>();
                return result;
            }
        }

        #endregion privateMethods
    }
}