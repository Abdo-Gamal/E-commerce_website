using E_comarce;
using E_ComerceSystem.Models;
using E_ComerceSystem.ModelView;
using ITIMVCProjectV1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace E_ComerceSystem.Controllers
{
    
    public class AccountController : Controller
    {
        DB context;

        public AccountController()
        {
            context = new DB();
        }
        // GET: Account
        
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(loginViewModel user)
        {

            if (!ModelState.IsValid)
                return View(user);

            try
            {

                UserStore<ApplicationUser> store =
                    new UserStore<ApplicationUser>
                    (context);
                UserManager<ApplicationUser> manager =
                    new UserManager<ApplicationUser>(store);

                var userName = user.Email;
                var result = manager.Find(userName, user.Password);

             
                if (result != null)
                {

                  //  MAnager SignIn " make cookes"
                    IAuthenticationManager authenticationManager =
                        HttpContext.GetOwinContext().Authentication;

                    SignInManager<ApplicationUser, string> signinmanager =
                        new SignInManager<ApplicationUser, string>
                        (manager, authenticationManager);
                    signinmanager.SignIn(result, true, true);//this line make cookes


                    if (User.IsInRole("Admin")==true)   //User  with cabtal U  // not work
                        return RedirectToAction("Index", "Admin");
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "invaild user name or passord");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(user);
            }
        }
       public ActionResult Registration()
        {
                return View();   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registration(ApplicationUserModelView userRegister)
        {
            if (!ModelState.IsValid)
                return View(userRegister);
            try
            {
              
                //Creat UserManager

                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> manager =
                    new UserManager<ApplicationUser>(store);
                //Map Vm To Model
                ApplicationUser user = new ApplicationUser();
                user.UserName = userRegister.Email;
                user.Email = userRegister.UserName;
                user.PasswordHash = userRegister.Password;
                user.PhoneNumber = userRegister.PhoneNumber;
                 user.Image = SaveImageFile(userRegister.ImageFile);
            
                ///
                //UserManager save user database
                IdentityResult resulte = await manager.CreateAsync(user, userRegister.Password);

                if (resulte.Succeeded)
                {
                   // manager.AddToRole(user.Id, "Admin"); //not use
                   
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    string listOfErrors = "";
                    foreach (string it in resulte.Errors)
                    {
                        listOfErrors += it;
                    }
                    ModelState.AddModelError("", listOfErrors);
                    return View(userRegister);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(userRegister);
            }
        }
        private string SaveImageFile(HttpPostedFileBase ImageFile, string oldImage = "")
        {
            if (ImageFile != null)
            {
                var FileExtantion = Path.GetExtension(ImageFile.FileName);
                var ImageGuid = Guid.NewGuid().ToString();
                string ImageName = ImageGuid + FileExtantion;
                //save
                string filePath = Server.MapPath($"~/Content/Images/{ImageName}");
                ImageFile.SaveAs(filePath);
                // dele old image
                if (!string.IsNullOrEmpty(oldImage))
                {
                    string oldpath = Server.MapPath($"~/Content/Images/{oldImage}");
                    System.IO.File.Delete(oldpath);
                }
                return ImageName;
            }
            return oldImage;
        }
        [Authorize]
        public ActionResult Signout()
        {
           
            IAuthenticationManager manger = HttpContext.GetOwinContext().Authentication;
            manger.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}