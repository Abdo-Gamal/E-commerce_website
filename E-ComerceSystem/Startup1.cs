using E_ComerceSystem.Models;
using ITIMVCProjectV1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(E_comarce.Startup1))]

namespace E_comarce
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {

            createRolesandUsersAsync();
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //Name
                ExpireTimeSpan = TimeSpan.FromDays(7),
                //Life Tim
                LoginPath = new PathString(@"/Account/Login")
                //if  no authzith go to login
            });
            // app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions { AuthenticationType = });
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }


        private async Task createRolesandUsersAsync()
        {

            DB context = new DB();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var Store = new UserStore<ApplicationUser>(context);
            var UserManager = new UserManager<ApplicationUser>(Store);
            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
               var AdminRole=roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   

                ApplicationUser user = new ApplicationUser();
                user.UserName = "bgmal917@gmail.com";
                user.Email = "AbdoGamal";
                string userPWD = "body6123";

                var chkUser = await UserManager.CreateAsync(user, userPWD);
                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }

            }
        }
    }
           
       
    
}
