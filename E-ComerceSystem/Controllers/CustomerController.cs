using E_ComerceSystem.Models;
using E_ComerceSystem.ModelView;
using ITIMVCProjectV1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_ComerceSystem.Controllers
{
    /// <summary>
    /// to get user id
    /// </summary>
    //UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(Context);
    //UserManager<ApplicationUser> manager =
    //    new UserManager<ApplicationUser>(store);
    //ApplicationUser item = manager.FindById(User.Identity.GetUserId());
    [Authorize]
    public class CustomerController : Controller
    {
        DB Context;
        public CustomerController()
        {
            Context = new DB();
            //if(User.Identity.IsAuthenticated==true)

        }
        // GET: Customer
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<ApplicationUser> ProductsData = Context.Users.ToList();
            // show type
            var ApplicationUser = new List<ApplicationUserModelView>();
            foreach (var item in ProductsData)
            {
                 ApplicationUser.Add(new ApplicationUserModelView
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    Image = item.Image,
                    PhoneNumber=item.PhoneNumber,
                });
            }
            return View(ApplicationUser);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {

            ApplicationUser item = Context.Users.Where(p => p.Id == id).SingleOrDefault();
            ApplicationUserModelView DelUser= new ApplicationUserModelView()
            {
                Id = item.Id,
                UserName = item.UserName,
                Email = item.Email,
                Image = item.Image,
                PhoneNumber = item.PhoneNumber,
            };
            return View(DelUser);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfig(string id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Delete", new { id = id });

            try
            {
                var DataDb = Context.Users.Where(p => p.Id == id).SingleOrDefault();
                Context.Users.Remove(DataDb);
                Context.SaveChanges();

                return RedirectToAction("Index", "Customer");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Delete", new { id = id });
            }
        }
        //for remote Attribute
        //public ActionResult CheackEmail(string Email, string Id = "0")
        //{
        //    var cat = Context.Users.FirstOrDefault(c => c.UserName == Email && c.Id != Id);
        //    if (cat != null)
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    else
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// user Authorize section
        /// </summary>
        /// 
        /// <returns></returns>

        public  new ActionResult Profile()
        {
            
            try
            {
                UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(Context);
                UserManager<ApplicationUser> manager =
                    new UserManager<ApplicationUser>(store);
                ApplicationUser item = manager.FindById(User.Identity.GetUserId());

                //var UserName = Session["UserName"].ToString();
                //ApplicationUser item = Context.Users.Where(e => e.UserName == UserName).FirstOrDefault();
                // show typ
                var ApplicationUser = new ApplicationUserModelView
                {

                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    Image = item.Image,
                    PhoneNumber = item.PhoneNumber,
                };
                return View(ApplicationUser);
            }
            catch ( Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
         
        }

        public ActionResult Edit(string id)
        {
            var data = Context.Users.Where(c => c.Id== id).FirstOrDefault();
            ApplicationUserModelView edit = new ApplicationUserModelView()
            {
                Id = data.Id,
                Email = data.UserName,
                UserName = data.Email,
                Password = data.PasswordHash,
                PhoneNumber = data.PhoneNumber
            };
            
                return View(edit);

        }
        [HttpPost]
        public ActionResult Edit(ApplicationUserModelView cust)
        {
            if (!ModelState.IsValid)
                return View(cust);
            try
            {
                var data = Context.Users.Where(c => c.Id == cust.Id).FirstOrDefault();
                if (data != null)
                {
                    data.Id = cust.Id;
                    data.Email = cust.UserName;
                    data.UserName = cust.Email;
                    data.PasswordHash = cust.Password;
                    data.PhoneNumber = cust.PhoneNumber;
                }
                Context.SaveChanges();
                return RedirectToAction("Profile");


            }
            catch (Exception ex) {
                ModelState.AddModelError("", ex.Message);
                return View(cust);
            }
        }


        public ActionResult orders()// show if  you have any order
        {
          

            string id= User.Identity.GetUserId(); //usual one order
            Order car = Context.Orders.Where(o => o.Customer_id ==id&& o.IsConfirmed == false).FirstOrDefault();
            if (car != null)
            {
                var allItemInCar = Context.SubOrders.Where(s => s.Order_id == car.ID).ToList();

                List<BillModelView> bill = new List<BillModelView>();

                foreach (var item in allItemInCar)
                {
                    bill.Add(new BillModelView
                    {
                        SuborderId = item.ID,
                        ProductName = item.Product.Name,
                        ProductAmount = item.Amount,
                        Cost = item.Amount * item.Product.price,
                        ProviderName = item.Product.ProviderName
                    });
                }

                // show  bill info
                double sum = bill.Sum(b => b.Cost);
                ViewBag.totalCost = sum;
                ViewBag.orderId = car.ID;
                ViewBag.ReservationDate = DateTime.Now;
                ViewBag.deleveryDate = DateTime.Now.AddDays(3);
                ViewBag.Cost = sum;
                return View(bill);
            }
            return Content("you have no orders");//make  new view
        }
    
        public ActionResult Confirm(CustomerConfirmModelView data)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("orders", "customer");

            }
            try
            {
                Order order = Context.Orders.
                    Where(o => o.ID == data.Order_id).SingleOrDefault();
                order.IsConfirmed = true;
                order.Cost = data.Cost;
                order.DeliveryDate = data.DelveryDate;
                order.destination = data.Destination;
                order.destination = data.Destination;
                Context.SaveChanges();
                List<SubOrder> subordersAndProduct = Context.SubOrders.Where(s => s.Order_id == data.Order_id).ToList();
                foreach (var item in subordersAndProduct)
                {
                    item.Product.Amount -= item.Amount;
                }
                Context.SaveChanges();

                return RedirectToAction("Profile", "Customer");
            }catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("orders", "customer");
            }
        }
        /// <summary>
        /// 
        /// </summary>
       
        /// <returns></returns>
       
        public ActionResult CancelItemFromCar(int id)
        {

            SubOrder SubOrderitem = Context.SubOrders.Where(p => p.ID == id).SingleOrDefault();

            ViewBag.OrderName= SubOrderitem.Product.Name;
            ViewBag.OrderProviderName = SubOrderitem.Product.ProviderName;
            return View(SubOrderitem);
        }
        [HttpPost]
       
        public ActionResult CancelItemFromCarConfig(int id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("CancelItemFromCar", new { id = id });

            try
            {
                var DataDb = Context.SubOrders.Where(p => p.ID == id).SingleOrDefault();
                Context.SubOrders.Remove(DataDb);
                Context.SaveChanges();

                return RedirectToAction("orders", "Customer");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("CancelItemFromCar", new { id = id });
            }
        }

    }

}