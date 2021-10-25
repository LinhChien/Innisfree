using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Innisfree_Shop.Areas.Admin.Controllers
{
    public class AdminProductCategoryController : Controller
    {
        // GET: Admin/AdminProductCategory
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
    }
}