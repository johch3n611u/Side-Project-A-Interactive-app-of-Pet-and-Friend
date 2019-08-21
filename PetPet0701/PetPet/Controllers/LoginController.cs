using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;

namespace PetPet.Controllers
{
    public class LoginController : Controller
    {
        petpetEntities db = new petpetEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection post)//擷取信箱和密碼
        {
            string fEmail = (post["fEmail"].ToLower());
            string fPwd = post["fPwd"];
            string fAuth = post["fAuth"];

            var result = db.Member.Where(m => m.Email == fEmail && m.Pwd == fPwd).FirstOrDefault();
            if (fAuth != Session["VerfiNumber"].ToString())
            {
                ViewBag.Msg = "驗證碼錯誤";
                return View();
            }

            if (result == null)
            {
                 ViewBag.Msg = "帳號密碼錯誤";
                 return View();
                 //return RedirectToAction("LoidnIndex");
            }
            else if (result.Enable == false)
            {
                ViewBag.Msg = "帳號未啟用";
                return View();
            }
            else
            {
                Session["semail"] = fEmail;
                Session["UserName"] = result.Name;
                Session["UserPhoto"] = result.Mem_photo;
                return RedirectToAction("Index", "Home");
            }
            //if ( fAuth == Session["Number"].ToString())
            //{
            //    ViewBag.Msg = "驗證碼正確";
                
            //}
            //else
            //{
            //    ViewBag.Msg = "驗證碼不正確";
            //    return View();
            //}
        }
        
        public ActionResult forgetpassword()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult forgetpassword(FormCollection post)
        {
            string fEmail = (post["fEmail"].ToLower());
            string fAuth = post["fAuth"];
            
            var result = db.Member.Where(m => m.Email == fEmail).FirstOrDefault();
            if (fAuth != Session["VerfiNumber"].ToString())
            {
                ViewBag.Msg = "驗證碼錯誤";
                return View();
            }
            result.Enable = false;
            db.SaveChanges();
            NewPwdMail(fEmail);
            TempData["msg"] = "請到信箱啟用!";
            return RedirectToAction("Index", "Home");
        }
        //寄改密碼的信
        protected void NewPwdMail(string mail)
        {
            SmtpClient myMail = new SmtpClient("msa.hinet.net");
            MailAddress from = new MailAddress("petpettaiwan@gmail.com", "配配台灣");
            MailAddress to = new MailAddress(mail);
            MailMessage Msg = new MailMessage(from, to);
            Msg.Subject = "[忘記密碼] PetPet修改密碼";
            Msg.Body = "<p>請盡速點擊下方連結修改新密碼</p>" +
                "<a href='http://10.10.3.213/Login/GetNewPwd?mail=" + mail + "'>請點我</a>";
            Msg.IsBodyHtml = true;
            myMail.Send(Msg);
            Response.Write("<script>alert('恭喜你完成註冊，請到信箱確認')</script>");
        }
        public ActionResult GetNewPwd(string mail)
        {
            ViewBag.mail = mail;
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GetNewPwd(string mail,string newpwd,string Pwd2)
        {
            var member = db.Member.Where(m => m.Email == mail).FirstOrDefault();
            if (member == null)
            {
                TempData["msg"] = "發生不明錯誤!!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (newpwd == Pwd2)
                {
                    member.Pwd = newpwd;
                    member.Enable = true;
                    db.SaveChanges();
                    TempData["msg"] = "修改成功! 請用新密碼登入";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["msg"] = "兩次密碼不相同!請重新輸入";
                    ViewBag.Msg = "兩次密碼不相同!請重新輸入";
                    ViewBag.mail = mail;
                    return View();
                }
            }            
        }
    }
}