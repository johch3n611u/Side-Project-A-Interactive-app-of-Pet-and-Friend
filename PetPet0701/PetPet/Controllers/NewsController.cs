using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;
using System.IO;

namespace PetPet.Controllers
{
    public class NewsController : Controller
    {
        petpetEntities db = new petpetEntities();

        // GET: News
        public ActionResult Index(int? overdue)
        {


            var overdueDate = DateTime.Today;

            //未過期資料
            if (overdue == 0)
            {
                var news = db.News.Where(m => m.N_post_deadline.CompareTo(overdueDate) >= 0).OrderByDescending(m => m.N_post_time).ToList();
                TempData["color"] = "0";
                return View(news);
            }

            //已過期資料
            if (overdue == 1)
            {
                var news = db.News.Where(m => m.N_post_deadline.CompareTo(overdueDate) < 0).OrderByDescending(m => m.N_post_time).ToList();
                TempData["color"] = "1";
                return View(news);
            }

            //所有資料
            return View(db.News.OrderByDescending(m => m.N_post_time).ToList());

        }

        public ActionResult Delete(int id)
        {
            var news = db.News.Where(m => m.News_no == id).FirstOrDefault();

            if (news.N_photo != "")
            {
                string fileName = news.N_photo;
                System.IO.File.Delete(Server.MapPath("~/NewsImages/" + fileName));
            }

            db.News.Remove(news);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {

            /*ViewBag.Admin_no = 1;*/ // 之後要抓session值 
            ViewBag.Admin_no = Session["Admin"];
            ViewBag.DateS = DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.DateE = DateTime.Today.AddDays(14).ToString("yyyy-MM-dd");


            return View("Create");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(News news, HttpPostedFileBase N_photo)
        //public ActionResult Create(string N_tital, string N_content, DateTime N_post_time, DateTime N_post_deadline, int Admin_no, HttpPostedFileBase N_photo)
        {
            try
            {
                //處理商品圖片檔
                //string fileName = "";
                //string subname = Path.GetExtension(N_photo.FileName);
                //if (subname == ".jpg" || subname == ".png")
                //{
                //    if (N_photo.ContentLength > 0)
                //    {
                //        fileName = Path.GetFileName(N_photo.FileName);
                //        N_photo.SaveAs(Server.MapPath("~/NewsImages/" + fileName));
                //    }
                //}

                string fileName = "";

                if (N_photo != null)
                {
                    if (N_photo.ContentLength > 0)
                    {
                        string subname = Path.GetExtension(N_photo.FileName);
                        if (subname == ".jpg" || subname == ".png")
                        {
                            Random r = new Random();
                            string datenow = DateTime.Now.ToString().Replace("/", "").Replace("上午", "").Replace("下午", "").Replace(":", "");
                            fileName = datenow + r.Next(1000, 9999).ToString();

                            fileName += Path.GetFileName(N_photo.FileName);
                            string path = Path.Combine(Server.MapPath("~/NewsImages/"), fileName);
                            N_photo.SaveAs(path);

                        }
                    }

                }


                news.N_photo = fileName;

                db.News.Add(news);
                db.SaveChanges();

                //News news = new News();

                //news.N_tital = N_tital;
                //news.N_content = N_content;
                //news.N_photo = fileName;
                //news.N_post_time = N_post_time;
                //news.N_post_deadline = N_post_deadline;
                //news.Admin_no = Admin_no;             

                //db.News.Add(news);
                //db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var news = db.News.Where(m => m.News_no == id).FirstOrDefault();

            ViewBag.Admin_no = 2; // 之後要抓session值 
            ViewBag.Admin_no = Session["Admin"];
            ViewBag.DateS = news.N_post_time.ToString("yyyy-MM-dd");
            ViewBag.DateE = news.N_post_deadline.ToString("yyyy-MM-dd");

            return View(news);
        }

        [HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Edit(News news, string oldImg, HttpPostedFileBase fImg)
        public ActionResult Edit(int News_no, string N_tital, string N_content, DateTime N_post_time, DateTime N_post_deadline, int Admin_no, HttpPostedFileBase N_photo, string oldImg)
        {

            try
            {
                string fileName = oldImg; ;

                if (N_photo != null)
                {
                    if (N_photo.ContentLength > 0)
                    {
                        string subname = Path.GetExtension(N_photo.FileName);
                        if (subname == ".jpg" || subname == ".png")
                        {
                            if (oldImg != "")
                            {
                                System.IO.File.Delete(Server.MapPath("~/NewsImages/" + oldImg));
                            }

                            Random r = new Random();
                            string datenow = DateTime.Now.ToString().Replace("/", "").Replace("上午", "").Replace("下午", "").Replace(":", "");
                            fileName = datenow + r.Next(1000, 9999).ToString();

                            fileName += Path.GetFileName(N_photo.FileName);
                            string path = Path.Combine(Server.MapPath("~/NewsImages/"), fileName);
                            N_photo.SaveAs(path);

                        }
                    }

                }

                var news = db.News.Where(m => m.News_no == News_no).FirstOrDefault();

                news.N_tital = N_tital;
                news.N_content = N_content;
                news.N_photo = fileName;
                news.N_post_time = N_post_time;
                news.N_post_deadline = N_post_deadline;
                news.Admin_no = Admin_no;

                db.SaveChanges();

                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        public ActionResult Details(int id)
        {

            return View(db.News.Where(m => m.News_no == id).FirstOrDefault());
        }
    }
}