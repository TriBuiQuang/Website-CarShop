using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Car.Models;

using PagedList;
using PagedList.Mvc;

namespace Car.Controllers
{
    public class CarController : Controller
    {
        // GET: Car
       
        dbCAR2DataContext data = new dbCAR2DataContext();
        private List<Xe> Layxemoi(int count)
        {
            return data.Xes.OrderByDescending(a => a.NamSanXuat).Take(count).ToList();
        }
        private List<SanPham> Layspmoi(int count)
        {
            return data.SanPhams.OrderByDescending(b => b.TenSP).Where(b => b.Xoa == false).Take(count).ToList();
        }

        public ActionResult Index(int ? page,VIEW v)
        {
            if (Session["VisitorsCount"] != null)
            {
                
                DateTime today = DateTime.Now.Date;
                var v1 = data.VIEWs.Where(a => a.Ngay == today).FirstOrDefault();
                if (v1 == null)
                {
                    v.Ngay = today;
                    v.View1 += 1;
                    data.VIEWs.InsertOnSubmit(v);
                    data.SubmitChanges();
                }
                else
                {
                    VIEW sp = data.VIEWs.SingleOrDefault(n => n.Ngay == today);
                    sp.View1 += 1;
                    UpdateModel(sp);
                    data.SubmitChanges();
                }
            }
            int pageSize = 8;
            int pageNum = (page ?? 1);

            var xemoi = Layxemoi(10);
            return View(xemoi.ToPagedList(pageNum,pageSize));
        }
        public ActionResult Index2(int ? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);

            var xemoi = Layxemoi(10);
            return View(xemoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Index3(int ? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);

            var ptmoi = Layspmoi(10);
            return View(ptmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SPSXGia(int? page)
        {
            
                int pageNumber = (page ?? 1);
                int pageSize = 7;
            var ptmoi = Layspmoi(10);
            return View(ptmoi.OrderBy(n => n.GiaBan).Where(n=>n.GiaBan<50).ToPagedList(pageNumber, pageSize));
            
        }
        public ActionResult Details2(int id)
        {
            var sp = from s in data.SanPhams
                     where s.MaSP == id
                     select s;
            return View(sp.Single());
        }
        public ActionResult Details1(int id)
        {
            var xe = from x in data.Xes
                     where x.Maxe == id
                     select x;
            return View(xe.Single());
        }
        public ActionResult Intro()
        {
            return View();
        }
        public ActionResult Search(string searchTerm)
        {
            var xes = from b in data.Xes select b;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                xes = data.Xes.Where(b => b.Tenxe.Contains(searchTerm));
            }
            ViewBag.SearchTerm = searchTerm;
            return View(xes);
        }
    }
}