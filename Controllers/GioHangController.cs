using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookManage.Models;

namespace BookManage.Controllers
{
    public class GioHangController : Controller
    {
        ASM_BookEntities db = new ASM_BookEntities();

        public List<SamPhamGH> LayGioHang()
        {
            List<SamPhamGH> lstSP = Session["GioHang"] as List<SamPhamGH>;
            if (lstSP == null)
            {
                lstSP = new List<SamPhamGH>();
                Session["GioHang"] = lstSP;
            }
            return lstSP;
        }
        public ActionResult GioHang()
        {

            if (Session["makh"] == null)
            {
                return RedirectToAction("Index", "SSO");
            }
            else
            {
                List<SamPhamGH> listSP = LayGioHang();
                int TongSL = 0;
                double TongTien = 0;
                foreach (var item in listSP)
                {
                    TongSL += item.SoLuongMua;
                    TongTien += item.TongTien;
                    var tongtien1 = String.Format("{0:NO}", TongTien);
                    ViewBag.tongtien = tongtien1;
                }
                if (Session["MaGiam"] != null)
                {
                    int vat = (int)Session["MaGiam"];
                    TongTien = TongTien - TongTien * vat / 100;
                    var tongtien = String.Format("{0:N0}", TongTien);
                    Session["TongSL"] = TongSL.ToString();
                    Session["TongTien"] = tongtien.ToString();
                }
                else
                {
                    var tongtien = String.Format("{0:N0}", TongTien);
                    Session["TongSL"] = TongSL.ToString();
                    Session["TongTien"] = tongtien.ToString();
                }

                //Session["TongSL"] = TongSL.ToString();
                //Session["TongTien"] = TongTien.ToString();
                int y = int.Parse(Session["makh"].ToString());
                List<User> DG = db.Users.Where(n => n.MaKH == y).ToList();
                ViewData["listKH"] = DG;
                return View(listSP);
            }
        }
        [HttpPost]
        public ActionResult ThemGioHang(int iMaSP, int? SL)
        {
            List<SamPhamGH> lstSP = LayGioHang();
            SamPhamGH SP = lstSP.Find(n => n.BookId == iMaSP);
            if (SP == null)
            {
                SP = new SamPhamGH();
                Book sp = db.Books.Single(n => n.BookId == iMaSP);
                SP.BookId = iMaSP;
                SP.Title = sp.Title;
                SP.ImgUrl = sp.ImgUrl;
                SP.Price = double.Parse(sp.Price.ToString());
                if (SL == null)
                {
                    SP.SoLuongMua = 1;
                }
                else
                {
                    SP.SoLuongMua = int.Parse(SL.ToString());
                }
                lstSP.Add(SP);
                Session["GioHang"] = lstSP;
                return Json(lstSP, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (SL == null)
                {
                    SP.SoLuongMua++;
                }
                else
                {
                    SP.SoLuongMua = int.Parse(SL.ToString());
                }
                Session["GioHang"] = lstSP;
                return Json(lstSP, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult XoaSP(int iMaSP)
        {
            List<SamPhamGH> lstSP = LayGioHang();
            SamPhamGH SP = lstSP.Find(n => n.BookId == iMaSP);
            lstSP.Remove(SP);
            Session["GioHang"] = lstSP;
            return Json(lstSP, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ThanhToanThanhCong()
        {
            return View();
        }



        [HttpPost]
        public ActionResult GioHang(FormCollection frm, DONDATHANG donhang)
        {
            if (Session["makh"] == null)
            {
                return RedirectToAction("Index", "SSO");
            }
            else
            {

                int x = int.Parse(Session["makh"].ToString());
                var user = db.Users.FirstOrDefault(n => n.MaKH == x);
                if (user.DiaChi == null || user.DienThoaiKH == null)
                {
                    user = db.Users.Find(x);
                    {
                        user.DienThoaiKH = frm["dienthoainhanhang"];
                        user.DiaChi = frm["diachinhanhang"];

                    };
                    db.Entry(user);
                    bool am = false;
                    donhang.MaKH = int.Parse(Session["makh"].ToString());
                    donhang.NgayDH = DateTime.Parse(DateTime.Now.ToString());
                    //donhang.NgayGiaoHang = DateTime.Parse(frm["ngaynhanhang"].ToString());
                    donhang.TriGia = decimal.Parse(Session["TongTien"].ToString());
                    donhang.TrangThai = am;
                    donhang.TenNguoiNhan = frm["tennguoinhan"];
                    donhang.DienThoaiNhan = int.Parse(frm["dienthoainhanhang"]);
                    donhang.DiaChiNhan = frm["diachinhanhang"];

                    db.DONDATHANGs.Add(donhang);
                    db.SaveChanges();
                    List<SamPhamGH> listSP = LayGioHang();
                    foreach (var item in listSP)
                    {
                        CTDONHANG ctdh = new CTDONHANG();
                        ctdh.SoDH = donhang.SoHD;
                        ctdh.MaSP = item.BookId;
                        ctdh.SoLuong = item.SoLuongMua;
                        ctdh.DonGia = (decimal)item.Price;
                        db.CTDONHANGs.Add(ctdh);
                        db.SaveChanges();

                    }
                    Session["GioHang"] = null;
                    return RedirectToAction("ThanhToanThanhCong", "GioHang");
                }
                else
                {
                    bool a = false;
                    donhang.MaKH = int.Parse(Session["makh"].ToString());
                    donhang.NgayDH = DateTime.Parse(DateTime.Now.ToString());
                    //donhang.NgayGiaoHang = DateTime.Parse(frm["ngaynhanhang"].ToString());
                    donhang.TriGia = decimal.Parse(Session["TongTien"].ToString());
                    donhang.TrangThai = a;
                    donhang.TenNguoiNhan = frm["tennguoinhan"];
                    donhang.DienThoaiNhan = int.Parse(frm["dienthoainhanhang"]);
                    donhang.DiaChiNhan = frm["diachinhanhang"];
                    db.DONDATHANGs.Add(donhang);
                    db.SaveChanges();
                    List<SamPhamGH> listSP = LayGioHang();
                    foreach (var item in listSP)
                    {
                        CTDONHANG ctdh = new CTDONHANG();
                        ctdh.SoDH = donhang.SoHD;
                        ctdh.MaSP = item.BookId;
                        ctdh.SoLuong = item.SoLuongMua;
                        ctdh.DonGia = (decimal)item.Price;
                        db.CTDONHANGs.Add(ctdh);
                        db.SaveChanges();

                    }
                    Session["Madh"] = donhang.SoHD;
                    return RedirectToAction("ThanhToanThanhCong", "GioHang");
                }
            }
        }
        public ActionResult MaGiam(string magiam)
        {
            var Magiam1 = db.GiamGias.Where(x => x.MaGiamGia == magiam).FirstOrDefault();
            if (Magiam1.SoLuong > 0 && Magiam1 != null)
            {
                GiamGia dh = db.GiamGias.Find(Magiam1.ID);
                Session["MaGiam"] = Magiam1.GiaTri;
                Session["soluongma"] = null;
                dh.SoLuong--;
                db.Entry(dh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GioHang");
            }
            else if (Magiam1.SoLuong <= 0 || Magiam1 != null)
            {
                Session["soluongma"] = 0;
                return RedirectToAction("GioHang");
            }
            else
            {
                return View();
            }


        }
        
    }
}
