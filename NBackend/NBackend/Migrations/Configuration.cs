namespace NBackend.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NBackend.Models.NBackendContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "NBackend.Models.NBackendContext";
        }

        protected override void Seed(NBackend.Models.NBackendContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Users.AddOrUpdate(
              p => p.user_name,
              new Models.User {
                  Id = 1651162,
                  user_name = "航程施",
                  department = "软件学院",
                  password = "123456",
                  phone_number = "13112525575",
                  mail = "144@11.com",
                  avatar = "http://p3d12u2wq.bkt.clouddn.com/FvvbTTdt98dOXonlvpdBBg8GdHDr",
                  role = "student"
              },
              new Models.User
              {
                  Id = 1651163,
                  user_name = "施程航",
                  department = "软件学院",
                  password = "123456",
                  phone_number = "13112525575",
                  mail = "144@11.com",
                  avatar = "http://p3d12u2wq.bkt.clouddn.com/FvvbTTdt98dOXonlvpdBBg8GdHDr",
                  role = "student"
              }
            );

        }
    }
}
