using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using BTNhom_MocPhuc.Models;
using System.Data.Entity;

namespace BTNhom_MocPhuc.Controllers
{
    public class QuanTriController : Controller
    {
        private MocPhucEntities db = new MocPhucEntities();
       

        string LayMaNV()
        {
            var maMax = db.NHANVIENs.ToList().Select(n => n.MANV).Max();
            int maNV;
            if (maMax == null)
            {
                maNV = 1;
            }
            else
            {
                maNV = int.Parse(maMax.Substring(2)) + 1;
            }

            string NV = String.Concat("00", maNV.ToString());
            return "NV" + NV.Substring(maNV.ToString().Length - 1);
        }

        //Dùng chuyển đổi một chuỗi về dạng mã hóa MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        // GET: QuanTri
        public ActionResult Index()
        {
            if(Session["MANV"] != null)
            {
                string maNV = Session["MANV"].ToString();
                NHANVIEN nv = db.NHANVIENs.FirstOrDefault(x => x.MANV == maNV);
                ViewBag.Admin = nv.IsAdmin;
                return View();
            }
            else
            {
                return RedirectToAction("DangNhap");
            }
            
        }


        [HttpGet]
        public ActionResult DoanhThu(string tuNgay = "", string denNgay = "")
        {
            NHANVIEN nv = null;
            if(Session["MANV"] != null)
            {
                string maNV = Session["MANV"].ToString();
                nv = db.NHANVIENs.FirstOrDefault(x => x.MANV == maNV);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }

            if (nv.IsAdmin == true)
            {
                ViewBag.TuNgay = tuNgay;
                ViewBag.DenNgay = denNgay;

                var hd = db.HOADONs.SqlQuery("SELECT * FROM HOADON WHERE NGAYDATHANG > '" + tuNgay + "' AND NGAYDATHANG < '" + denNgay + "'");
                if (hd.Count() == 0)
                    ViewBag.TB = "Không có thông tin thống kê.";
                return View(hd.ToList());
            }
            else
            {
                return RedirectToAction("DangNhap");
            }

        }


        public ActionResult DangKy()
        {
            NHANVIEN nv = null;
            if (Session["MANV"] != null)
            {
                string maNV = Session["MANV"].ToString();
                nv = db.NHANVIENs.FirstOrDefault(x => x.MANV == maNV);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }

            if (nv.IsAdmin == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("DangNhap");
            }

            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy([Bind(Include = "MANV,TEN,TENDEM,HO,NGAYSINH,DIACHI,SDT,AVATAR,EMAIL,TENDN,PASS,IsAdmin,ConfirmPassword")] NHANVIEN nv)
        {

            if (ModelState.IsValid)
            {
                var check = db.NHANVIENs.FirstOrDefault(s => s.EMAIL == nv.EMAIL);
                var check2 = db.NHANVIENs.FirstOrDefault(s => s.TENDN == nv.TENDN);
                if (check == null && check2 == null)
                {
                    nv.MANV = LayMaNV();
                    nv.PASS = GetMD5(nv.PASS);
                    nv.AVATAR = "employee.png";
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.NHANVIENs.Add(nv);
                    db.SaveChanges();
                    return Redirect("/NHANVIEN/Index");

                }
                else if(check != null)
                {
                    ViewBag.error = "Email đã tồn tại !!!";
                    
                    return View();
                }

                else if(check2 != null)
                {
                    ViewBag.error2 = "Tên đăng nhập đã tồn tại";
                    return View();
                }
            }

            return View();
        }

        public ActionResult DangNhap()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string TENDN, string PASS)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(PASS);
                var data = db.NHANVIENs.Where(s => s.TENDN.Equals(TENDN) && s.PASS.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //tạo session
                    Session["TENNV"] = data.FirstOrDefault().TEN;
                    Session["AVATARNV"] = data.FirstOrDefault().AVATAR;
                    Session["MANV"] = data.FirstOrDefault().MANV;

                    HttpCookie ck = new HttpCookie("nvCookies");
                    ck["idNV"] = data.FirstOrDefault().MANV;
                    //tạo Cookies
                    Response.Cookies.Add(ck);
                    ck.Expires = DateTime.Now.AddDays(1); //giới hạn thời gian 1 ngày

                    return Redirect("/QuanTri/Index");
                }
                else
                {
                    ViewBag.error = "Tên đang nhập hoặc mật khẩu không đúng !!!";
                    return View();
                }
            }
            return View();
        }

        //Logout
        public ActionResult DangXuat()
        {
            //Xóa session
            Session.Clear();
            //Xóa cookies
            if (Request.Cookies["nvCookies"] != null)
            {
                HttpCookie myCookie = new HttpCookie("nvCookies");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            return RedirectToAction("DangNhap");
        }

        [HttpGet]
        public ActionResult XacNhanHD()
        {
            NHANVIEN nv = null;
            if (Session["MANV"] != null)
            {
                string maNV = Session["MANV"].ToString();
                nv = db.NHANVIENs.FirstOrDefault(x => x.MANV == maNV);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }


            if (nv.IsAdmin == true)
            {
                var orders = db.HOADONs.Where(x => x.TINHTRANG == "TT1").ToList();
                ViewBag.Count = orders.Count;

                return View(orders);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }

            
        }

        [HttpGet]
        public ActionResult NhanGiaoHang()
        {
            NHANVIEN nv = null;
            if (Session["MANV"] != null)
            {
                string maNV = Session["MANV"].ToString();
                nv = db.NHANVIENs.FirstOrDefault(x => x.MANV == maNV);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }


            if (nv.IsAdmin == false)
            {
                var orders = db.HOADONs.Where(x => x.TINHTRANG == "TT2").ToList();
                ViewBag.Count = orders.Count;

                return View(orders);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }

            
        }

        [HttpGet]
        public ActionResult DaGiaoHang()
        {
            NHANVIEN nv = null;
            if (Session["MANV"] != null)
            {
                string maNV = Session["MANV"].ToString();
                nv = db.NHANVIENs.FirstOrDefault(x => x.MANV == maNV);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }


            if (nv.IsAdmin == false)
            {
                string maNVGiao = Session["MANV"].ToString();
                var orders = db.HOADONs.Where(x => x.MANVGIAO == maNVGiao).ToList();
                ViewBag.Count = orders.Count;

                return View(orders);
            }
            else
            {
                return RedirectToAction("DangNhap");
            }

            
        }

        public JsonResult XacNhan(string id)
        {
            HOADON hd = db.HOADONs.FirstOrDefault(x => x.IDHD.ToString() == id);
            hd.TINHTRANG = "TT2";
            hd.MANVDUYET = Session["MANV"].ToString();
            db.Entry(hd).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { status = true });
        }

        public JsonResult GiaoDon(string id)
        {
            HOADON hd = db.HOADONs.FirstOrDefault(x => x.IDHD.ToString() == id);
            hd.TINHTRANG = "TT3";
            hd.MANVGIAO = Session["MANV"].ToString();
            db.Entry(hd).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { status = true });
        }

        public JsonResult DaGiao(string id)
        {
            HOADON hd = db.HOADONs.FirstOrDefault(x => x.IDHD.ToString() == id);
            hd.TINHTRANG = "TT4";
            hd.NGAYGIAOHANG = DateTime.Today;
            db.Entry(hd).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { status = true });
        }
    }
}