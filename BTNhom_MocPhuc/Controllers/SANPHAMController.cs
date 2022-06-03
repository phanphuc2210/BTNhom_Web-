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

        string LayMaSP()
        {
            var maMax = db.SANPHAMs.ToList().Select(n => n.MASP).Max();
            int maSP;
            if (maMax == null)
            {
                maSP = 1;
            }
            else
            {
                maSP = int.Parse(maMax.Substring(2)) + 1;
            }

            string SP = String.Concat("00", maSP.ToString());
            return "SP" + SP.Substring(maSP.ToString().Length - 1);
        }

        [HttpGet]
        public ActionResult TimKiem(string timKiem = "")
        {
            ViewBag.timKiem = timKiem;

            var sanPhams = db.SANPHAMs.SqlQuery("SELECT * FROM SANPHAM WHERE TENSP LIKE N'%" + timKiem + "%'");
            

            return View(sanPhams.ToList());
        }

        [HttpGet]
        public ActionResult TimKiemNC(string MASP = "", string TENSP = "", string MALSP = "")
        {
            if(Session["MANV"] != null)
            {
                ViewBag.MASP = MASP;
                ViewBag.TENSP = TENSP;
                ViewBag.MALSP = new SelectList(db.LOAISPs, "MALSP", "TENLSP");
                var sp = db.SANPHAMs.SqlQuery("SanPham_TimKiem'" + MASP + "',N'" + TENSP + "','" + MALSP + "'");
                if (sp.Count() == 0)
                    ViewBag.TB = "Không có thông tin tìm kiếm.";
                return View(sp.ToList());
            }
           
            else
            {
                return Redirect("/QuanTri/DangNhap");
            }
        }

        // GET: SANPHAM/Home
        public ActionResult Home()
        {
            var sANPHAM1 = db.SANPHAMs.SqlQuery("SELECT TOP(4) * FROM SANPHAM ORDER BY NGAYTHEM DESC");
            var sANPHAM2 = db.SANPHAMs.SqlQuery("SELECT TOP(4) * FROM SANPHAM");

            ViewBag.SPMoi = sANPHAM1.ToList();
            ViewBag.SPGoiY = sANPHAM2.ToList();
            return View();
        }

        // GET: SANPHAM
        public ActionResult Index()
        {
            if(Session["MANV"] != null)
            {
                var sANPHAMs = db.SANPHAMs.Include(s => s.LOAISP);
                return View(sANPHAMs.ToList());
            }
            else
            {
                return Redirect("/QuanTri/DangNhap");
            }

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


        // GET: SANPHAM/Comments/SP001
        public ActionResult Comments(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bINHLUANs = db.BINHLUANs.Include(b => b.KHACHHANG).Where(s => s.MASP == id).OrderByDescending(s => s.THOIGIANBL);
            ViewBag.binhLuan = bINHLUANs.ToList();
            ViewBag.TENSP = db.SANPHAMs.FirstOrDefault(x => x.MASP == id).TENSP;

            
            return View();
        }


        // GET: SANPHAM/Details/SP001
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);

            var spTuongTu = db.SANPHAMs.SqlQuery("SELECT TOP(4) * FROM SANPHAM WHERE MALSP = '" + sANPHAM.MALSP + "' AND MASP != '" + id + "'");
            ViewBag.spTT = spTuongTu.ToList();

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
            //System.Web.HttpPostedFileBase Avatar;
            var imgSP = Request.Files["Avatar"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(imgSP.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/" + postedFileName);
            imgSP.SaveAs(path);

            if (ModelState.IsValid)
            {
                sANPHAM.MASP = LayMaSP();
                sANPHAM.IMG = postedFileName;
                sANPHAM.NGAYTHEM = DateTime.Today;  
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
            var imgSP = Request.Files["Avatar"];
            try
            {
                //Lấy thông tin từ input type=file có tên Avatar
                string postedFileName = System.IO.Path.GetFileName(imgSP.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Images/" + postedFileName);
                imgSP.SaveAs(path);
            }
            catch { }

            if (ModelState.IsValid)
            {
                db.Entry(sANPHAM).State = EntityState.Modified;
                db.SaveChanges();
                
            }
            ViewBag.MALSP = new SelectList(db.LOAISPs, "MALSP", "TENLSP", sANPHAM.MALSP);
            return View(sANPHAM);
        }

        
    }
}
