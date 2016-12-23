namespace K53Forum.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<K53Forum.Models.DbK53Forum>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(K53Forum.Models.DbK53Forum context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //  AddOrUpdate  default Roles
            context.Roles.AddOrUpdate(
              new Models.Role { Id = 1, RoleName = "Admin" },
              new Models.Role { Id = 2, RoleName = "Member" }
            );
            //  AddOrUpdate  default Category
            context.Categories.AddOrUpdate(
              new Models.Category { Id = 1, Name = "Học tập" },
              new Models.Category { Id = 2, Name = "Giải trí" }
            );
        }
    }
}
