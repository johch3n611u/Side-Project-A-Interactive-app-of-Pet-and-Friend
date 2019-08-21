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


    public class BgStatisticalAnalysisController : Controller
    {
        //private 
        petpetEntities db = new petpetEntities();
        // GET: BgStatisticalAnalysis
        public ActionResult Index()
        {

            var MaxlengthPost = (db.Post.ToList()).Count();
            int SeasonOne = 0, SeasonSecond = 0, SeasonThird = 0, SeasonFourth = 0;
            for (var i = 0; i < MaxlengthPost; i++)
            {

                var OwnPostTime = db.Post.ToList()[i];

                var OwnPostTimeMonth = OwnPostTime.Post_time.Month;

                if (OwnPostTimeMonth <= 3)
                {
                    SeasonOne++;
                }

                else if (OwnPostTimeMonth <= 6)
                {
                    SeasonSecond++;
                }

                else if (OwnPostTimeMonth <= 9)
                {
                    SeasonThird++;
                }

                else if (OwnPostTimeMonth <= 12)
                {
                    SeasonFourth++;
                }
            };

            ViewBag.SeasonOne = SeasonOne;
            ViewBag.SeasonSecond = SeasonSecond;
            ViewBag.SeasonThird = SeasonThird;
            ViewBag.SeasonFourth = SeasonFourth;




            var MaxlengthMember = (db.Member.ToList()).Count();
            int MemberMan = 0, MemberWoman = 0, MemberManSeasonOne = 0, MemberWomanSeasonOne = 0, MemberManSeasonSecond = 0, MemberWomanSeasonSecond = 0, MemberManSeasonThird = 0, MemberWomanSeasonThird = 0, MemberManSeasonFourth = 0, MemberWomanSeasonFourth = 0;

            for (var i = 0; i < MaxlengthPost; i++)
            {

                var OwnMember = db.Member.ToList()[i];

                var OwnBirthdayMonth = OwnMember.Birthday.Month;

                if (OwnMember.Gender == true)
                {
                    MemberMan++;

                    if (OwnBirthdayMonth <= 3)
                    {
                        MemberManSeasonOne++;
                    }

                    else if (OwnBirthdayMonth <= 6)
                    {
                        MemberManSeasonSecond++;
                    }

                    else if (OwnBirthdayMonth <= 9)
                    {
                        MemberManSeasonThird++;
                    }

                    else if (OwnBirthdayMonth <= 12)
                    {
                        MemberManSeasonFourth++;
                    }
                }

                else if (OwnMember.Gender == false)
                {
                    MemberWoman++;

                    if (OwnBirthdayMonth <= 3)
                    {
                        MemberWomanSeasonOne++;
                    }

                    else if (OwnBirthdayMonth <= 6)
                    {
                        MemberWomanSeasonSecond++;
                    }

                    else if (OwnBirthdayMonth <= 9)
                    {
                        MemberWomanSeasonThird++;
                    }

                    else if (OwnBirthdayMonth <= 12)
                    {
                        MemberWomanSeasonFourth++;
                    }
                }


            };

            ViewBag.MemberMan = MemberMan;
            ViewBag.MemberWoman = MemberWoman;
            ViewBag.MemberManSeasonOne = MemberManSeasonOne;
            ViewBag.MemberManSeasonSecond = MemberManSeasonSecond;
            ViewBag.MemberManSeasonThird = MemberManSeasonThird;
            ViewBag.MemberManSeasonFourth = MemberManSeasonFourth;
            ViewBag.MemberWomanSeasonOne = MemberWomanSeasonOne;
            ViewBag.MemberWomanSeasonSecond = MemberWomanSeasonSecond;
            ViewBag.MemberWomanSeasonThird = MemberWomanSeasonThird;
            ViewBag.MemberWomanSeasonFourth = MemberWomanSeasonFourth;

            ViewBag.TotalMember = MemberMan + MemberWoman;

            //var MaxlengthReport = (db.Report.ToList()).Count(); 
            //int nonpetrelated=0,
            //    brainwashing=0 ,
            //    Abusiveanimal=0,
            //    bare=0,
            //    violence=0,
            //    harassment=0,
            //    Suicideorselfharm=0,
            //    Untruereport=0,
            //    spammessage=0,
            //    Sellingcontraband=0,
            //    Hatespeech=0,
            //    terrorism=0;

            var MaxlengthViolation_type = (db.Violation_type.ToList()).Count();
            var Violation_typeName = db.Violation_type.ToList();
            String[] Violation_typeNames = new String[MaxlengthViolation_type];

            for (var i = 0; i < MaxlengthViolation_type; i++)
            {

                Violation_typeNames[i] = Violation_typeName.ToList()[i].VType_name;
            }

            ViewBag.Violation_typeNames = Violation_typeNames;
            ViewBag.MaxlengthViolation_type = MaxlengthViolation_type;

            //for (var i = 0; i < MaxlengthReport; i++)
            //{
            //    var OwnVTypeno = db.Report.ToList()[i].VType_no;

            //    switch (OwnVTypeno) {


            //        case 1:
            //            nonpetrelated++;
            //            break;
            //        case 2:

            //            brainwashing++;

            //            break;
            //        case 3:

            //            Abusiveanimal++;

            //            break;
            //        case 4:

            //            bare++;

            //            break;
            //        case 5:

            //            violence++;

            //            break;
            //        case 6:

            //            harassment++;

            //            break;
            //        case 7:

            //            Suicideorselfharm++;

            //            break;
            //        case 8:

            //            Untruereport++;

            //            break;
            //        case 9:

            //            spammessage++;

            //            break;
            //        case 10:

            //            Sellingcontraband++;

            //            break;
            //        case 11:

            //            Hatespeech++;

            //            break;
            //        case 12:

            //            terrorism++;
            //            break;
            //    };


            //};


            //ViewBag.nonpetrelated = nonpetrelated;
            //ViewBag.brainwashing = brainwashing;
            //ViewBag.Abusiveanimal = Abusiveanimal;
            //ViewBag.bare = bare;
            //ViewBag.violence = violence;
            //ViewBag.harassment = harassment;
            //ViewBag.Suicideorselfharm = Suicideorselfharm;
            //ViewBag.Untruereport = Untruereport;
            //ViewBag.spammessage = spammessage;
            //ViewBag.Sellingcontraband = Sellingcontraband;
            //ViewBag.Hatespeech = Hatespeech;
            //ViewBag.terrorism = terrorism;


            var MaxlengthReport = (db.Report.ToList()).Count();
            int[] ReportNumbers = new int[MaxlengthReport];

            for (var i = 0; i < MaxlengthReport; i++)
            {

                var OwnReportViolation_no = db.Report.ToList()[i].VType_no;

                var SeachPetType_no = db.Violation_type.Where(m => m.VType_no == OwnReportViolation_no).First().VType_no;

                for (var j = 0; j < MaxlengthViolation_type; j++)
                {

                    if (SeachPetType_no == Violation_typeName.ToList()[j].VType_no)
                    {

                        ReportNumbers[j] += 1;
                        j = MaxlengthViolation_type;
                    }


                }

            }


            ViewBag.ReportNumbers = ReportNumbers;
            ViewBag.MaxlengthReport = MaxlengthReport;
































            //Test陣列方式[]

            var MaxlengthPetType = (db.PetType.ToList()).Count();
            var PetTypeName = db.PetType.ToList();
            String[] PetTypeNames = new String[MaxlengthPetType];

            for (var i = 0; i < MaxlengthPetType; i++)
            {

                PetTypeNames[i] = PetTypeName.ToList()[i].PetType_name;

            };

            ViewBag.PetTypeNames = PetTypeNames;
            ViewBag.MaxlengthPetType = MaxlengthPetType;









            var MaxlengthPet = (db.Pet.ToList()).Count();

            int[] PetTypeNumbers = new int[MaxlengthPetType];

            for (var i = 0; i < MaxlengthPet; i++)
            {
                var OwnPetVariety_no = db.Pet.ToList()[i].PetVariety_no;

                var SeachPetType_no = db.PetVariety.Where(m => m.PetVariety_no == OwnPetVariety_no).First().PetType_no;

                for (var j = 0; j < MaxlengthPetType; j++)
                {

                    if (SeachPetType_no == PetTypeName.ToList()[j].PetType_no)
                    {

                        PetTypeNumbers[j] += 1;

                        j = MaxlengthPetType;
                    };

                };

            };

            ViewBag.PetTypeNumbers = PetTypeNumbers;
            ViewBag.MaxlengthPet = MaxlengthPet;


            return View();
        }
    }
}