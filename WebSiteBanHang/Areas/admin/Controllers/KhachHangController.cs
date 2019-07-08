using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Areas.admin.Controllers
{
    public class KhachHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [Authorize(Roles = "QLKhachHang")]
        public ActionResult Index()
        {
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }
        public ActionResult DanhSachKhachHang(string timkiem, int? page)
        {
            ViewBag.TuKhoa = timkiem;
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            if (timkiem != null)
            {
                List<KhachHang> listKQ = db.KhachHangs.Where(n => n.TenKH.Contains(timkiem)).ToList();
                if (listKQ.Count == 0)
                {
                    TempData["thongbao"] = "Không tìm thấy khách hàng nào phù hợp.";
                    return View(db.KhachHangs.OrderBy(n => n.TenKH).ToPagedList(pageNumber, pageSize));
                }
                return View(listKQ.OrderBy(n => n.TenKH).ToPagedList(pageNumber, pageSize));
            }
            return View(db.KhachHangs.OrderBy(n => n.TenKH).ToPagedList(pageNumber, pageSize));

        }
        [Authorize(Roles = "QLDonHang")]


        public ActionResult TruyVan1DoiTuong()
        {
           
           KhachHang khang = db.KhachHangs.SingleOrDefault(n => n.MaKH == 2);
            return View(khang);
        }
        public ActionResult SortDuLieu()
        {
            List<KhachHang> lstKh = db.KhachHangs.OrderByDescending(n => n.TenKH).ToList();
            return View(lstKh);
        }
        public ActionResult GroupDuLieu()
        {
            List<ThanhVien> lstTV = new List<ThanhVien>();
            lstTV = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstTV);
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
