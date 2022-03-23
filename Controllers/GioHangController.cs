using Buoi4_NguyenTrungHau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Buoi4_NguyenTrungHau.Controllers
{
    public class GioHangController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if(lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }

        public ActionResult ThemGioHang (int id, string strURL)
        {
            List<GioHang> lstGioHang = Laygiohang();
            GioHang sanpham = lstGioHang.Find(n => n.masach == id);
            if(sanpham==null)
            {
                sanpham = new GioHang(id);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }    
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }    
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                tsl = lstGioHang.Sum(n => n.iSoluong);
            }
            return tsl;
        }

        private int ToSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if(lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }

        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if(lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.dThanhtien);
            }
            return tt;
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = ToSoLuongSanPham();
            ViewBag.Employee = "";

            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = ToSoLuongSanPham();

            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.masach == id);
                if(sanpham != null){
                    lstGiohang.RemoveAll(n => n.masach == id);
                return RedirectToAction("GioHang");
                }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhapGioHang (int id, FormCollection collection)
        {
            Sach sach = data.Saches.First(n => n.masach == id);
            List<GioHang> lstGiohang = Laygiohang();
            GioHang sanpham = lstGiohang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                if(int.Parse(collection["txtSolg"].ToString()) > sach.soluongton)
                {

                    ViewBag.Employee = "Sai Rồi";

                    ViewBag.ErrorMessage = "Số Lượng Nhập Quá Lớn";
                    return RedirectToAction("GioHang");

                }
                else
                {
                    sanpham.iSoluong = int.Parse(collection["txtSolg"].ToString());
                }
            }
            return RedirectToAction("GioHang");
        }



        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("GioHang");
        }

        //public ActionResult DatHang()
        //{
        //    List<GioHang> list = Laygiohang();
        //    foreach(var item in list)
        //    {
        //        var sach = data.Saches.FirstOrDefault(m => m.masach == item.masach);
        //        sach.soluongton -= item.iSoluong;
        //    }
        //    data.SubmitChanges();
        //    list.Clear();
        //    return RedirectToAction("GioHang");
        //}
        [HttpGet]

        public ActionResult DatHang()
        {
            if(Session["TaiKhoan"]== null || Session["TaiKhoan"].ToString()=="")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if(Session["GioHang"]==null)
            {
                return RedirectToAction("Index", "Sach");
            }
            List<GioHang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = ToSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Sach s = new Sach();
            List<GioHang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            //if (DateTime.Parse(ngaygiao) < DateTime.Now)
            //{
            //    ViewData["Date"] = "Ngày giao phải lớn hơn ngày hiện tại!";
            //} 
            //else
            //{
                
            //}    
            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = item.masach;
                ctdh.soluong = item.iSoluong;
                s = data.Saches.Single(n => n.masach == item.masach);
                s.soluongton -= ctdh.soluong;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
                try
                {
                    if (ModelState.IsValid)
                    {
                        
                        var senderEmail = new MailAddress("haunguyenaaaa6@gmail.com", "Cửa Hàng Sách Hutech");
                        var receiverEmail = new MailAddress(kh.email, "Receiver");
                        var password = "hau121pk";
                        var sub = "Xác Nhận Mua Hàng Thành Công";
                        var body = "Đơn hàng " + ctdh.madon + " đang được giao đến bạn \nCảm ơn bạn";
                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            Subject = sub,
                            Body = body
                        })
                        {
                            smtp.Send(mess);
                        }

                    }
                }
                catch (Exception ex)
                {
                    return HttpNotFound();
                }
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            
            return RedirectToAction("XacNhanDonHang","GioHang");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }

        //public ActionResult SendMail()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult SendMail(KhachHang receiver, string subject, string message)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var senderEmail = new MailAddress("haunguyenaa6@gmail.com", "Nguyen Trung Hau");
        //            var receiverEmail = new MailAddress(receiver.email, "Receiver");
        //            var password = "hau121pk";
        //            var sub = "Xác Nhận Mua Hàng Thành Công";
        //            var body ="Đơn hàng của bạn đang được giao đến bạn \n Cảm ơn bạn";
        //            var smtp = new SmtpClient
        //            {
        //                Host = "smtp.gmail.com",
        //                Port = 587,
        //                EnableSsl = true,
        //                DeliveryMethod = SmtpDeliveryMethod.Network,
        //                UseDefaultCredentials = false,
        //                Credentials = new NetworkCredential(senderEmail.Address, password)
        //            };
        //            using (var mess = new MailMessage(senderEmail, receiverEmail)
        //            {
        //                Subject = sub,
        //                Body = body
        //            })
        //            {
        //                smtp.Send(mess);
        //            }
                   
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return HttpNotFound();
        //    }
        //    return RedirectToAction("XacNhanGioHang");
        //}

    }
}
