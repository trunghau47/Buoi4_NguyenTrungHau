using Buoi4_NguyenTrungHau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ViewData["Error"] = "So Luong Mua Qua Lon!";
            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = ToSoLuongSanPham();
            ViewData["Error"] = "So Luong Mua Qua Lon!";
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
                if(sanpham.iSoluong > sach.soluongton)
                {
                    ViewData["Error"] = "So Luong Mua Qua Lon!";
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
    }
}