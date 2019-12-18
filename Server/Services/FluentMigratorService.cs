using FluentMigrator.Runner;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System;
using YourNote.Server.Migrations;

namespace YourNote.Server.Services
{
    public class FluentMigratorService
    {
        #region Connection handler

        public static ISessionFactory SessionFactory { get; set; }
        private static string[] connectionData = GetConnectionData();
        private static string Host = connectionData[0];
        private static string Port = connectionData[1];
        private static string DBname = connectionData[2];
        private static string User = connectionData[3];
        private static string Password = connectionData[4];

        private static readonly string connectionString = $" Host={Host}; Port={Port}; Database={DBname};" +
                $" Username={User}; Password={Password};";

        private static string[] GetConnectionData() => Environment.GetEnvironmentVariable("PGPASSDATA").Split(':');

        #endregion Connection handler

        private static long? Version { get; set; }

        public FluentMigratorService()
        {
            Version = null;
            SessionFactory = CreateSession();
        }

        public bool MigrateUp(long? version)
        {
            Version = version;
            var serviceProvider = CreateServices(connectionString);

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabaseUp(scope.ServiceProvider);
            }

            return true;
        }

        public bool MigrateDown(long? version)
        {
            bool exist = false;
            using (var session = SessionFactory.OpenSession())
            {
                var list = OpenSession().CreateSQLQuery("SELECT * FROM public.\"VersionInfo\"").List<Object[]>();

                foreach (var item in list)
                {
                    for (var i = 0; i < 1; i++)
                    {
                        var type1 = (long?)item[i];
                        var type2 = version;
                        if (type1.Value == type2.Value)
                            exist = true;
                    }
                }
            }

            if (exist)
            {
                Version = version;
                var serviceProvider = CreateServices(connectionString);

                using (var scope = serviceProvider.CreateScope())
                {
                    UpdateDatabaseDown(scope.ServiceProvider);
                }
            }

            return true;
        }

        private static void UpdateDatabaseDown(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            if (Version.HasValue)
            {
                runner.MigrateDown(Version.Value);
            }
            else
            {
                runner.MigrateUp();
            }
        }

        private static void UpdateDatabaseUp(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            if (Version.HasValue)
            {
                runner.MigrateUp(Version.Value);
            }
            else
                runner.MigrateUp();
        }

        private static IServiceProvider CreateServices(string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(StartingVersion).Assembly, typeof(EditSomeTable).Assembly, typeof(AddSomeTable).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(true);
        }

        public static ISessionFactory CreateSession()
        {
            Console.WriteLine(connectionData);

            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL81
                .ConnectionString($" Host={Host}; Port={Port}; Database={DBname};" +
                $" Username={User}; Password={Password};"))
                .BuildSessionFactory();
        }

        public ISession OpenSession() => SessionFactory.OpenSession();
    }
}