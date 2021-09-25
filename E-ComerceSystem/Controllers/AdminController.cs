using E_ComerceSystem.Models;
using E_ComerceSystem.ModelView;
using ITIMVCProjectV1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_comarce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        DB context ;

       public AdminController()
        {
            context = new DB();
        }
        // GET: Admin
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Orders()
        {
           
            var data = context.Orders.Where(o => o.IsConfirmed == true).ToList();

            return View(data);
        }
        public ActionResult OrderDetails(int id)// order id
        {
           
            var order = context.Orders.Where(o => o.ID == id).SingleOrDefault();
            ViewBag.CustomerName = context.Users.Where(c => c.Id == order.Customer_id).SingleOrDefault().UserName;
            var data = context.SubOrders.Where(s => s.Order_id == order.ID).ToList();
            List<AdminOrderDetailsModelView> viewData = new List<AdminOrderDetailsModelView>();
            foreach (var item in data)
            {
                viewData.Add(new AdminOrderDetailsModelView
                {
                    Name = item.Product.Name,
                    Amount = item.Amount,
                    ProviderName = item.Product.ProviderName,
                    price = item.Product.price,
                }
                );

            }

            return View(viewData);
        }
    }
}