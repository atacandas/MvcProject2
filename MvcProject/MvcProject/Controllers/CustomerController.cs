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
    public class CustomerController : Controller
    {
        public ActionResult ListMyProducts()
        {
            List<Product> products;

            if (HttpContext.Cache["listProducts"] == null)
            {
                using (NorthwindEntities db = new NorthwindEntities())
                {
                    products = db.Products.Include("Supplier").Include("Category").ToList();
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
    }
}