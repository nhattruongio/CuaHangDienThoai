﻿using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using WebSiteBanHang.Models;
namespace WebSiteBanHang.Areas.admin.Controllers
{
    public class QuanLyDonHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyDonHang
        public ActionResult ChuaThanhToan()
        {
            // Ds đơn hàng chưa duyệt
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false && n.TinhTrangGiaoHang == false).OrderByDescending(n => n.NgayDat);
            return View(lst);
        }

        public ActionResult ChuaGiao()
        {
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false && n.TinhTrangGiaoHang == true).OrderByDescending(n => n.NgayDat);
            return View(lst);
        }

        public ActionResult DaGiaoDaThanhToan()
        {
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == true && n.TinhTrangGiaoHang == true).OrderByDescending(n => n.NgayDat);
            return View(lst);
        }

        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n =>n.MaDDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            // Lấy ds chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n =>  n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            // Lấy dữ liệu của đơn hàng đó
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.TinhTrangGiaoHang = true;
            int id = int.Parse(ddhUpdate.MaKH.ToString());
            string email = db.KhachHangs.Where(x => x.MaKH == id).FirstOrDefault().Email.ToString();
            db.SaveChanges();
            // Lấy ds chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            GuiEmail("Xác nhận đơn hàng", email, "abc@gmail.com", "zewang.help", "Đơn hàng của bạn đã được đặt thành công cảm ơn bạn đã mua hàng của chúng tôi !");
            return View(ddhUpdate);
        }

        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            // goi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); // Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); // Địa chửi gửi
            mail.Subject = Title;  // tiêu đề gửi
            mail.Body = Content;                 // Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // host gửi của Gmail
            smtp.Port = 587;               //port của Gmail
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential
            ("mocmien1551@gmail.com","Mien12345678");//Tài khoản/password người gửi
            smtp.EnableSsl = true;   //kích hoạt giao tiếp an toàn SSL
            smtp.Send(message: mail);   //Gửi mail đi
        }
        [HttpGet]
        public ActionResult GiaoHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            // Lấy ds chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        [HttpPost]
        public ActionResult GiaoHang(DonDatHang ddh)
        {
            // Lấy dữ liệu của đơn hàng đó
            DonDatHang ddhUpdate1 = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate1.DaThanhToan = true;
           ddhUpdate1.NgayGiao = DateTime.Now;
            //ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            //ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            //ddh.TinhTrangGiaoHang=true;
            //db.DonDatHangs.Add(ddh);
            db.SaveChanges();

            // Lấy ds chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            //GuiEmail("Xác nhận đơn hàng", "nhattruongiot@gmail.com", "abc@gmail.com", "zewang.help", "Đơn hàng của bạn đã được đặt thành công cảm ơn bạn đã mua hàng của chúng tôi !");
            return View(ddhUpdate1);
            //return View(DaGiaoDaThanhToan);
        }
        [HttpGet]
        public ActionResult ChiTietDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            // Lấy ds chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        //[HttpPost]
        //public ActionResult ChiTietDonHang(DonDatHang ddh)
        //{
        //    // Lấy dữ liệu của đơn hàng đó
        //    DonDatHang ddhUpdate1 = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
        //    ddhUpdate1.DaThanhToan = true;
        //    //ddhUpdate.DaThanhToan = ddh.DaThanhToan;
        //    //ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
        //    //ddh.TinhTrangGiaoHang=true;
        //    //db.DonDatHangs.Add(ddh);
        //    db.SaveChanges();

        //    // Lấy ds chi tiết đơn hàng để hiển thị cho người dùng thấy
        //    var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
        //    ViewBag.ListChiTietDH = lstChiTietDH;
        //    //GuiEmail("Xác nhận đơn hàng", "nhattruongiot@gmail.com", "abc@gmail.com", "zewang.help", "Đơn hàng của bạn đã được đặt thành công cảm ơn bạn đã mua hàng của chúng tôi !");
        //    return View(ddhUpdate1);
        //    //return View(DaGiaoDaThanhToan);
        //}
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