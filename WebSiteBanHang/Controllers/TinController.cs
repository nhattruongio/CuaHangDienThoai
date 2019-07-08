using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebSiteBanHang.Models;
namespace WebSiteBanHang.Controllers
{
    public class TinController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: Tin
        public ActionResult Index()
        {
            IQueryable<Tin> lstTin = db.Tins.Where(n => n.MaTin >0);
            ViewBag.ListTin = lstTin;
            return View();

        }
        public ActionResult TinPartial()
        {
            var lstTin = db.SanPhams;
            return PartialView(lstTin);
        }
        //Xây dựng trang xem chi tiết
        public ActionResult ChiTiet(int? id, string tensp)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tin tin = db.Tins.SingleOrDefault(n =>n.MaTin ==id);
            if (tin == null)
            {
                return HttpNotFound();
            }
            return View(tin);
        }
    }
}