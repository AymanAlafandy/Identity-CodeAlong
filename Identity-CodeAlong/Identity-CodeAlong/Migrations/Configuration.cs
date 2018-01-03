namespace Identity_CodeAlong.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Identity_CodeAlong.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Identity_CodeAlong.Models.ApplicationDbContext context)
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

            context.Roles.AddOrUpdate(x => x.Name, new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Admin"));
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            ApplicationUser user = new ApplicationUser() { UserName="admin@gymbooking.se",
                Email = "admin@gymbooking.se",
                FirstName ="Admin",
                LastName ="AdminAdmin",
                TimeOfRegistration = DateTime.Now };

            //user manager will set the password for the admin

            var result = userManager.Create(user, "password");

            ApplicationUser admin = userManager.FindByName("admin@gymbooking.se");

            userManager.AddToRole(admin.Id, "Admin");

            context.SaveChanges(); 
        }
    }
}
