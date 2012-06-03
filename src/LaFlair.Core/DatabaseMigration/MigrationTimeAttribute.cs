using FluentMigrator;

namespace LaFlair.Core.DatabaseMigration
{
    public class MigrationTimeAttribute : MigrationAttribute
    {
        public MigrationTimeAttribute(int year, int month, int day, int hour, int minute, int seconds)
            : base(long.Parse(string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}", year, month, day, hour, minute, seconds)))
        {
        }
    }
}
