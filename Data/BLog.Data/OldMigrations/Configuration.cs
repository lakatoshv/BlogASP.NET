using System.Data.Entity.Migrations;

namespace BLog.Data.OldMigrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BlogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
