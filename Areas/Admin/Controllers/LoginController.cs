using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookManage.Models;

namespace BookManage.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(User usermodel)
        {
            //New dbConnect
            using (ASM_BookEntities db = new ASM_BookEntities())
            {

                //Lấy username và password ở bản ghi đầu tiên
                var user = db.Admins.Where(x => x.UserName == usermodel.UserName && x.Password == usermodel.Password).FirstOrDefault();
                if (user == null)
                {
                    
                    ViewBag.error = "Email or Password is fail";
                    return View("LoginAdmin", usermodel);
                }
                else
                {
                    Session["Email"] = user.Email;
                    Session["Username"] = user.UserName;
                    //return View(user)

                    return RedirectToAction("Index", "AdminCRUD");
                }

            }
        }
        public ActionResult Test()
        {
            return View();
        }
    }
}