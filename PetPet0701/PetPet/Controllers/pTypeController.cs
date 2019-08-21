using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;

namespace PetPet.Controllers
{
    public class PTypeController : Controller
    {
        petpetEntities db = new petpetEntities();

        public ActionResult Index()
        {
            return View(db.PetType.ToList());
        }

        public ActionResult Delete(int id)
        {
            var petvariety = db.PetVariety.Where(m => m.PetType_no == id).FirstOrDefault();

            if (petvariety == null)
            {
                var pettype = db.PetType.Where(m => m.PetType_no == id).FirstOrDefault();

                db.PetType.Remove(pettype);
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
        public ActionResult Create(PetType type1)
        {

            db.PetType.Add(type1);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {

            return View(db.PetType.Where(m => m.PetType_no == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(int PetType_no, string PetType_name)
        {
            try
            {

                var ptype = db.PetType.Where(m => m.PetType_no == PetType_no).FirstOrDefault();

                ptype.PetType_name = PetType_name;

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