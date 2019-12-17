
using FluentMigrator.Runner;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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

        #endregion

        private static long? Version { get; set; }

        public FluentMigratorService()
        {

            Version = null;

           
        }



        public bool MigrateTo(long? version)
        {



            SessionFactory = CreateSession();
            bool exist = false;
            using (var session = SessionFactory.OpenSession())
            {

               exist  = session.Query<VersionInfo>()
                               .Any(x => x.Version.Equals(version.ToString()));
               
            }
            
            
            if(exist)
            {
                Version = version;
                var serviceProvider = CreateServices(connectionString);

                using (var scope = serviceProvider.CreateScope())
                {
                    UpdateDatabase(scope.ServiceProvider);
                }
            }

            return exist;

        }

        private static IServiceProvider CreateServices(string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Program).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(true);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
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

    internal class VersionInfo
    {

        public string Version { get; set; }
        public DateTime AppiledOn { get; set; }
        public string Description { get; set; }

    }
}
