using MvcProject.Models;
using MvcProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace MvcProject.Controllers
{
    public class AdminController : Controller
    {
        private ProductManiViewModel GetProductManiViewModelObject(int id = 0)
        {
            List<Category> categories;
            List<Supplier> suppliers;

            if (HttpContext.Cache["listCategories"] == null)
            {
                using (NorthwindEntities db = new NorthwindEntities())
                {
                    categories = db.Categories.ToList();
                }

                HttpContext.Cache.Add("listCategories", categories, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), CacheItemPriority.Low, null);
            }
            else
            {
                categories = HttpContext.Cache["listCategories"] as List<Category>;
            }

            if (HttpContext.Cache["listSuppliers"] == null)
            {
                using (NorthwindEntities db = new NorthwindEntities())
                {
                    suppliers = db.Suppliers.ToList();
                }

                HttpContext.Cache.Add("listSuppliers", suppliers, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), CacheItemPriority.Low, null);
            }
            else
            {
                suppliers = HttpContext.Cache["listSuppliers"] as List<Supplier>;
            }

            ProductManiViewModel model = new ProductManiViewModel();

            model.Categories = categories;
            model.Suppliers = suppliers;

            if (id == 0)
            {
                model.Product = new Product();
                model.ButtonName = "Add Product";
            }
            else
            {
                List<Product> products = HttpContext.Cache["listProducts"] as List<Product>;
                model.Product = products.FirstOrDefault(x => x.ProductID == id);
                model.ButtonName = "Update Product";
            }

            return model;
        }

        [HttpGet]
        public ActionResult ListMyProducts()
        {
            List<Product> products;

            if (HttpContext.Cache["listProducts"] == null)
            {
                using (NorthwindEntities db = new NorthwindEntities())
                {
                    products = db.Products.Include("Category").Include("Supplier").ToList();
                }

                HttpContext.Cache.Add("listProducts", products, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), CacheItemPriority.Normal, null);
            }
            else
            {
                products = HttpContext.Cache["listProducts"] as List<Product>;
            }

            ListProductViewModel model = new ListProductViewModel
            {
                Products = products
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult AddMyProduct()
        {
            ProductManiViewModel model = GetProductManiViewModelObject();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddMyProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                ProductManiViewModel model = GetProductManiViewModelObject();

                return View(model);
            }

            using (NorthwindEntities db = new NorthwindEntities())
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }

            TempData["message"] = "Veri girildi";

            HttpContext.Cache.Remove("listProducts");

            return RedirectToAction("ListMyProducts");
        }

        [HttpGet]
        public ActionResult UpdateMyProduct(int id)
        {
            ProductManiViewModel model = GetProductManiViewModelObject(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateMyProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                ProductManiViewModel model = GetProductManiViewModelObject(product.ProductID);

                return View(model);
            }

            using (NorthwindEntities db=new NorthwindEntities())
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            TempData["message"] = "Veri guncellendi";

            HttpContext.Cache.Remove("listProducts");

            return RedirectToAction("ListMyProducts");
        }
    }
}