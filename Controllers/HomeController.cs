using Buoi4_NguyenTrungHau.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buoi4_NguyenTrungHau.Controllers
{
    public class HomeController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var all_sach = (from s in data.Saches select s).OrderBy(m => m.masach);
            int pageSize = 3;
            int pageNum = page ?? 1;
            return View(all_sach.ToPagedList(pageNum, pageSize));
        }

     
    }
}