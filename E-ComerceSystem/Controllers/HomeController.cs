using E_ComerceSystem.Models;
using E_ComerceSystem.ModelView;

using ITIMVCProjectV1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace E_ComerceSystem.Controllers
{
    public class HomeController : Controller
    {

        DB Context;
        public HomeController()
        {
            Context = new DB();
        
           
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            var products = Context.Products.Where(p => p.Amount > 0).ToList();
            List<ProductModelView> data = new List<ProductModelView>();
            foreach (var item in products)
            {

                ProductModelView cur = new ProductModelView
                {
                    ID = item.ID,
                    Name = item.Name,
                    price = item.price,
                    Image = item.Image,
                    ProviderName = item.ProviderName,
                    CategoryType = item.Category.Type,
                    //Description = item.Description,
                   
                };
                data.Add(cur);
             }
                ViewBag.category = Context.Categories.ToList();
            return View(data);
        }
        public ActionResult GetCategory(int id)
        {
            
            var products = Context.Products.Where(p => p.Category_id == id).ToList();
            if (products != null && products.Count>0)
            {
                List<ProductModelView> data = new List<ProductModelView>();
                foreach (var item in products)
                {
                    data.Add(new ProductModelView
                    {
                        ID = item.ID,
                        Name = item.Name,
                        price = item.price,
                        Image = Path.GetFileName(item.Image),
                        ProviderName = item.ProviderName,
                        CategoryType = item.Category.Type

                    });
                }
                ViewBag.CategoryName = products[0].Category.Type;
                ViewBag.category = Context.Categories.ToList();

                return View(data);
            }
            return Content("make new view to tell no item in this category");
        }
        public ActionResult Details(int id)
        {
            Product data = Context.Products.Where(p => p.ID == id).FirstOrDefault();
            List<Feedback> feed = Context.Feadbacks.Where(f => f.Product.ID == id).ToList();

            ViewBag.ProductId = data.ID;
            ViewBag.Name = data.Name;
            ViewBag.Image =data.Image;
            ViewBag.price = data.price;
            ViewBag.ProviderName = data.ProviderName;
            ViewBag.Discraption = data.Description;
            List<FeedbackModelView> Feadback = new List<FeedbackModelView>();
            foreach (var item in feed)
            {
                Feadback.Add(new FeedbackModelView
                {
                    Comment = item.Comment,
                    CustomerName = item.Customer.Email,
                    Rate = item.Rate,
                });
            }

            return View(Feadback);
        }

        [HttpGet]
        public ActionResult AddComment(int id) // id == product id
        {

            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(Context);
            UserManager<ApplicationUser> manager =
                new UserManager<ApplicationUser>(store);
            ApplicationUser item = manager.FindById(User.Identity.GetUserId());

            ViewBag.CustomerId = Context.Users.
                Where(c => c.UserName.Trim() == item.UserName.Trim()).
                SingleOrDefault().Id;

            ViewBag.ProductId = id;
            return View();
        }
        [HttpPost]
        public ActionResult AddComment(FeedbackModelView data)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddComment", "Home", new { id=data.ProductId });
            }
            try
            {
                data.Rate = (data.Rate <= 0) ? 1 : data.Rate;
                Context.Feadbacks.Add(new Feedback
                {
                    Product_ID = data.ProductId,
                    Customer_ID = data.CustomerId,
                    Comment = data.Comment,
                    Rate = (data.Rate >= 10) ? 10 : data.Rate

                });
                Context.SaveChanges();
                return RedirectToAction("Details", "Home", new { id = data.ProductId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return RedirectToAction("AddComment", "Home", new { id = data.ProductId });
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(ContactModelView vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(vm.Email);//Email which you are getting 
                                                         //from contact us page 
                    msz.To.Add("bgmal917@gmail.com");//Where mail will be sent 
                    msz.Subject = vm.Subject;
                    msz.Body = vm.Message;
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";

                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
                    ("bgmal917@gmail.com", "body6123A");

                    smtp.EnableSsl = true;

                    smtp.Send(msz);

                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }
            }

            return View();
        }
        /// <summary>
        /// table order normilizeed and beccome suborder and order 
        /// </summary>
      
        [HttpGet]
        public ActionResult Bay(int id) // product Id // add to card
        {
            ViewBag.ProductId = id;
            return View();
        }
        /// <summary>
        /// we  will add item in car by 
        /// add only uer id in order table and amount of item, orderid in suborder table
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Bay(CustomerAddToCardModelView data)// CustomerAddToCardModelView productid,amount
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("index", "Home");
            }
            try
            {
                //to  get userobject but not use

                //UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(Context);
                //UserManager<ApplicationUser> manager =
                //    new UserManager<ApplicationUser>(store);
                //ApplicationUser item = manager.FindById(User.Identity.GetUserId());
                //string CustomerId = Context.Users.Where(c => c.UserName.Trim() == UserNmae.Trim()).SingleOrDefault().Id;
                string UserID=User.Identity.GetUserId();
                var x = DateTime.Now.Date;
                if (Context.Orders.Where(o => o.Customer_id == UserID && o.IsConfirmed == false).Count() == 0)
                {

                    Context.Orders.Add( // add user in ont found
                        new Order  //just add  uer id and amount and this is bill
                        {
                            Customer_id = UserID
                        }
                        );
                    Context.SaveChanges();
                }

                int OrderId = Context.Orders.Where(o => o.Customer_id == UserID &&
                o.IsConfirmed == false).SingleOrDefault().ID;
                Context.SubOrders.Add(new SubOrder
                {
                    Order_id = OrderId,
                    Product_id = data.ProductId,
                    Amount = data.Amount,
                   
                });
                Context.SaveChanges();
                return RedirectToAction("index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return RedirectToAction("Bay","Home",new { id = data.ProductId });
            }
        }

    }
}