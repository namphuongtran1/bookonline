using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookManage.Models
{
    public class SamPhamGH
    {
        ASM_BookEntities db = new ASM_BookEntities();
        public int BookId { get; set; }
        public string Title { get; set; }
      
        public string Summary { get; set; }
        public double Price { get; set; }
        public DateTime CretedDate { get; set; }
        public string ImgUrl { get; set; }
        public int Quantity { get; set; }
        public int SoLuongMua { get; set; }
        public int CateId { get; set; }
        public double TongTien => SoLuongMua * Price;
        public string CateName { get; set; }
    }
}