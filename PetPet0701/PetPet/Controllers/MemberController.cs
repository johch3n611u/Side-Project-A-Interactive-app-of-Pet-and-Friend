using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;

namespace PetPet.Controllers
{
    public class MemberController : Controller
    {
        petpetEntities db = new petpetEntities();
        // GET: Member
        public ActionResult MemData()
        {
            if (Session["semail"] != null)
            {
                string fEmail = Session["semail"].ToString();
                return View(db.Member.Where(m => m.Email == fEmail).FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult MemberEdit()
        {
            if (Session["semail"] != null)
            {
                string fEmail = Session["semail"].ToString();
                var memdata = db.Member.Where(m => m.Email == fEmail).FirstOrDefault();
                var citylist = db.City_list.ToList();
                List<SelectListItem> items = new List<SelectListItem>();
                foreach (var city in citylist)
                {
                    items.Add(new SelectListItem() { Text = city.City_name, Value = city.City_no.ToString() });
                }
                ViewBag.memgender = memdata.Gender.ToString();
                ViewData["citiesitem"] = items;
                return View(memdata);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult MemberEdit(string Email, string Name, DateTime Birthday, bool Gender, string Phone, string City_no, HttpPostedFileBase Photo, string oldimg)
        {
            try
            {
                string fileName = "";
                if (Photo != null)
                {
                    string subname = System.IO.Path.GetExtension(Photo.FileName).ToLower();
                    if (subname == ".jpg" || subname == ".png")
                    {
                        if (Photo.ContentLength > 0)
                        {
                            //檢查是否有要新增的圖
                            Random r = new Random();
                            string datenow = DateTime.Now.ToString().Replace("/", "").Replace("上午", "").Replace("下午", "").Replace(":", "");
                            string FileName = r.Next(1000, 9999).ToString() + datenow;

                            FileName = FileName + System.IO.Path.GetFileName(Photo.FileName);
                            System.IO.File.Delete(Server.MapPath("~/images/memberimg/") + oldimg);
                            Photo.SaveAs(Server.MapPath("~/images/memberimg/" + FileName));
                            fileName = FileName;
                        }
                    }
                }
                else
                {
                    fileName = oldimg;
                }

                var member = db.Member.Where(m => m.Email == Email).FirstOrDefault();
                member.Name = Name;
                member.Birthday = Birthday;
                member.Gender = Gender;
                member.Phone = Phone;
                member.City_no = Convert.ToInt32(City_no);
                member.Mem_photo = fileName;

                db.SaveChanges();
                Session["UserName"] = Name;
                Session["UserPhoto"] = fileName;
                return RedirectToAction("MemData", "Member", new { semail = Email });

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
        public ActionResult MyPost()
        {
            if (Session["semail"] != null)
            {
                string fEmail = Session["semail"].ToString();
                var MyPost = db.Post.Where(m => m.Post_Email == fEmail).ToList();

                return View(MyPost);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult MyPet()
        {
            string fEmail = Session["semail"].ToString();
            var clickmail = db.Member.Where(m => m.Email == fEmail).FirstOrDefault();

            if (clickmail != null)
            {
                var MyPet = db.Pet.Where(m => m.Email == fEmail).ToList();
                return View(MyPet);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult PetCreate()
        {
            ViewBag.petv = db.PetVariety.ToList();
            ViewBag.pType = db.PetType.ToList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult PetCreate(string Variety_no, string Pet_name, bool Pet_gender, HttpPostedFileBase Pet_photo)
        {
            string Email = Session["semail"].ToString();
            var clickmail = db.Member.Where(m => m.Email == Email).FirstOrDefault();

            if (clickmail != null)
            {
                try
                {
                    //檢查是否有要新增的圖
                    Random r = new Random();
                    string datenow = DateTime.Now.ToString().Replace("/", "").Replace("上午", "").Replace("下午", "").Replace(":", "");
                    string FileName = r.Next(1000, 9999).ToString() + datenow;

                    int variety_no = Convert.ToInt32(Variety_no);
                    string subname = System.IO.Path.GetExtension(Pet_photo.FileName).ToLower();
                    if (subname == ".jpg" || subname == ".png")
                    {
                        if (Pet_photo.ContentLength > 0)
                        {
                            FileName = FileName + System.IO.Path.GetFileName(Pet_photo.FileName);
                            Pet_photo.SaveAs(Server.MapPath("~/images/petimg/" + FileName));

                        }

                        Pet pet = new Pet();
                        pet.Email = Email;
                        pet.PetVariety_no = Convert.ToInt32(variety_no);
                        pet.Pet_name = Pet_name;
                        pet.Pet_gender = Pet_gender;
                        pet.Pet_photo = FileName;

                        db.Pet.Add(pet);
                        db.SaveChanges();
                        return RedirectToAction("Mypet", "Member");
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
                    return RedirectToAction("Mypet", "Member");
                }
            }
            else
            {
                ViewBag.Error = "請登入後再新增寵物!";
                return RedirectToAction("Index", "Home");
            }
            //return RedirectToAction("Mypet", "Member");
        }

        public ActionResult EditPwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditPwd(string oldpwd, string newpwd, string newpwd2)
        {
            string fEmail = Session["semail"].ToString();
            var checkmem = db.Member.Where(m => m.Email == fEmail && m.Pwd == oldpwd).FirstOrDefault();
            if (checkmem != null)
            {
                var member = db.Member.Where(m => m.Email == fEmail).FirstOrDefault();
                member.Pwd = newpwd;
                db.SaveChanges();
                TempData["msg"] = "修改成功!";
                ViewBag.Editpwd = true;
                return RedirectToAction("EditPwd");
            }
            else
            {
                TempData["msg"] = "密碼錯誤!";
                ViewBag.Editpwd = false;
                return RedirectToAction("EditPwd");
            }
        }
        //寵物資料編輯
        public ActionResult PetEdit(int id)
        {
            if (Session["semail"] != null)
            {
                string fEmail = Session["semail"].ToString();
                var petdata = db.Pet.Where(m => m.Email == fEmail && m.Pet_no == id).FirstOrDefault();
                var petvar = db.PetVariety.ToList();
                List<SelectListItem> items = new List<SelectListItem>();
                foreach (var PV in petvar)
                {
                    items.Add(new SelectListItem() { Text = PV.Variety_name, Value = PV.PetVariety_no.ToString() });
                }
                ViewBag.petgender = petdata.Pet_gender.ToString();
                ViewBag.petvar = db.PetVariety.ToList();
                ViewData["PetVaritem"] = items;
                return View(petdata);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult PetEdit(int Pet_no, string Pet_name, bool Gender, string Varietyno, HttpPostedFileBase Photo, string oldimg)
        {
            try
            {
                string fileName = "";
                if (Photo != null)
                {
                    string subname = System.IO.Path.GetExtension(Photo.FileName).ToLower();
                    if (subname == ".jpg" || subname == ".png")
                    {
                        if (Photo.ContentLength > 0)
                        {
                            fileName = System.IO.Path.GetFileName(Photo.FileName);
                            Photo.SaveAs(Server.MapPath("~/images/petimg/" + Photo.FileName));
                            System.IO.File.Delete(Server.MapPath("~/images/petimg/") + oldimg);
                        }
                    }
                }
                else
                {
                    fileName = oldimg;
                }
                string fEmail = Session["semail"].ToString();
                var petdata = db.Pet.Where(m => m.Pet_no == Pet_no).FirstOrDefault();
                petdata.Pet_name = Pet_name;
                petdata.Pet_gender = Gender;
                petdata.PetVariety_no = Convert.ToInt32(Varietyno);
                petdata.Pet_photo = fileName;

                db.SaveChanges();
                return RedirectToAction("MyPet", "Member", new { semail = fEmail });

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
        public ActionResult PetDelete(int id)
        {
            string fEmail = Session["semail"].ToString();
            var petdata = db.Pet.Where(m => m.Pet_no == id).FirstOrDefault();

            string filename = petdata.Pet_photo;
            System.IO.File.Delete(Server.MapPath("~/images/petimg/") + filename);

            db.Pet.Remove(petdata);
            db.SaveChanges();
            return RedirectToAction("MyPet", "Member", new { semail = fEmail });
        }
        public ActionResult _PetVariety()
        {
            ViewBag.ptn = db.PetVariety.ToList();
            return PartialView("_PetVariety");
        }
        [HttpPost]
        public ActionResult _PetVariety(int PetTno)
        {
            ViewBag.ptn = db.PetVariety.Where(m => m.PetType_no == PetTno).ToList();
            return PartialView("_PetVariety");
        }
    }
}