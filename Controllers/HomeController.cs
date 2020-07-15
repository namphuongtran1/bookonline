using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using BookManage.Models;

namespace BookManage.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        ASM_BookEntities db = new ASM_BookEntities();
        public ActionResult Home()
        {
            
           
            return View(db.Books.Take(24).ToList());
        }
        public PartialViewResult BookSpecialParialView()
        {
           
            return PartialView(db.Books.Where(n=> n.CateId==9).ToList());

        }

    }
}