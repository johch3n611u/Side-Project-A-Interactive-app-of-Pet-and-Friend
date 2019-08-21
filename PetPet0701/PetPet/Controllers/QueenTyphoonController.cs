using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;

namespace PetPet.Controllers
{
    public class QueenTyphoonController : Controller
    {
        private petpetEntities db = new petpetEntities();

        public ActionResult QueenTyphoonLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QueenTyphoonLogin(string Admin_account, string Admin_pwd, int? VerificationImgNumberInput, string AutoLogin)
        {
            var Admin = db.Admin.Where(m => m.Admin_account == Admin_account && m.Admin_pwd == Admin_pwd).FirstOrDefault();

            bool VerificationImgNumberTorF = VerificationImgNumberInput == Convert.ToInt32(Session["VerificationImgNumber"]);

            if (Admin == null)

            {

                var Errors_Numner_Value = Convert.ToInt32(Session["ErrorsNumner"]);

                Errors_Numner_Value += 1;

                Session.Add("ErrorsNumner", Errors_Numner_Value);

                int Pre_Errors_Numner_Value = 4;

                Errors_Numner_Value = Pre_Errors_Numner_Value - Errors_Numner_Value;

                if (Errors_Numner_Value <= 0)
                {


                    ViewBag.Message = "<div class='alert alert-danger' role='alert''><img src='/QueenTyphoonContent/VerificationImg.ashx'/><label for='Errors_Numner_Input' class='sr-only'>請輸入上圖內數字:</label><input id='VerificationImgNumberInput' name='VerificationImgNumberInput' class='form-control' placeholder='圖形驗證碼' required='required' autofocus=''><strong>" + Session["VerificationImgError"] + "</strong></div>";
                    //Postback 預備 <a class='btn btn-link' href='/QueenTyphoon/QueenTyphoonLogin' title='更換驗證碼圖片'><img src='../QueenTyphoonContent/libraries/ic_refresh_48px-128.png'/></a><br>


                    if (VerificationImgNumberTorF | VerificationImgNumberInput != null)
                    {
                        Session["ErrorsNumner"] = 0;

                        Session["VerificationImgError"] = "驗證碼正確!<br>";

                        return View();

                    }
                    else if (VerificationImgNumberTorF == false | VerificationImgNumberInput != null)
                    {
                        Session["VerificationImgError"] = "驗證碼錯誤!<br>";

                        return View();
                    }

                    return View();

                }

                ViewBag.Message = "'<div class='alert alert-danger' role ='alert'' ><strong>加油呦!!帳號或密碼錯誤!!<br>您還剩" + Errors_Numner_Value + "次機會</strong></div>";

                return View();

            }

            else if (((Admin != null) & VerificationImgNumberTorF) | ((Admin != null) & (VerificationImgNumberInput == null)))
            {



                Session["VerificationImgNumber"] = null;

                Session["WelCome"] = "美好的一天，很高興見到您，" + Admin.Admin_no + "號管理員!!!";

                Session["Admin"] = Admin.Admin_no;

                // Test Cookie AutoLogin
                // https://dotblogs.com.tw/shadow/2011/12/15/62306
                // https://www.cnblogs.com/vebest/archive/2011/08/31/2161326.html
                // https://www.itread01.com/article/1536733191.html
                // https://blog.csdn.net/chengly0129/article/details/7847889
                // http://mirlab.org/jang/books/javascript/cookie01.asp?title=9-1%20%C5%AA%BCg%A4p%BB%E6%B0%AE

                var AutoLoginTorF = AutoLogin; //string or null

                HttpCookie AutoLoginCookie = new HttpCookie("AutoLoginCookie")
                {
                    Expires = DateTime.Now.AddDays(2)
                };

                // AutoLoginTorF = T

                if (AutoLoginTorF != null)
                {

                    AutoLoginCookie.Values.Add("Admin_account_Cookie", Admin_account);

                    AutoLoginCookie.Values.Add("Admin_pwd_Cookie", Admin_pwd);

                    AutoLoginCookie.Values.Add("AutoLogin_status", "checked");

                    Response.Cookies.Add(AutoLoginCookie);

                    return RedirectToAction("QueenTyphoonIndex");

                }

                //AutoLoginTorF = F
                else
                {

                    AutoLoginCookie.Values.Remove("Admin_account_Cookie");

                    AutoLoginCookie.Values.Remove("Admin_pwd_Cookie");

                    AutoLoginCookie.Values.Remove("AutoLogin_status");

                    Response.Cookies.Add(AutoLoginCookie);

                    return RedirectToAction("QueenTyphoonIndex");
                }

            }

            ViewBag.VerificationImgErrorbutAdminTrue = "感謝您的嘗試!!<br>驗證碼錯誤但帳號密碼已答對<br>請再次輸入帳號密碼進入!!";

            return View();

        }

        public ActionResult QueenTyphoonIndex()
        {

            if (Session["Admin"] == null)
            {


                return RedirectToAction("QueenTyphoonLogin", "QueenTyphoon");

            }

            return View("QueenTyphoonIndex", "_LayoutAdmin");

        }

        public ActionResult QueenTyphoonLogout()
        {
            Session.Clear();
            return RedirectToAction("QueenTyphoonLogin");
        }





    }
}