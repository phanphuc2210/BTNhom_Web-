using BTNhom_MocPhuc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BTNhom_MocPhuc.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        private MocPhucEntities db = new MocPhucEntities();

        public int InsertHoaDon(HOADON hoaDon)
        {
            db.HOADONs.Add(hoaDon);

            
            return hoaDon.IDHD;
        }

        public bool InsertCTHD(CTHD cthd)
        {
            try
            {
                db.CTHDs.Add(cthd);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if(cart != null)
            {
                list = (List<CartItem>)cart;

            }

            return View(list);
        }

        public JsonResult DeleteALL()
        {
            Session[CartSession] = null;
            return Json(new { status = true });
        }

        public JsonResult Delete(string maSP)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.sanPham.MASP == maSP);
            Session[CartSession] = sessionCart;

            return Json(new { status = true });
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];

            foreach(var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.sanPham.MASP == item.sanPham.MASP);
                if(jsonItem != null)
                {
                    item.soLuong = jsonItem.soLuong;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new { status = true });
        }

        public ActionResult AddItem(string maSP, int soLuong)
        {
            var sanPham = db.SANPHAMs.FirstOrDefault(s => s.MASP == maSP);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if(list.Exists(x => x.sanPham.MASP == maSP))
                {
                    foreach(var item in list)
                    {
                        if(item.sanPham.MASP == maSP)
                        {
                            item.soLuong += soLuong;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng Cart Item
                    var item = new CartItem();
                    item.sanPham = sanPham;
                    item.soLuong = soLuong;
                    list.Add(item);
                }

                //Gán vào session
                Session[CartSession] = list;
            }
            else
            {
                //tạo mới đối tượng Cart Item
                var item = new CartItem();
                item.sanPham = sanPham;
                item.soLuong = soLuong;
                var list = new List<CartItem>();
                list.Add(item);

                //Gán vào session
                Session[CartSession] = list;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Payment()
        {
            if(Session["MAKH"] != null)
            {
                var cart = Session[CartSession];
                var list = new List<CartItem>();
                if (cart != null)
                {
                    list = (List<CartItem>)cart;
                }

                var khachHang = db.KHACHHANGs.Find(Session["MAKH"].ToString());
                ViewBag.KHACHHANG = khachHang;

                var ptThanhToan = db.PHUONGTHUCTTs.SqlQuery("SELECT * FROM PHUONGTHUCTT");
                ViewBag.ptThanhToan = ptThanhToan.ToList();

                return View(list);
            }
            else
            {
                return Redirect("/KHACHHANG/DangNhap");
            }
           
        }

        [HttpPost]
        public ActionResult Payment(string NOIGIAOHANG, string ptThanhToan)
        {


            var hoaDon = new HOADON();
            hoaDon.MAKH = Session["MAKH"].ToString();
            hoaDon.NGAYDATHANG = DateTime.Today;
            hoaDon.NGAYGIAOHANG = DateTime.Today.AddDays(5);
            hoaDon.NOIGIAOHANG = NOIGIAOHANG;
            hoaDon.MAPT = ptThanhToan;
            hoaDon.TINHTRANG = "TT1";

            try
            {
                var id = InsertHoaDon(hoaDon);
                var cart = (List<CartItem>)Session[CartSession];
                foreach (var item in cart)
                {
                    var cthd = new CTHD();
                    cthd.MASP = item.sanPham.MASP;
                    cthd.IDHD = id;
                    cthd.DONGIA = (int)item.sanPham.GIABAN * item.soLuong;
                    cthd.SOLUONG = item.soLuong;
                    InsertCTHD(cthd);


                    SANPHAM sANPHAM = db.SANPHAMs.Find(item.sanPham.MASP);
                    sANPHAM.SOLUONGTON -= item.soLuong;
                    db.Entry(sANPHAM).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }

            

            Session[CartSession] = null;
            return Redirect("/Cart/Success");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}