using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookManage.Models;
using System.ComponentModel;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using PagedList;
using System.Data.Entity;

namespace BookManage.Areas.Admin.Controllers
{
    
    public class AdminCRUDController : Controller
    {
       ASM_BookEntities db = new ASM_BookEntities();
        // GET: Admin/AdminCRUD
        public class HttpParamActionAttribute : ActionNameSelectorAttribute
        {
            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                    return true;

                var request = controllerContext.RequestContext.HttpContext.Request;
                return request[methodInfo.Name] != null;
            }
        }
        [HttpGet]
        public ActionResult Index(int? size, int? page, string sortProperty, string sortOrder, string searchString)
        {
            ViewBag.searchValue = searchString;
            ViewBag.sortProperty = sortProperty;
            ViewBag.page = page;

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });

            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }
            ViewBag.size = items;
            ViewBag.currentSize = size;

            var links = from l in db.Books select l;
            // 5. T?o thu?c tính s?p x?p m?c d?nh là "LinkID"
            if (String.IsNullOrEmpty(sortProperty)) sortProperty = "BookId";

            // 5. S?p x?p tang/gi?m b?ng phuong th?c OrderBy s? d?ng trong thu vi?n Dynamic LINQ
            if (sortOrder == "desc") links = links.OrderBy(sortProperty + " desc");
            else if (sortOrder == "asc") links = links.OrderBy(sortProperty);
            else links = links.OrderBy("Title");

            if (!String.IsNullOrEmpty(searchString))
            {
                links = links.Where(s => s.Title.Contains(searchString));
            }

            page = page ?? 1;


            int pageSize = (size ?? 5);

            ViewBag.pageSize = pageSize;

            // 6. Toán t? ?? trong C# mô t? n?u page khác null thì l?y giá tr? page, còn
            // n?u page = null thì l?y giá tr? 1 cho bi?n pageNumber. --- dammio.com
            int pageNumber = (page ?? 1);

            // 6.2 L?y t?ng s? record chia cho kích thu?c d? bi?t bao nhiêu trang
            int checkTotal = (int)(links.ToList().Count / pageSize) + 1;
            // N?u trang vu?t qua t?ng s? trang thì thi?t l?p là 1 ho?c t?ng s? trang
            if (pageNumber > checkTotal) pageNumber = checkTotal;

            return View(links.ToPagedList(pageNumber, pageSize));
            
        }
        [HttpPost, HttpParamAction]
        public ActionResult Reset()
        {
            ViewBag.searchValue = "";
            Index(null, null, "", "", "");
            return View();

        }
        public ActionResult Edit(int? id)
        {

            ViewBag.imgurl = db.Books.SingleOrDefault(n => n.BookId == id).ImgUrl;
            // List<Category> lis = db.Categories.ToList();

            Book sach = db.Books.Find(id);
            return View(sach);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,Title,CateId,AuthorId,PubId,Summary,ImgUrl,Price,Quantity,CreatedDate,ModifiedDate,IsActive")] Book sach)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sach).State=EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sach);
        }
        public ActionResult Delete(int? id)
        {
            Book sach = db.Books.Find(id);
            return View(sach);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book sach = db.Books.Find(id);
            db.Books.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ChitietBookAdmin(int? BookId)
        {
            //  Book s = db.Books.Single(n => n.BookId == id);
            Book b = (from p in db.Books where p.BookId == BookId select p).ToArray()[0];

            return View(b);
        }

        public ActionResult Create()
        {
            List<Publisher> pub = db.Publishers.ToList();
            SelectList ListPub = new SelectList(pub, "PubId", "Name");
            List<Category> cate = db.Categories.ToList();
            SelectList ListCate = new SelectList(cate,"CateId","CateName");
            List<Author> au = db.Authors.ToList();
            SelectList ListAuthor = new SelectList(au, "AuthorId","AuthorName");
            ViewBag.AuthorList = ListAuthor;
            ViewBag.CategoryList = ListCate;
            ViewBag.PubList = ListPub;
            return View();
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            //xử lí upload
            file.SaveAs(Server.MapPath("~/Images/" + file.FileName));
            return "/Images/" + file.FileName;
        }
        public string UploadEdit(HttpPostedFileBase file)
        {
            //xử lí upload
            file.SaveAs(Server.MapPath("~/Images" + file.FileName));
            return file.FileName;
        }
        [HttpPost]

        public ActionResult Tao(FormCollection frmTao, Book Book)
        {

            Book.Title = frmTao["title"];
            Book.Summary = frmTao["mota"];
            Book.CateId = Convert.ToInt32(frmTao["cate"]);
            Book.AuthorId = Convert.ToInt32(frmTao["author"]);
            Book.PubId = Convert.ToInt32(frmTao["pub"]);
            Book.Price = Convert.ToDecimal(frmTao["gia"]);
            Book.Quantity = frmTao["SL"];
            Book.IsActive = frmTao["trangthai"];
            Book.CreatedDate = DateTime.Now;
            Book.ImgUrl = frmTao["fileUpload"];
            db.Books.Add(Book);
            db.SaveChanges();
            return RedirectToAction("Index", "AdminCRUD");
        }

        public ActionResult createAuthor()
        {
            var f = from s in db.Authors select s;
            ViewBag.sklist = db.Authors.ToList();
            return View();

        }
        [HttpPost]
        public ActionResult createAuthor(FormCollection frmCreate, Author a) { 

            a.AuthorName = frmCreate["AuthorName"];
            a.History = frmCreate["History"];
            a.SDT = frmCreate["SDT"];

            a.Adress = frmCreate["Adress"];

            db.Authors.Add(a);
            db.SaveChanges();
            return RedirectToAction("createAuthor", "AdminCRUD");
        }
        public ActionResult createPub()
        {
            var f = from s in db.Publishers select s;
            ViewBag.sklist = db.Publishers.ToList();
            return View();

        }
        [HttpPost]
        public ActionResult createPub(FormCollection frmCreate, Publisher p)
        {

            p.Name = frmCreate["Name"];
            p.Description = frmCreate["Description"];
            p.Adress = frmCreate["Adress"];
            p.SDT = frmCreate["SDT"];

            db.Publishers.Add(p);
            db.SaveChanges();
            return RedirectToAction("createPub", "AdminCRUD");
        }
        public ActionResult createCate()
        {
            var f = from s in db.Categories select s;
            ViewBag.sklist = db.Categories.ToList();
            return View();

        }
        [HttpPost]
        public ActionResult createCate(FormCollection frmCreate, Category c)
        {

            c.CateName = frmCreate["CateName"];
            c.Description = frmCreate["Description"];

            db.Categories.Add(c);
            db.SaveChanges();
            return RedirectToAction("createCate", "AdminCRUD");
        }
    }
}


