using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using System.Web.Security;
using System.Web.Helpers;

namespace WebSiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        private List<SanPham> spmoi(int count)
        {
            return db.SanPhams.OrderByDescending(a => a.NgayCapNhap).Take(count).ToList();
        }
        public ActionResult Index()
        {

            // List điện thoại
            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1).OrderByDescending(n => n.SoLanMua);
            ViewBag.ListDTM = lstDTM;
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }

            public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams; 
            return PartialView(lstSP);
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.question = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpPost]
        public JsonResult DangKy(string username, string password, string repassword, string email, string address, string cellphone, string answer,string question)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
             
            ////Kiểm tra Captcha hợp lệ
            //if (this.IsCaptchaValid("Captcha is not valid"))
            //{
            var KiemTraMail = db.ThanhViens.Where(u => u.Email == email).FirstOrDefault();
            if (KiemTraMail == null)
            {
                    //if (password != repassword)
                    //{
                    //    ViewData["Loi1"] = "không trùng mật khẩu!";
                    //}
                    //if(password.Length <= 6)
                    //{
                    //    ViewData["Loi2"] = "Mật khẩu phải lớn hơn 6 kí tự!";
                    //}
                    //if (KiemTraMail == null)
                    //{
                    //    ViewData["Loi3"] = "Email đã tồn tại!";
                    //}
                   
                    ThanhVien tv = new ThanhVien
                    {
                        TaiKhoan = username,
                        MatKhau = password,
                        Email = email,
                        DiaChi = address,
                        SoDienThoai = cellphone,
                        CauHoi = question,
                        CauTraLoi = answer
                    };
                    var dangky = db.ThanhViens.Add(tv);

                    if (dangky != null)
                    {
                        db.SaveChanges();
                        return Json(new { data = "Đăng ký thành công", status = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = "Đăng ký thất bại", status = false }, JsonRequestBehavior.AllowGet);
                    }

            }
            else
            {
                return Json(new { data = "Email đã tồn tại, vui lòng nhập lại", status = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Ca sĩ mà bạn yêu thích?");
            lstCauHoi.Add("Nghề nghiệp của bạn là gì?");
            return lstCauHoi;
        }

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
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n=>n.TaiKhoan== username && n.MatKhau== password);
            Session["TaiKhoan"] = tv;
            if (tv != null)
            {
                Session["MaThanhVien"] = tv.MaThanhVien.ToString();

                return Json(new { data = "Đăng nhập thành công", status = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "Sai tên đăng nhập hoặc mật khẩu", status = false }, JsonRequestBehavior.AllowGet);
            }
        }
    
        public ActionResult Dangxuat()
        {
            //Gán về null
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult DangPhatTrien()
        {
            return Index();
        }

        [HttpGet]
        public ActionResult DoiMatKhau()
        {
            if (Session != null || (Session)["MaThanhVien"] != null || (Session)["MaThanhVien"].ToString() != "")
            {
                int matv = (int)Session["MaThanhVien"];

                var tvv = db.ThanhViens.Where(n=> n.MaThanhVien == matv).FirstOrDefault();
                ViewBag.tvv = tvv;
            }
            else
            {
                return RedirectToAction("DoiMatKhau", "Home", new { area = "" });
            }

            return View();
        }

        [HttpPost]
        public JsonResult DoiMatKhau(string old_password, string new_password)
        {
            int matv = (int)Session["MaThanhVien"];
            var updatePassword = db.ThanhViens.Where(x => x.MaThanhVien == matv && x.MatKhau == old_password).FirstOrDefault();

            if (updatePassword != null)
            {
                updatePassword.MatKhau = new_password;
                db.SaveChanges();

                return Json(new { data = "Đổi mật khẩu thành công", status = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = "Mật khẩu cũ không chính xác", status = false }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public ActionResult QuenMatKhau()
        //{
        //    if (Session == null || Session["UserID"] == null)
        //    {
        //        ViewBag.Title = "Đặt lại mật khẩu";
        //        return View();
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home", new { area = "Client" });
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public JsonResult QuenMatKhau(string Mail)
        //{
        //    bool flag = false;
        //    var checkMailExists = db.ThanhViens.Where(u => u.Email == Mail).FirstOrDefault();
        //    if (checkMailExists != null)
        //    {
        //        try
        //        {
        //            string code = GenerateString.RandomString(5);
        //            var query = _dbContext.Users.SingleOrDefault(u => u.Email == Mail);
        //            query.ForgotPassword = code;

        //            string body = "<h1><font color = \"#00b894\">Thay đổi mật khẩu của bạn</font></h1><br /><p><i><font color=\"#dfe6e9\">Chúng tôi đã nhận được yêu cầu thay đổi mật khẩu của bạn. Nếu bạn đổi ý, hãy bỏ qua email này.<br />Mã xác nhận của bạn là</font></i></p><br /><h2>" + code + "</h2>";

        //            WebMail.SmtpServer = "smtp.gmail.com";
        //            WebMail.SmtpPort = 587;
        //            WebMail.SmtpUseDefaultCredentials = true;
        //            WebMail.EnableSsl = true;
        //            WebMail.UserName = "phuxxxtruong@gmail.com";
        //            WebMail.Password = "Truongcuty9997";
        //            WebMail.From = "phuxxxtruong@gmail.com";
        //            WebMail.Send(to: Mail, subject: "Yêu cầu thay đổi mật khẩu", body: body, isBodyHtml: true);
        //            _dbContext.SaveChanges();
        //            mailPasswd = Mail;

        //            flag = true;
        //        }
        //        catch (Exception)
        //        {
        //            flag = false;
        //        }
        //    }
        //    else
        //    {
        //        flag = false;
        //    }

        //    return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public ActionResult DoiMatKhau()
        //{
        //    if ( == "" || mailPasswd == null)
        //    {
        //        return RedirectToAction("ForgotPassword", "Account", new { area = "" });
        //    }
        //    else
        //    {
        //        ViewBag.Title = "Đổi mật khẩu";
        //        ViewBag.mailPasswd = mailPasswd;
        //        return View();
        //    }
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public JsonResult ChangePassword(string code, string passwd)
        //{
        //    bool flag = false;
        //    string password = StringHash.crypto(passwd);

        //    string Upper_code = code.ToUpper();
        //    var update_password = _dbContext.Users
        //                                .Where(u => u.Email == mailPasswd)
        //                                .Where(u => u.ForgotPassword == Upper_code)
        //                                .FirstOrDefault();

        //    if (update_password != null)
        //    {
        //        update_password.ForgotPassword = "";
        //        update_password.Password = password;
        //        _dbContext.SaveChanges();
        //        mailPasswd = "";

        //        flag = true;
        //    }
        //    else
        //    {
        //        flag = false;
        //    }
        //    return Json(new { status = flag }, JsonRequestBehavior.AllowGet);
        //}

    }
}