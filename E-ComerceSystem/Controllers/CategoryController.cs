using E_ComerceSystem.Models;
using E_ComerceSystem.ModelView;
using ITIMVCProjectV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_ComerceSystem.Controllers
{

  [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        DB Context;
       public CategoryController()
        {
            Context = new DB();
        }
        // GET: Category
        public ActionResult Index()
        {
            List<Category> CategorysData= Context.Categories.ToList();
            var CategorydataVM = new List<CategoryModelView>();
            foreach (var item in CategorysData)
            {
                CategorydataVM.Add(new CategoryModelView
                {
                    Type = item.Type,
                    ID = item.ID,
                }); 
            }
            return View(CategorydataVM);
        }
        // for remote type 
        
       public ActionResult CheackType(string Type,int ID=0)
       {
            var cat = Context.Categories.FirstOrDefault(c=>c.Type== Type&&c.ID!=ID);
            if (cat != null)
                return Json( false,JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
           
        }

        public ActionResult Add()
        {
          
            return View();
        }
        [HttpPost]
       
        public ActionResult Add(Category categoery)
        {
            if (!ModelState.IsValid)
                return View(categoery);
            try
            {
                Context.Categories.Add(
                    new Category { Type = categoery.Type.Trim() }
                    );
                Context.SaveChanges();
                return RedirectToAction("index", "Category");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(categoery);
            }
        }


        public ActionResult Edit(int id)
        {
          
            var dataDb = Context.Categories.Where(c => c.ID == id).
                SingleOrDefault();
            if (dataDb == null)
            {
                return RedirectToAction("index", "category");

            }
            CategoryModelView data = new CategoryModelView
            {
                Type = dataDb.Type,
                ID = dataDb.ID
            };
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(CategoryModelView categoery)
        {
            if (!ModelState.IsValid)
            
                return View(categoery);
            try
            {
                var data = Context.Categories.Where(c => c.ID == categoery.ID).SingleOrDefault();
                data.Type = categoery.Type.Trim();
                Context.SaveChanges();
                return RedirectToAction("index", "Category");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(categoery);
            }
        }

       
        public ActionResult Delete(int id)
        {
            Category dataDb = Context.Categories.Where(c => c.ID == id).SingleOrDefault();
            if (dataDb == null)
            {
                return RedirectToAction("index", "category");

            }
            var data = new CategoryModelView
            {
               Type  = dataDb.Type,
                ID = dataDb.ID,
            };
            return View(data);
        }
        [HttpPost]
        public ActionResult Delete(CategoryModelView categoery)
        {

            if (!ModelState.IsValid)
            {
                return View(categoery);
            }
            try
            { 
                var data = Context.Categories.Where(c => c.ID == categoery.ID).SingleOrDefault();
                Context.Categories.Remove(data);
                Context.SaveChanges();
                return RedirectToAction("index", "Category");
            }catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(categoery);
            }
        }

    }
}