using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;

namespace PetPet.Controllers
{
    public class MailController : Controller
    {
        petpetEntities db = new petpetEntities();
        // GET: Mail
        public ActionResult MailIndex()
        {
            string semail = Session["semail"].ToString();
            var Mail = db.Mail.Where(m => m.Re_email == semail).ToList();
            ViewBag.Friends = db.Friend.Where(m => m.Email == semail && m.Add_ststus == true).ToList();
            return View(Mail);
        }

        public ActionResult CreateMail(string Receiver, string Mail_title, string Mail_content, IEnumerable<HttpPostedFileBase> Mail_photo)
        {
            string semail = Session["semail"].ToString();

            Mail_photo mail_photo = new Mail_photo();
            Mail mail = new Mail();

            //將信件內容寫入創建的mail後再新增進資料庫
            mail.Email = semail;
            mail.Re_email = Receiver;
            mail.Mail_tital = Mail_title;
            mail.Mail_content = Mail_content;
            mail.Send_time = DateTime.Now;

            db.Mail.Add(mail);

            Random r = new Random();
            string datenow = DateTime.Now.ToString().Replace("/", "").Replace("上午", "").Replace("下午", "").Replace(":", "");
            string fileName = r.Next(1000, 9999).ToString() + datenow;
            //檢查是否有圖片
            if (Mail_photo != null)
            {
                foreach (var file in Mail_photo)
                {
                    if (file != null)
                    {
                        string subname = Path.GetExtension(file.FileName).ToLower();
                        if (subname == ".jpg" || subname == ".png")
                        {
                            if (file.ContentLength > 0)
                            {
                                fileName += Path.GetFileName(file.FileName);
                                string path = Path.Combine(Server.MapPath("~/images/mailimg/"), fileName);
                                file.SaveAs(path);
                                mail_photo.Mail_Photo1 = fileName;
                                db.Mail_photo.Add(mail_photo);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            db.SaveChanges();

            return RedirectToAction("MailIndex");
        }


        //讀信
        public ActionResult ReadMail(int id)
        {
            var Mail = db.Mail.Where(m => m.Mail_no == id).FirstOrDefault();
            return View(Mail);
        }
        //刪除信
        public ActionResult DeleteMail(IEnumerable<int> DelMail, int num)
        {
            //檢查有沒有信
            if (DelMail == null)
            {
                TempData["msg"] = "查無信件";
                if (num == 0)
                    return RedirectToAction("MailIndex");
                else
                    return RedirectToAction("CopyMailIndex");
            }

            //檢查是否有多張圖片
            foreach (int delmail in DelMail)
            {
                var MailImg = db.Mail_photo.Where(m => m.Mail_no == delmail).ToList();
                var Mail = db.Mail.Where(m => m.Mail_no == delmail).FirstOrDefault();
                foreach (var delimg in MailImg)
                {
                    string filename = delimg.Mail_Photo1;
                    System.IO.File.Delete(Server.MapPath("~/images/mailimg/") + filename);
                    db.Mail_photo.Remove(delimg);
                }
                db.Mail.Remove(Mail);
                db.SaveChanges();
            }

            if (num == 0)
                return RedirectToAction("MailIndex");
            else
                return RedirectToAction("CopyMailIndex");
        }

        //寄件備份
        public ActionResult CopyMailIndex()
        {
            string semail = Session["semail"].ToString();
            var Mail = db.Mail.Where(m => m.Email == semail).ToList();
            ViewBag.Friends = db.Friend.Where(m => m.Email == semail && m.Add_ststus == true).ToList();
            return View(Mail);
        }
    }
}