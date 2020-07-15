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
    public class ChitietController : Controller
    {
        ASM_BookEntities db = new ASM_BookEntities();
        // GET: Chitiet
        public ActionResult ChitietBook(int? BookID)
        {     
            //  Book s = db.Books.Single(n => n.BookId == id);
            Book b = (from p in db.Books where p.BookId == BookID select p).ToArray()[0];
           
            List<Comment> DG = db.Comments.Where(n => n.BookId == BookID).OrderByDescending(n => n.CommentId).ToList();
            ViewData["listDG"] = DG;
          
            return View(b);
        }
        [HttpPost]
        public ActionResult ChiTietBook(int iMaSach, string iName, string iComment, string Ngay, Comment DG)
        {

           

            DG.BookId = iMaSach;
            DG.HoTenKH = iName;
            DG.Content = iComment;
           
            DG.CreatedDate = DateTime.Parse(Ngay);
            TempData["MaSach"] = iMaSach;
            db.Comments.Add(DG);
            db.SaveChanges();
            List<Comment> LDG = db.Comments.Where(n => n.BookId == iMaSach).ToList();
          
       
            return RedirectToAction("BookSpecialParialView", "Home");


        }
       
        public ActionResult ChudeBook(int? CateId)
        {
           

            ViewBag.name = db.Categories.SingleOrDefault(n => n.CateId == CateId).CateName;
            return View(db.Books.Where(n=>n.CateId== CateId).ToList());
        }
    }
}