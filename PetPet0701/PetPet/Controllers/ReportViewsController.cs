using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;
using PetPet.ViewModel;



namespace PetPet.Controllers
{
    public class ReportViewsController : Controller
    {
        private petpetEntities db = new petpetEntities();

        public ActionResult Index(int? CommandOrdered)
        {

            //時間到解鎖迴圈

            var Maxlength = (db.Report.ToList()).Count();

            for (var i = 0; i < Maxlength; i++)
            {

                var Outofprison = db.Report.ToList()[i];
                //var Outofprison = db.Report.Where(m => m.Report_no == Report_no).FirstOrDefault();

                if (Outofprison.Unfreeze_date != null)
                {

                    DateTime start = Convert.ToDateTime(DateTime.Today);

                    DateTime end = Convert.ToDateTime(Outofprison.Unfreeze_date);

                    TimeSpan ts = end.Subtract(start);

                    int dayCount = ts.Days;

                    if (dayCount <= 0)
                    {

                        var PStatus = db.Report.ToList()[i];
                        //if (PStatus.Processing_stsus == true) { PStatus.Processing_stsus = false; } else { PStatus.Processing_stsus = false; }
                        PStatus.Unfreeze_date = null;

                        var Post_no_Outofprison = PStatus.Post_no;

                        var Email_Outofprison = db.Post.Where(m => m.Post_no == Post_no_Outofprison).First().Post_Email;
                        Post SeachPost_no = db.Post.Where(m => m.Post_no == Post_no_Outofprison).FirstOrDefault();
                        if (SeachPost_no.Post_Enable == false) { SeachPost_no.Post_Enable = true; } else { SeachPost_no.Post_Enable = true; }

                        db.SaveChanges();
                    }
                }
            }

            //自動排程 https://social.msdn.microsoft.com/Forums/zh-TW/5eecd17e-3c3f-4e91-b62c-950fdca36d38/355312183924590271712148720197234502639922519348921996820491?forum=236

            //排序查詢
            if (CommandOrdered == 1)
            {

                ViewBag.CommandOrderedBag = CommandOrdered;

                return View(db.ReportView.Where(m => m.Processing_stsus == false).ToList());
            }

            else if (CommandOrdered == 2)
            {

                ViewBag.CommandOrderedBag = CommandOrdered;

                return View(db.ReportView.Where(m => m.Processing_stsus == true).ToList());
            }

            return View(db.ReportView.ToList());
        }










        public ActionResult IndexOrdered()
        {

            return RedirectToAction("Index", new { CommandOrdered = 1 });
            //return RedirectToAction("Index", new { Email = Email2, Report_no = Report_no, Post_no = Post_no });
        }

        public ActionResult IndexOrdered2()
        {

            return RedirectToAction("Index", new { CommandOrdered = 2 });
            //return RedirectToAction("Index", new { Email = Email2, Report_no = Report_no, Post_no = Post_no });
        }









        public ActionResult Reportprocessing(int Post_no, string Email, int? Report_no)
        {         //放一個session處理值

            int Post_no_Outofprison = Post_no;
            string Email_Outofprison = Email;
            int? Report_no_Outofprison = Report_no;



            // 時間到解鎖

            //var Outofprison = db.Report.Where(m => m.Report_no == Report_no).FirstOrDefault();

            //DateTime start = Convert.ToDateTime(DateTime.Today);

            //DateTime end = Convert.ToDateTime(Outofprison.Unfreeze_date); 

            //TimeSpan ts = end.Subtract(start);

            //int dayCount = ts.Days;

            //if (dayCount <= 0) {

            //Post_no_Outofprison = db.Post.Where(m => m.Post_Email == Email_Outofprison).First().Post_no;
            //Member SeachEmail = db.Member.Where(m => m.Email == Email_Outofprison).FirstOrDefault();
            //if (SeachEmail.Enable == true) { SeachEmail.Enable = false; } else { SeachEmail.Enable = false; }

            //Email_Outofprison = db.Post.Where(m => m.Post_no == Post_no_Outofprison).First().Post_Email;
            //Post SeachPost_no = db.Post.Where(m => m.Post_no == Post_no_Outofprison).FirstOrDefault();
            //if (SeachPost_no.Post_Enable == true) { SeachPost_no.Post_Enable = false; } else { SeachPost_no.Post_Enable = false; }

            //Email_Outofprison = db.Report.Where(m => m.Report_no == Report_no).First().Email;
            //Post_no_Outofprison = db.Report.Where(m => m.Report_no == Report_no).First().Post_no;
            //Post_img SeachPost_img = db.Post_img.Where(m => m.Photo_no == Post_no_Outofprison).FirstOrDefault();
            //if (SeachPost_img.PImg_Enable == true) { SeachPost_img.PImg_Enable = false; } else { SeachPost_img.PImg_Enable = false; }

            //    var PStatus = db.Report.Where(m => m.Report_no == Report_no).FirstOrDefault();
            //    if (PStatus.Processing_stsus == true) { PStatus.Processing_stsus = false; } else { PStatus.Processing_stsus = false; }
            //    db.SaveChanges();

            //}







            ViewModel.ReportViewModel petpettest = new ViewModel.ReportViewModel()
            {
                RPost = db.Post.Where(m => m.Post_no == Post_no).ToList(),
                RMember = db.Member.Where(m => m.Email == Email).ToList(),
                RPost_img = db.Post_img.Where(m => m.Post_no == Post_no).ToList(),
                RViolation_type = db.Violation_type.ToList(),
                RReportView = db.ReportView.Where(m => m.Report_no == Report_no).ToList(),
            };

            ViewBag.SelectViewBag = db.Report.Where(m => m.Report_no == Report_no).First().VType_no;

            ViewBag.Report_no = Report_no;
            return View(petpettest);
        }
        public ActionResult Reportreallyprocessing2(string Email, int? RCommand, int? Report_no)
        {


            try
            {


                switch (RCommand)
                { //email要將%轉為@
                    case 1:
                        var Post_no = db.Report.Where(m => m.Report_no == Report_no).First().Post_no;
                        Member SeachEmail = db.Member.Where(m => m.Email == Email).FirstOrDefault();
                        if (SeachEmail.Enable == true) { SeachEmail.Enable = false; } else { SeachEmail.Enable = false; }
                        db.SaveChanges();
                        return RedirectToAction("Reportprocessing", new { Email = Email, Report_no = Report_no, Post_no = Post_no });
                    case 2:
                        var Post_no2 = db.Report.Where(m => m.Report_no == Report_no).First().Post_no;
                        Member SeachEmail2 = db.Member.Where(m => m.Email == Email).FirstOrDefault();
                        if (SeachEmail2.Enable == false) { SeachEmail2.Enable = true; } else { SeachEmail2.Enable = true; }
                        db.SaveChanges();
                        return RedirectToAction("Reportprocessing", new { Email = Email, Report_no = Report_no, Post_no = Post_no2 });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }



            return View();
        }
        public ActionResult Reportreallyprocessing3(int? Post_no, int? RCommand, int? Report_no)
        {



            try
            {



                switch (RCommand)
                {
                    case 3:
                        var Email = db.Post.Where(m => m.Post_no == Post_no).First().Post_Email;
                        Post SeachPost_no = db.Post.Where(m => m.Post_no == Post_no).FirstOrDefault();
                        if (SeachPost_no.Post_Enable == true) { SeachPost_no.Post_Enable = false; } else { SeachPost_no.Post_Enable = false; }
                        db.SaveChanges();

                        return RedirectToAction("Reportprocessing", new { Email = Email, Report_no = Report_no, Post_no = Post_no });
                    case 4:
                        var Email2 = db.Post.Where(m => m.Post_no == Post_no).First().Post_Email;
                        Post SeachPost_no2 = db.Post.Where(m => m.Post_no == Post_no).FirstOrDefault();
                        if (SeachPost_no2.Post_Enable == false) { SeachPost_no2.Post_Enable = true; } else { SeachPost_no2.Post_Enable = true; }
                        db.SaveChanges();

                        return RedirectToAction("Reportprocessing", new { Email = Email2, Report_no = Report_no, Post_no = Post_no });
                }




            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        public ActionResult Reportreallyprocessing4(int Photo_no, int? RCommand, int? Report_no)
        {



            try
            {



                switch (RCommand)
                {
                    case 5:
                        var Email = db.Report.Where(m => m.Report_no == Report_no).First().Email;
                        var Post_no = db.Report.Where(m => m.Report_no == Report_no).First().Post_no;
                        Post_img SeachPost_img = db.Post_img.Where(m => m.Photo_no == Photo_no).FirstOrDefault();
                        if (SeachPost_img.PImg_Enable == true) { SeachPost_img.PImg_Enable = false; } else { SeachPost_img.PImg_Enable = false; }
                        db.SaveChanges();

                        return RedirectToAction("Reportprocessing", new { Email = Email, Report_no = Report_no, Post_no = Post_no });
                    case 6:
                        var Email2 = db.Report.Where(m => m.Report_no == Report_no).First().Email;
                        var Post_no2 = db.Report.Where(m => m.Report_no == Report_no).First().Post_no;
                        Post_img SeachPost_img2 = db.Post_img.Where(m => m.Photo_no == Photo_no).FirstOrDefault();
                        if (SeachPost_img2.PImg_Enable == false) { SeachPost_img2.PImg_Enable = true; } else { SeachPost_img2.PImg_Enable = true; }
                        db.SaveChanges();

                        return RedirectToAction("Reportprocessing", new { Email = Email2, Report_no = Report_no, Post_no = Post_no2 });
                }




            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
        public ActionResult Trial(string Email, int? Post_no, int? Report_no, int? ReturnLockday_no, int? VType_no)
        {
            var PStatus = db.Report.Where(m => m.Report_no == Report_no).FirstOrDefault();
            if (PStatus.Processing_stsus == true) { PStatus.Processing_stsus = true; } else { PStatus.Processing_stsus = true; }
            db.SaveChanges();



            if (ReturnLockday_no == null)
            {


                var SearchDays = db.Violation_type.Where(m => m.VType_no == VType_no).First().Freeze_day;

                var SelectUnfreezedate = db.Report.Where(m => m.Report_no == Report_no).FirstOrDefault();
                var OperationDay = DateTime.Today.AddDays((Double)SearchDays);
                //DateTime.Today.AddDays((Double)Freeze_day).ToString("yyyy-MM-dd");
                SelectUnfreezedate.Unfreeze_date = OperationDay;
                db.SaveChanges();





            }
            else
            {

                var SearchDays = db.Violation_type.Where(m => m.VType_no == ReturnLockday_no).First().Freeze_day;

                var SelectUnfreezedate = db.Report.Where(m => m.Report_no == Report_no).FirstOrDefault();
                var OperationDay = DateTime.Today.AddDays((Double)SearchDays); //DateTime.Today.AddDays((Double)Freeze_day).ToString("yyyy-MM-dd");
                SelectUnfreezedate.Unfreeze_date = OperationDay;
                db.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        public ActionResult NoViolation(int? Report_no)
        {
            var PStatus = db.Report.Where(m => m.Report_no == Report_no).FirstOrDefault();
            if (PStatus.Processing_stsus == false) { PStatus.Processing_stsus = true; } else { PStatus.Processing_stsus = true; }


            var NStatus = db.Report.Where(m => m.Report_no == Report_no).FirstOrDefault();
            NStatus.Unfreeze_date = null;
            db.SaveChanges();

            return RedirectToAction("Index");
        }






    }
}





