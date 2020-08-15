using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.Models.ViewModels
{
    public class ProductManiViewModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public string ButtonName { get; set; }
    }
}