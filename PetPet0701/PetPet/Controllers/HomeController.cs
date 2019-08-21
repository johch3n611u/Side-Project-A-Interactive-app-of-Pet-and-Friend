using PetPet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetPet.Controllers
{
    public class HomeController : Controller
    {
        petpetEntities db = new petpetEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ViewNews()
        {
            ViewBag.Message = "Your contact page.";
            var News = db.News.ToList();
            return View(News);
        }
    }
}