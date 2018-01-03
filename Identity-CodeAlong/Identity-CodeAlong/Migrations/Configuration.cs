namespace Identity_CodeAlong.Migrations
{
    using Identity_CodeAlong.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
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

            //* Proper way of doing it *//
             /* RoleStore<IdentityRole> roleStore = new RoleStore<IdentityRole>(context);
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(roleStore);

            string[] roleNames = new[] { "Admin", "Member" };
            foreach (string roleName in roleNames)
            {
                if (!context.Roles.Any(r => r.Name == roleName))
                {
                    IdentityRole role = new IdentityRole { Name = roleName };
                    IdentityResult result = roleManager.Create(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }
            }*/

            //* Quick and dirty way of doing it *//
            context.Roles.AddOrUpdate(x=> x.Name, new IdentityRole("Admin"));

            UserStore<ApplicationUser> userStore = new 
                UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = 
                new UserManager<ApplicationUser>(userStore);

            ApplicationUser user = new ApplicationUser()
            { UserName="admin@GymBooking.se",
                Email ="admin@GymBooking.se",
                FirstName ="Admin",
                LastName ="Adminsson",
                TimeOfRegistration = DateTime.Now};

            var result = userManager.Create(user, "password");

            ApplicationUser admin = 
                userManager.FindByName("admin@GymBooking.se");
            userManager.AddToRole(admin.Id, "Admin");
            context.SaveChanges();
        }
    }
}
