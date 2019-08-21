using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;
using PetPet.ViewModel;

namespace PetPet.Controllers
{
    public class PetVarietyController : Controller
    {
        // GET: PetVariety       
        petpetEntities db = new petpetEntities();


        public ActionResult Index(int? id) //=1

        {

            if (id == null)
            {
                CVM cvm1 = new CVM()
                {
                    ptype = db.PetType.ToList(),
                    variety = db.PetVariety.ToList(),
                };

                return View(cvm1);
            }


            CVM cvm2 = new CVM()
            {
                ptype = db.PetType.ToList(),
                variety = db.PetVariety.Where(m => m.PetType_no == id).ToList()

            };

            return View(cvm2);
        }

        public ActionResult Create()
        {
            return View(db.PetType.ToList());
        }

        [HttpPost]
        public ActionResult Create(PetVariety va)
        {

            if (va == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            db.PetVariety.Add(va);
            db.SaveChanges();

            return RedirectToAction("Index", new { id = va.PetType_no });
        }


        public ActionResult Edit(int id)


        {
            var va = db.PetVariety.Where(m => m.PetVariety_no == id).FirstOrDefault();

            ViewBag.Variety_no = va.PetVariety_no;
            ViewBag.Variety_name = va.Variety_name;
            ViewBag.PetType_no = va.PetType_no;

            return View(db.PetType.ToList());
        }

        [HttpPost]
        public ActionResult Edit(int Variety_no, int PetType_no, string Variety_name)
        {
            try
            {
                var va = db.PetVariety.Where(m => m.PetVariety_no == Variety_no).FirstOrDefault();

                va.PetVariety_no = Variety_no;
                va.PetType_no = PetType_no;
                va.Variety_name = Variety_name;

                db.SaveChanges();

                return RedirectToAction("Index", new { id = va.PetType_no });

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }



        public ActionResult Delete(int id)
        {

            var petvariety = db.Pet.Where(m => m.PetVariety_no == id).FirstOrDefault();
            var va = db.PetVariety.Where(m => m.PetVariety_no == id).FirstOrDefault();

            if (petvariety == null)
            {

                db.PetVariety.Remove(va);
                db.SaveChanges();

            }
            else
            {
                TempData["message"] = "提醒您，此筆資料已在其他資料表使用，無法刪除!!";
            }

            return RedirectToAction("Index", new { id = va.PetType_no });

        }


    }
}