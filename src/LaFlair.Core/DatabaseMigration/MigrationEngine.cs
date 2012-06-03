using System;
using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;

namespace LaFlair.Core.DatabaseMigration
{
    public static class MigrationEngine
    {
        public static void Update(MigrationProcessorType type, string connectionString, Assembly migrationAssembly)
        {
            Update(new NullAnnouncer(), type, connectionString, migrationAssembly);
        }

        public static void Update(IAnnouncer announcer, MigrationProcessorType type, string connectionString, Assembly migrationAssembly)
        {
            var context = new RunnerContext(announcer);
            var options = new ProcessorOptions();

            var factory = GetProcessorFactory(type);
            var processor = factory.Create(connectionString, announcer, options);

            var runner = new MigrationRunner(migrationAssembly, context, processor);

            runner.MigrateUp();
        }

        private static IMigrationProcessorFactory GetProcessorFactory(MigrationProcessorType type)
        {
            switch (type)
            {
                case MigrationProcessorType.SQLCE:
                    return new SqlServerCeProcessorFactory();
                case MigrationProcessorType.SQL2005:
                    return new SqlServer2005ProcessorFactory();
                case MigrationProcessorType.SQL2008:
                    return new SqlServer2008ProcessorFactory();
                case MigrationProcessorType.SQL2012:
                    throw new NotSupportedException("Not Yet Supported");
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}
