namespace Dapper.数据模型.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Dapper.数据模型.数据库上下文>
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = false;

            AutomaticMigrationsEnabled = true; // 任何Model Class的修改將會直接更新DB
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Dapper.数据模型.数据库上下文 context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
