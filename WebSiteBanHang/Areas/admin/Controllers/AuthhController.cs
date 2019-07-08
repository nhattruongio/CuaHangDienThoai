using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Areas.Admin.Controllers
{
    public class AuthhController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        
        //Xây dựng Action đăng nhập
        [HttpGet]      
        [AllowAnonymous]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]

        public JsonResult DangNhap(string username, string password)
        {
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == username && n.MatKhau == password);

            Session["TaiKhoan1"] = tv.TaiKhoan;
            if (tv != null)
            {
                Session["MaThanhVien"] = tv.MaThanhVien.ToString();
               if (tv.MaLoaiTV == 1)
                {
                    return Json(new { data = "Đăng nhập thành công", status = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "Tài khoản không có quyền truy cập", status = false}, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { data = "Sai tên đăng nhập hoặc mật khẩu", status = false }, JsonRequestBehavior.AllowGet);

            }
        }
     
        public ActionResult Dangxuat()
        {
            //Gán về null
            Session["TaiKhoan1"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap","Authh");
        }
    }
}