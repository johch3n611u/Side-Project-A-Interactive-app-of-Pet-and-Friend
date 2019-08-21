using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;

namespace PetPet.Controllers
{
    public class AdminController : Controller
    {
        petpetEntities db = new petpetEntities();

        public ActionResult Index()
        {
            //if ((int)Session["Admin"] == 1) {

            //    return View(db.Admin.ToList());
            //}
            //    var id = Convert.ToInt32(Session["Admin"]);
            //    var r = db.Admin.Where(m => m.Admin_no == id).ToList();

            //    return View(r);

            return View(db.Admin.ToList());

        }


        public ActionResult Delete(int id)
        {
            var newsadmin = db.News.Where(m => m.Admin_no == id).FirstOrDefault();
            if (newsadmin == null)
            {
                var admin = db.Admin.Where(m => m.Admin_no == id).FirstOrDefault();

                db.Admin.Remove(admin);
                db.SaveChanges();
            }
            else
            {
                TempData["message"] = "提醒您，此筆資料已在其他資料表使用，無法刪除!!";
            }

            return RedirectToAction("Index");

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Admin admin)
        {

            db.Admin.Add(admin);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var admin = db.Admin.Where(m => m.Admin_no == id).FirstOrDefault();

            return View(admin);
        }

        [HttpPost]
        public ActionResult Edit(int Admin_no, string Admin_account, string Admin_pwd)
        {
            try
            {
                var admin = db.Admin.Where(m => m.Admin_no == Admin_no).FirstOrDefault();

                admin.Admin_no = Admin_no;
                admin.Admin_account = Admin_account;
                admin.Admin_pwd = Admin_pwd;

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            if ((int)Session["Admin"] == 1) { return RedirectToAction("Index"); }


            return RedirectToAction("Index", "BgStatisticalAnalysis");

        }

        //public ActionResult ChangePassword(int id)
        //{
        //    var admin = db.Admin.Where(m => m.Admin_no == id).FirstOrDefault();

        //    return View(admin);
        //}

        //[HttpPost]
        //public ActionResult ChangePassword(int Admin_no, string Admin_account, string Admin_pwd)
        //{
        //    try
        //    {
        //        var admin = db.Admin.Where(m => m.Admin_no == Admin_no).FirstOrDefault();

        //        admin.Admin_no = Admin_no;
        //        admin.Admin_account = Admin_account;
        //        admin.Admin_pwd = Admin_pwd;

        //        db.SaveChanges();


        //        return RedirectToAction("QueenTyphoonIndex", "QueenTyphoon");

        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Error = ex.Message;
        //    }

        //    return View();
        //}
    }
}