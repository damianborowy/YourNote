
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourNote.Shared.Models;

namespace YourNote.Server.Services
{
    public class FluentMigratorService
    {
        #region Connection handler

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



        private static void MigrateTo(long? version)
        {

            Version = version;
            var serviceProvider = CreateServices(connectionString);

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
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

    }
}
