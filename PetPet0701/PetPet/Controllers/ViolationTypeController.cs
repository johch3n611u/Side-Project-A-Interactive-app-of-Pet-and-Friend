using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;

namespace PetPet.Controllers
{
    public class ViolationTypeController : Controller
    {
        petpetEntities db = new petpetEntities();

        public ActionResult Index()
        {
            return View(db.Violation_type.ToList());
        }


        public ActionResult Delete(int id)
        {
            var vtypeno = db.Report.Where(m => m.VType_no == id).FirstOrDefault();

            if (vtypeno == null)
            {
                var vtype = db.Violation_type.Where(m => m.VType_no == id).FirstOrDefault();

                db.Violation_type.Remove(vtype);
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
        public ActionResult Create(Violation_type type1)
        {

            db.Violation_type.Add(type1);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {

            return View(db.Violation_type.Where(m => m.VType_no == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(int? VType_no, string VType_name, int Freeze_day)
        {
            try
            {

                var vtype = db.Violation_type.Where(m => m.VType_no == VType_no).FirstOrDefault();

                vtype.VType_name = VType_name;
                vtype.Freeze_day = Freeze_day;

                db.SaveChanges();

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }
    }
}