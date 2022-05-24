using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTNhom_MocPhuc.Models;

namespace BTNhom_MocPhuc.Controllers
{
    public class SANPHAMController : Controller
    {
        private MocPhucEntities db = new MocPhucEntities();

        [HttpGet]
        public ActionResult TimKiem(string timKiem = "")
        {
            ViewBag.timKiem = timKiem;

            var sanPhams = db.SANPHAMs.SqlQuery("SELECT * FROM SANPHAM WHERE TENSP LIKE N'%" + timKiem + "%'");
            

            return View(sanPhams.ToList());
        }
             
        // GET: SANPHAM/Home
        public ActionResult Home()
        {
            var sANPHAM1 = db.SANPHAMs.SqlQuery("SELECT TOP(4) * FROM SANPHAM ORDER BY NGAYTHEM DESC");
            var sANPHAM2 = db.SANPHAMs.SqlQuery("SELECT TOP(4) * FROM SANPHAM");

            ViewBag.SPMoi = sANPHAM1.ToList();
            ViewBag.SPBanChay = sANPHAM2.ToList();
            return View();
        }

        // GET: SANPHAM
        public ActionResult Index()
        {
            var sANPHAMs = db.SANPHAMs.Include(s => s.LOAISP);
            return View(sANPHAMs.ToList());
        }

        // GET: SANPHAM/Categories/Ban
        public ActionResult Categories(string id)
        {
            var sANPHAMs = db.SANPHAMs.SqlQuery("EXEC SP_LAYDANHMUC_SPHAM " + id);
            var tenLSP = db.LOAISPs.FirstOrDefault(s => s.MALSP == id);

            if (tenLSP == null)
            {
                ViewBag.tenLSP = "Tất cả sản phẩm";
            }
            else
            {
                ViewBag.tenLSP = tenLSP.TENLSP.ToString();
            }

            if (sANPHAMs.Count() == 0)
            {
                return HttpNotFound();
            }

            return View(sANPHAMs.ToList());
        }

        

        

        // GET: SANPHAM/Details/SP001
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);

            var bINHLUANs = db.BINHLUANs.Include(b => b.KHACHHANG).Include(b => b.SANPHAM).Where(s => s.MASP == id).OrderByDescending(s => s.THOIGIANBL);
            ViewBag.binhLuan = bINHLUANs.ToList();


            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // GET: SANPHAM/Create
        public ActionResult Create()
        {
            ViewBag.MALSP = new SelectList(db.LOAISPs, "MALSP", "TENLSP");
            return View();
        }

        // POST: SANPHAM/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MASP,MALSP,TENSP,SOLUONGTON,IMG,MOTA,GIABAN,NGAYTHEM")] SANPHAM sANPHAM)
        {
            if (ModelState.IsValid)
            {
                db.SANPHAMs.Add(sANPHAM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MALSP = new SelectList(db.LOAISPs, "MALSP", "TENLSP", sANPHAM.MALSP);
            return View(sANPHAM);
        }

        // GET: SANPHAM/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            ViewBag.MALSP = new SelectList(db.LOAISPs, "MALSP", "TENLSP", sANPHAM.MALSP);
            return View(sANPHAM);
        }

        // POST: SANPHAM/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MASP,MALSP,TENSP,SOLUONGTON,IMG,MOTA,GIABAN,NGAYTHEM")] SANPHAM sANPHAM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sANPHAM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MALSP = new SelectList(db.LOAISPs, "MALSP", "TENLSP", sANPHAM.MALSP);
            return View(sANPHAM);
        }

        // GET: SANPHAM/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // POST: SANPHAM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            db.SANPHAMs.Remove(sANPHAM);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
