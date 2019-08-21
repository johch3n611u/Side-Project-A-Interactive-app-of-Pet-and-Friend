using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;

namespace PetPet.Controllers
{
    public class REGController : Controller
    {
        petpetEntities db = new petpetEntities();
        // GET: REG
        public ActionResult MemberCreate()
        {
            ViewBag.City = db.City_list.ToList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult MemberCreate(string Email, string Pwd, string Pwd_Prompt, string Pwd_Ans,
            string Name, DateTime Birthday, bool Gender, string Phone, string City_no, HttpPostedFileBase Photo)
        {
            var clickmail = db.Member.Where(m => m.Email == Email).FirstOrDefault();
            ViewBag.City = db.City_list.ToList();

            if (clickmail == null)
            {
                try
                {
                    string subname = System.IO.Path.GetExtension(Photo.FileName).ToLower();
                    if (subname == ".jpg" || subname == ".png")
                    {
                        if (Photo.ContentLength > 0)
                        {
                            Random r = new Random();
                            string datenow = DateTime.Now.ToString().Replace("/", "").Replace("上午", "").Replace("下午", "").Replace(":", "");
                            string FileName = r.Next(1000, 9999).ToString() + datenow;

                            FileName = (FileName + System.IO.Path.GetFileName(Photo.FileName)).ToString().Replace(" ", "");
                            Photo.SaveAs(Server.MapPath("~/images/memberimg/" + FileName));
                            Member member = new Member();
                            member.Email = (Email.ToLower());
                            member.Pwd = Pwd;
                            member.Name = Name;
                            member.Birthday = Birthday;
                            member.Gender = Gender;
                            member.Phone = Phone;
                            member.City_no = Convert.ToInt16(City_no);
                            member.Mem_photo = FileName;
                            member.Enable = false;
                            member.Match_Enable = true;


                            db.Member.Add(member);
                            db.SaveChanges();
                            SendAuthMail(Email, Name);
                            TempData["msg"] = "請到信箱啟用!";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.Error = "檔案大小異常!";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Error = "圖片格式錯誤!";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "帳號已被使用!換一個看看";
                return View();
            }
            //return View();
        }

        //寄註冊確認信
        protected void SendAuthMail(string mail, string Name)
        {
            SmtpClient myMail = new SmtpClient("msa.hinet.net");

            MailAddress from = new MailAddress("petpettaiwan@gmail.com", "配配台灣");
            MailAddress to = new MailAddress(mail);
            MailMessage Msg = new MailMessage(from, to);
            Msg.Subject = "[驗證信] 歡迎加入PetPet";
            Msg.Body = Name + "<p>歡迎加入配配請點擊下方連結完成帳號啟用</p>" +
                "<a href='http://10.10.3.213/REG/Auth_Ok?mail=" + mail + "'>請點我</a>";
            Msg.IsBodyHtml = true;
            myMail.Send(Msg);
            Response.Write("<script>alert('恭喜你完成註冊，請到信箱確認')</script>");
        }
        //信箱驗證
        public ActionResult Auth_Ok(string mail)
        {
            var checkmail = db.Member.Where(m => m.Email == mail).FirstOrDefault();
            checkmail.Enable = true;
            db.SaveChanges();
            Session["semail"] = checkmail.Email;
            Session["UserName"] = checkmail.Name;
            Session["UserPhoto"] = checkmail.Mem_photo;
            TempData["msg"] = "歡迎來到配配";
            return RedirectToAction("Index", "Home");
        }
    }
}