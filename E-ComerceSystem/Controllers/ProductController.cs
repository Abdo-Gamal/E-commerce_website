using E_ComerceSystem.Models;
using E_ComerceSystem.ModelView;
using ITIMVCProjectV1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_ComerceSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        DB Context;
        public ProductController()
        {
            Context = new DB();
        }
        // GET: Product
        public ActionResult Index()
        {
            List<Product> ProductsData = Context.Products.ToList();
            // show type
            var ProductVM = new List<ProductModelView>();
            foreach (var item in ProductsData)
            {
                ProductVM.Add(new ProductModelView
                {
                    Image = item.Image,
                    Name = item.Name,
                    price = item.price,
                    ID = item.ID,
                    
                });
               // you can make view model
                ViewData[$"{item.ID}Type"] = Context.Categories.Where(c => c.ID == item.Category_id).FirstOrDefault().Type;
            }
            return View(ProductVM);
        }

        public ActionResult Add()
        {
            List<Category> CategorysData = Context.Categories.ToList();
            ViewBag.CategorysData = CategorysData;
            return View();
        }
        [HttpPost]
        public ActionResult Add(ProductModelView data)
        {
            List<Category> CategorysData = Context.Categories.ToList();
            ViewBag.CategorysData = CategorysData;
            if (!ModelState.IsValid)
            {
                return View(data);
            }
            try
            {
                data.Image = SaveImageFile(data.ImageFile);

                Product NewPoduct = new Product
                {

                    Cost = data.Cost,
                    Description = data.Description,
                    Image = data.Image,
                    Name = data.Name,
                    price = data.price,
                    Amount = data.TotleAmount,
                    TotleAmount = data.TotleAmount,
                    ProviderName = data.ProviderName,
                    Category_id = data.Category_id,
                };
                Context.Products.Add(NewPoduct);
                Context.SaveChanges();
                return RedirectToAction("Index", "Product");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            List<Category> CategorysData = Context.Categories.ToList();
            ViewBag.CategorysData = CategorysData;

            var date = Context.Products.Where(p => p.ID == id).FirstOrDefault();
            ProductModelView dataView = new ProductModelView()
            {
                ID = date.ID,
                Name = date.Name,
                Cost = date.Cost,
                price = date.price,
                Image = date.Image,
                TotleAmount = date.TotleAmount,
                ProviderName = date.ProviderName,
                Category_id = date.Category_id,
                Description=date.Description,
            };
            
            return View(dataView);

        }
        [HttpPost]
        public ActionResult Edit(ProductModelView data)
        {
            if (!ModelState.IsValid)
                return View(data);
           
            try
            {
                //if (data.ImageFile == null || data.ImageFile.ContentLength == 0)

                //    return View(new { id = data.ID });
              

                var Product = Context.Products.Where(p => p.ID == data.ID).SingleOrDefault();
                int NewAmount = data.TotleAmount;
                int diff = NewAmount - Product.Amount;

                Product.Image = SaveImageFile(data.ImageFile, Product.Image);

                Product.Name = data.Name;
                Product.Cost = data.Cost;
                Product.price = data.price;
                Product.Amount += diff;
                Product.TotleAmount += diff;
                Product.ProviderName = data.ProviderName;
                Product.Category_id = data.Category_id;


                Context.SaveChanges();

                return RedirectToAction("Index", "product",new { id = data.ID });
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View(data);
            }

        }

      
        public ActionResult Details(int id)
        {
          
                Product item = Context.Products.Where(a => a.ID == id).FirstOrDefault();

                ProductModelView viewData = new ProductModelView()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Cost = item.Cost,
                    Amount = item.Amount,
                    TotleAmount = item.TotleAmount,
                    ProviderName = item.ProviderName,
                    Description = item.Description,
                   
                };

                return View(viewData);
            
        
            
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
          
            var date = Context.Products.Where(p => p.ID == id).SingleOrDefault();
            ProductModelView ProductView = new ProductModelView()
            {
                ID = date.ID,
                Name = date.Name,
                Cost = date.Cost,
                price = date.price,
                ProviderName = date.ProviderName,
                Image = date.Image,
                Description = date.Description,
                Amount = date.Amount,
                TotleAmount=date.TotleAmount,
            };
            return View(ProductView);
        }
        [HttpPost]
        public ActionResult DeleteConfig(int id)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Delete",new { id=id});
            
            try {
                var DataDb = Context.Products.Where(p => p.ID == id).SingleOrDefault();
                Context.Products.Remove(DataDb);
                Context.SaveChanges();

                return RedirectToAction("index", "Product");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Delete", new { id = id });
            }
         }
    }

}
