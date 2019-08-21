using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;

namespace PetPet.Controllers
{
    public class MatchController : Controller
    {
        petpetEntities db = new petpetEntities();
        // GET: Match
        public ActionResult MatchIndex()
        {
            if (Session["semail"] != null)
            {
                ViewBag.City = db.City_list.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Matchfail()
        {
            return View();
        }

        public ActionResult Match(string semail, string City_no, string Gender)
        {
            //var a = db.Member.ToList();
            var r = new Random();
            bool gender;
            if (Gender == "小王子")
                gender = true;
            else
                gender = false;

            var city_no = Convert.ToInt32(City_no);

            var myfriendList = db.Friend.Where(m => m.Email == semail).ToList();

            //a = db.Member.Where(m => m.Email != semail && m.City == City && m.Gender == gender).ToList();
            var MatchList = (from M in db.Member
                     where M.Email != semail && M.City_no == city_no && M.Gender == gender
                     select M).ToList();

            foreach (var f in myfriendList) {
                if (MatchList.Where(m => m.Email == f.F_Email).FirstOrDefault()!=null)
                {
                    var RemoveMan = MatchList.Where(m => m.Email == f.F_Email).FirstOrDefault();

                    MatchList.Remove(RemoveMan);
                }
            }

            if (MatchList.Count == 0)
            {
                return RedirectToAction("Matchfail", new { @semail = semail });
            }

            while (MatchList.Count > 1)
            {
                MatchList.RemoveAt(r.Next(MatchList.Count));
            }
            return View(MatchList);
        }
    }
}