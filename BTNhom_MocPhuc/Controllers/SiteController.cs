using BTNhom_MocPhuc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTNhom_MocPhuc.Controllers
{
    public class SiteController : Controller
    {

        // GET: Site
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GioiThieu()
        {
            return View();
        }


        public ActionResult LienHe()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult headerCart()
        {
            var cart = Session["CartSession"];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
    }
}