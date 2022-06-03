using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTNhom_MocPhuc.Models;

namespace BTNhom_MocPhuc.Controllers
{
    public class HOADONsController : Controller
    {
        private MocPhucEntities db = new MocPhucEntities();

        // GET: HOADONs
        public ActionResult Index()
        {
            if(Session["MANV"] != null)
            {
                var hOADONs = db.HOADONs.Include(h => h.KHACHHANG).Include(h => h.NHANVIEN).Include(h => h.NHANVIEN1).Include(h => h.PHUONGTHUCTT).Include(h => h.TINHTRANG1);
                return View(hOADONs.ToList());
            }
            else
            {
                return Redirect("/QuanTri/DangNhap");
            }
        }

        [HttpGet]
        public ActionResult TimKiemNC(string MAHD = "", string MANVDUYET = "", string MANVGIAO = "", string MAPT = "" , string MATT = "")
        {
            if(Session["MANV"] != null)
            {
                ViewBag.MAHD = MAHD;
                ViewBag.MANVDUYET = MANVDUYET;
                ViewBag.MANVGIAO = MANVGIAO;
                ViewBag.MAPT = new SelectList(db.PHUONGTHUCTTs, "MAPT", "TENPT");
                ViewBag.MATT = new SelectList(db.TINHTRANGs, "MATINHTRANG", "TENTINHTRANG");
                var hd = db.HOADONs.SqlQuery("HoaDon_TimKiem'" + MAHD + "','" + MANVDUYET + "','" + MANVGIAO + "','" + MAPT + "','" + MATT + "'");
                if (hd.Count() == 0)
                    ViewBag.TB = "Không có thông tin tìm kiếm.";
                return View(hd.ToList());
            }
            else
            {
                return Redirect("/QuanTri/DangNhap");
            }
        }

        // GET: HOADONs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var detailOrders = db.CTHDs.Where(x => x.IDHD == id).Include(x => x.SANPHAM).ToList();


            ViewBag.IDHD = id;

            var hd = db.HOADONs.FirstOrDefault(x => x.IDHD == id);


            ViewBag.TT = db.TINHTRANGs.FirstOrDefault(x => x.MATINHTRANG == hd.TINHTRANG).TENTINHTRANG;

            ViewBag.NgayDat = hd.NGAYDATHANG;
            ViewBag.KH = db.KHACHHANGs.FirstOrDefault(x => x.MAKH == hd.MAKH);

            ViewBag.ptThanhToan = db.PHUONGTHUCTTs.FirstOrDefault(x => x.MAPT == hd.MAPT).TENPT;

            
            if (detailOrders == null)
            {
                return HttpNotFound();
            }

            return View(detailOrders);
        }

      
    }
}
