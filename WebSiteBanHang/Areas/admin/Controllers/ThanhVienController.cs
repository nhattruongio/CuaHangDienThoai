using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Areas.admin.Controllers
{
    public class ThanhVienController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        [Authorize(Roles = "QLThanhVien")]
        public ActionResult Index()
        {
            var lstTV = db.ThanhViens;
            return View(lstTV);
        }
        public ActionResult DanhSachThanhVien(string timkiem, int? page)
        {
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            if (timkiem != null)
            {
                List<ThanhVien> listKQ = db.ThanhViens.Where(n => n.HoTen.Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy khách hàng nào phù hợp.";
                    return View(db.ThanhViens.OrderBy(n => n.HoTen).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderBy(n => n.HoTen).ToPagedList(pageNumber, pageSize));
            }
            return View(db.ThanhViens.OrderBy(n => n.HoTen).ToPagedList(pageNumber, pageSize));

        }
        [Authorize(Roles = "QLDonHang")]


        public ActionResult TruyVan1DoiTuong()
        {

            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == 2);
            return View(tv);
        }
        public ActionResult SortDuLieu()
        {
            List<ThanhVien> lstTv = db.ThanhViens.OrderByDescending(n => n.HoTen).ToList();
            return View(lstTv);
        }
        public ActionResult GroupDuLieu()
        {
            List<ThanhVien> lstTV = new List<ThanhVien>();
            lstTV = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstTV);
        }
        [ValidateInput(false)]
        [HttpGet]
        public ActionResult XetQuyen(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                return HttpNotFound();
            }
            //Load dropdownlist loại thành viên
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai", tv.MaLoaiTV);
            return View(tv);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult XetQuyen(ThanhVien model)
        {
            //Load dropdownlist loại thành viên
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai",model.MaLoaiTV);

            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachThanhVien");
        }
        [HttpGet]
        public ActionResult Xoa(int? id, ThanhVien tv)
        {
            //Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ThanhVien sp = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            //Load dropdownlist nhà cung cấp và dropdownlist loại sp, mã nhà sản xuất
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", tv.MaLoaiTV);
            return View(sp);
        }

        [HttpPost]
        public ActionResult Xoa(int? id, FormCollection f)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            db.SaveChanges();
            return RedirectToAction("DanhSachThanhVien");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}