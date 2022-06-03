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
    public class NHANVIENController : Controller
    {
        private MocPhucEntities db = new MocPhucEntities();

        // GET: NHANVIEN
        public ActionResult Index()
        {
            if(Session["MANV"] != null)
            {
                return View(db.NHANVIENs.ToList());
            }
            else
            {
                return Redirect("/QuanTri/DangNhap");
            }
        }

        // GET: NHANVIEN/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nHANVIEN = db.NHANVIENs.Find(id);
            if (nHANVIEN == null)
            {
                return HttpNotFound();
            }
            return View(nHANVIEN);
        }


        // GET: NHANVIEN/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHANVIEN nHANVIEN = db.NHANVIENs.Find(id);
            if (nHANVIEN == null)
            {
                return HttpNotFound();
            }
            return View(nHANVIEN);
        }

        // POST: NHANVIEN/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANV,TEN,TENDEM,HO,NGAYSINH,DIACHI,SDT,AVATAR,EMAIL,TENDN,PASS,IsAdmin,ConfirmPassword")] NHANVIEN nHANVIEN)
        {
            var imgNV = Request.Files["Avatar"];
            try
            {
                //Lấy thông tin từ input type=file có tên Avatar
                string postedFileName = System.IO.Path.GetFileName(imgNV.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Images/" + postedFileName);
                imgNV.SaveAs(path);
            }
            catch { }

            if (ModelState.IsValid)
            {
                db.Entry(nHANVIEN).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(nHANVIEN);
        }


       
    }
}
