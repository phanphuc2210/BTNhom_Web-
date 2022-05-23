using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTNhom_MocPhuc.Models;
using System.Security.Cryptography;
using System.Text;

namespace BTNhom_MocPhuc.Controllers
{
    public class SiteController : Controller
    {

        private MocPhucEntities db = new MocPhucEntities();

        string LayMaKH()
        {
            var maMax = db.KHACHHANGs.ToList().Select(n => n.MAKH).Max();
            int maKH;
            if (maMax == null)
            {
                maKH = 1;
            } else
            {
                maKH = int.Parse(maMax.Substring(2)) + 1;
            }
          
            string KH = String.Concat("000", maKH.ToString());
            return "KH" + KH.Substring(maKH.ToString().Length - 1);
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

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy([Bind(Include = "MAKH,TEN,TENDEM,HO,DIACHI,SDT,AVATAR,EMAIL,PASS,ConfirmPassword")] KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                var check = db.KHACHHANGs.FirstOrDefault(s => s.EMAIL == kh.EMAIL);
                if (check == null)
                {
                   
                        kh.MAKH = LayMaKH();
                        kh.PASS = GetMD5(kh.PASS);
                        kh.AVATAR = "anhkh.png";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.KHACHHANGs.Add(kh);
                        db.SaveChanges();
                        return RedirectToAction("./DangNhap");
                    
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại !!!";
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
        public ActionResult DangNhap(string EMAIL, string PASS)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(PASS);
                var data = db.KHACHHANGs.Where(s => s.EMAIL.Equals(EMAIL) && s.PASS.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //tạo session
                    Session["TEN"] = data.FirstOrDefault().TEN;
                    Session["AVATAR"] = data.FirstOrDefault().AVATAR;
                    Session["MAKH"] = data.FirstOrDefault().MAKH;

                    HttpCookie ck = new HttpCookie("myCookies");
                    ck["idUser"] = data.FirstOrDefault().MAKH;
                    //tạo Cookies
                    Response.Cookies.Add(ck);
                    ck.Expires = DateTime.Now.AddDays(1); //giới hạn thời gian 1 ngày

                    return RedirectToAction("../");
                }
                else
                {
                    ViewBag.error = "Email hoặc mật khẩu không đúng !!!";
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
            if(Request.Cookies["myCookies"] != null)
            {
                HttpCookie myCookie = new HttpCookie("myCookies");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            return RedirectToAction("../");
        }



        public ActionResult GioHang()
        {
            return View();
        }
    }
}