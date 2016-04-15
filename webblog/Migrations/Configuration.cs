namespace webblog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<webblog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //Admin Account Details
            if (!context.Users.Any(u => u.Email == "danlor6808@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "danlor6808@gmail.com",
                    Email = "danlor6808@gmail.com",
                    FirstName = "Danny",
                    LastName = "Lorn",
                    DisplayName = "danlor6808"
                }, "Password1");
            }
     
            var userId_Admin = userManager.FindByEmail("danlor6808@gmail.com").Id;
            userManager.AddToRole(userId_Admin, "Admin");

            //Moderator Account Details
            if (!context.Users.Any(u => u.Email == "riamang@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "riamang@coderfoundry.com",
                    Email = "riamang@coderfoundry.com",
                    FirstName = "Ria",
                    LastName = "",
                    DisplayName = "riamang"
                }, "Password1");
            }
  
            var userId_Mod = userManager.FindByEmail("riamang@coderfoundry.com").Id;
            userManager.AddToRole(userId_Mod, "Moderator");

            //Moderator Account Details
            if (!context.Users.Any(u => u.Email == "anivra@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "anivra@coderfoundry.com",
                    Email = "anivra@coderfoundry.com",
                    FirstName = "Antonio",
                    LastName = "",
                    DisplayName = "anivra"
                }, "Password1");
            }

            var userId_Mod2 = userManager.FindByEmail("anivra@coderfoundry.com").Id;
            userManager.AddToRole(userId_Mod2, "Moderator");

            //Moderator Account Details
            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "moderator@coderfoundry.com",
                    Email = "moderator@coderfoundry.com",
                    FirstName = "Coder",
                    LastName = "Foundry",
                    DisplayName = "Coderfoundry"
                }, "Password1");
            }

            var userId_Mod3 = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(userId_Mod3, "Moderator");
        }
    }
}
