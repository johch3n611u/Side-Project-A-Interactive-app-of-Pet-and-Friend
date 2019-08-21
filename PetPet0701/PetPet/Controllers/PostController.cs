using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;
using System.IO;

namespace PetPet.Controllers
{
    public class PostController : Controller
    {
        petpetEntities db = new petpetEntities();
        // GET: Post
        public ActionResult PostIndex(string search)
        {
            if (Session["semail"] != null)
            {
                var post = from m in db.Post
                           select m;
                ViewBag.FristR = db.Violation_type.First();
                ViewBag.Report = db.Violation_type.ToList().Skip(1);


                if (!String.IsNullOrEmpty(search))
                {
                    post = post.Where(s => s.Post_title.Contains(search));
                }
                var myemail = Session["semail"].ToString();
                ViewBag.like = db.Like_record.ToList();
                ViewBag.postimg = db.Post_img.ToList();
                Session["Pimg"] = db.Post_img.ToList();
                ViewData["Pimg"] = db.Post_img.ToList();
                ViewBag.Friend = db.Friend.Where(m => m.Email == myemail).ToList();
                return View(post);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //詳細貼文
        //在前端View做一個ActionLink，設定@name=item.Post_no;
        //如此一來，網址連結就會有帶參數，參數為貼文編號。
        //一定要抓string進來
        public ActionResult DetailPost(string Post_no)
        {
            if (Session["semail"] != null)
            {
                string usericon = Session["semail"].ToString();
                ViewBag.usericon = db.Member.Where(m => m.Email == usericon).FirstOrDefault().Mem_photo;
                //宣告一個變數存放前端帶過來的值
                //使用Request的QueryString方法可以讀網址的值，例如name=P01;
                int PostDD = Convert.ToInt32(Request.QueryString["name"].ToString());
                ViewBag.like = db.Like_record.Where(m => m.Post_no == PostDD).ToList().Count();
                //Convert轉行成int跟資料庫資料做比對
                var post = db.Post.Where(m => m.Post_no == PostDD).FirstOrDefault();
                //return資料
                return View(post);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }           
        }

        //新增貼文
        [HttpPost]
        public ActionResult PostIndex(string Email, string Post_title, string Post_content, IEnumerable<HttpPostedFileBase> Post_photo)
        {
            Post_img post_Img = new Post_img();
            Post post = new Post();

            //將文章內容寫入創建的post後再新增進資料庫
            post.Post_time = DateTime.Now;
            post.Post_Email = Email;
            post.Post_title = Post_title;
            post.Post_content = Post_content;
            post.Post_Enable = Convert.ToBoolean("True");

            db.Post.Add(post);
            Random r = new Random();
            string datenow = DateTime.Now.ToString().Replace("/", "").Replace("上午", "").Replace("下午", "").Replace(":", "");
            string fileName = r.Next(1000, 9999).ToString() + datenow;
            //檢查是否有圖片
            if (Post_photo != null)
            {
                foreach (var file in Post_photo)
                {
                    if (file != null)
                    {
                        string subname = Path.GetExtension(file.FileName).ToLower();
                        if (subname == ".jpg" || subname == ".png")
                        {
                            if (file.ContentLength > 0)
                            {
                                fileName += Path.GetFileName(file.FileName);
                                string path = Path.Combine(Server.MapPath("~/images/postimg/"), fileName);
                                file.SaveAs(path);
                                post_Img.Post_photo = fileName;
                                post_Img.PImg_Enable = true;
                                db.Post_img.Add(post_Img);
                                db.SaveChanges();

                            }
                        }
                    }
                }
            }
            return RedirectToAction("PostIndex");
        }


        //編輯貼文
        [HttpPost]
        public ActionResult EditPost(string id, string Etitle, string EdPost, IEnumerable<HttpPostedFileBase> EditPostPhoto, IEnumerable<int> EdimgNo,string status)
        {

            Post_img post_Img = new Post_img();
            int EdPostNo = Convert.ToInt32(id);
            var EditPost = db.Post.Where(m => m.Post_no == EdPostNo).FirstOrDefault();
            EditPost.Post_time = DateTime.Now;
            EditPost.Post_title = Etitle;
            EditPost.Post_content = EdPost;
            int Status = Convert.ToInt32(status);

            //檢查是否有要刪的圖
            int EdImgCount = 0;
            int DelImgCount = 0;
            
            int UpImgCount = 0;
            //要刪除的圖片總數
            if (EdimgNo != null) 
            {
                DelImgCount = Convert.ToInt32(EdimgNo.ToList().Count());
            }
            else
            {
                DelImgCount = 0;
            }
            //修改前圖片總數
            if (db.Post_img.Where(m => m.Post_no == EdPostNo).FirstOrDefault() != null)
                {
                    EdImgCount = Convert.ToInt32(db.Post_img.Where(m => m.Post_no == EdPostNo).ToList().Count());
                }
            else
            {
                TempData["msg"] = "出現異常2!";
                return RedirectToAction("DetailPost", new { name = EdPostNo });
            }
            if (Status == 1)
            {
                if (EditPostPhoto != null)
                {
                    UpImgCount = Convert.ToInt32(EditPostPhoto.ToList().Count());
                }
                else
                {
                    TempData["msg"] = "出現異常3!";
                    return RedirectToAction("DetailPost", new { name = EdPostNo });
                }
            }
            else
            {
                UpImgCount = 0;
            }

            //上傳的圖片總數
            int k = EdImgCount;
            int j = DelImgCount;
            int h = UpImgCount;
            int i = ((EdImgCount - DelImgCount) + UpImgCount);
            //如果Po文會沒有圖，不執行，至少要有一張圖
            if (i < 1)
            {
                TempData["msg"] = "姆~~~貼文最少要有一張圖ㄛ!";
                return RedirectToAction("DetailPost", new { name = EdPostNo });
            }

            //檢查是否有要新增的圖
            else if (i >= 1)
            {
                //TempData["msg"] = "信息,修改後的總數=" + i + ",修改前總數=" + k + ",要上傳數量=" + h + ",要刪除數量=" + j + ",status=" + status;
                Random r = new Random();
                string datenow = DateTime.Now.ToString().Replace("/", "").Replace("上午", "").Replace("下午", "").Replace(":", "");
                string fileName = r.Next(1000, 9999).ToString() + datenow;
                //檢查是否有圖片
                if (EditPostPhoto != null)
                {
                    foreach (var AddPhoto in EditPostPhoto)
                    {
                        if (AddPhoto != null)
                        {
                            string subname = Path.GetExtension(AddPhoto.FileName).ToLower();
                            if (subname == ".jpg" || subname == ".png")
                            {
                                if (AddPhoto.ContentLength > 0)
                                {
                                    fileName += Path.GetFileName(AddPhoto.FileName);
                                    string path = Path.Combine(Server.MapPath("~/images/postimg/"), fileName);
                                    AddPhoto.SaveAs(path);
                                    post_Img.Post_photo = fileName;
                                    post_Img.PImg_Enable = true;
                                    post_Img.Post_no = EdPostNo;
                                    db.Post_img.Add(post_Img);
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    if (EdimgNo != null)
                    {
                        foreach (var Delimg in EdimgNo)
                        {
                            var delimg = db.Post_img.Where(m => m.Photo_no == Delimg).FirstOrDefault();
                            db.Post_img.Remove(delimg);
                            //本機刪圖片
                            System.IO.File.Delete(Server.MapPath("~/images/postimg/") + delimg.Post_photo);

                            db.SaveChanges();
                        }
                    }
                }                
            }
            db.SaveChanges();
            return RedirectToAction("DetailPost", new { name = EdPostNo });
        }

        //刪除貼文
        public ActionResult DeletePost(string Post_no)
        {
            int P = Convert.ToInt32(Request.QueryString["name"].ToString());
            var post = db.Post.Where(m => m.Post_no == P).FirstOrDefault();
            var post_img = db.Post_img.Where(m => m.Post_no == P).ToList();

            //要順便把有關聯的按讚資料、留言資料也一併刪除
            var Like = db.Like_record.Where(m => m.Post_no == P).ToList();
            var Msg = db.Messenger.Where(m => m.Post_no == P).ToList();
            foreach (var dellike in Like)
            {
                db.Like_record.Remove(dellike);
            }
            foreach (var delmsg in Msg)
            {
                db.Messenger.Remove(delmsg);
            }
            //刪除被檢舉資料
            var Report = db.Report.Where(m => m.Post_no == P).ToList();

            foreach (var delreport in Report)
            {
                db.Report.Remove(delreport);
            }

            //從本機刪圖片
            foreach (var delimg in post_img)
            {
                string filename = delimg.Post_photo;
                System.IO.File.Delete(Server.MapPath("~/images/postimg/") + filename);
                db.Post_img.Remove(delimg);
            }

            db.Post.Remove(post);
            db.SaveChanges();

            return RedirectToAction("PostIndex");
        }


        //[HttpPost]
        //public ActionResult Like(string id)
        //{
        //    string fEmail = Session["semail"].ToString();
        //    int postno = Convert.ToInt32(id);
        //    string user = db.Member.Where(m => m.Email == fEmail).FirstOrDefault().Email;
        //    var LikeItem = db.Like_record.Where(m => m.Email == fEmail && m.Post_no == postno).FirstOrDefault();
        //    Like_record like = new Like_record();

        //    if (LikeItem == null)
        //    {
        //        like.Email = user;
        //        like.Post_no = Convert.ToInt32(id);
        //        db.Like_record.Add(like);
        //        db.SaveChanges();

        //    }
        //    else
        //    {
        //        db.Like_record.Remove(LikeItem);
        //        db.SaveChanges();

        //    }
        //    return RedirectToAction("PostIndex", new { name = id });
        //}

        //[HttpPost]
        //public ActionResult DLike(string id)
        //{
        //    string fEmail = Session["semail"].ToString();
        //    int postno = Convert.ToInt32(id);
        //    string user = db.Member.Where(m => m.Email == fEmail).FirstOrDefault().Email;
        //    var LikeItem = db.Like_record.Where(m => m.Email == fEmail && m.Post_no == postno).FirstOrDefault();
        //    Like_record like = new Like_record();

        //    if (LikeItem == null)
        //    {
        //        like.Email = user;
        //        like.Post_no = Convert.ToInt32(id);
        //        db.Like_record.Add(like);
        //        db.SaveChanges();

        //    }
        //    else
        //    {
        //        db.Like_record.Remove(LikeItem);
        //        db.SaveChanges();

        //    }

        //    return RedirectToAction("DetailPost", new { name = id });
        //}

        //GET: Report
        [HttpPost]
        public ActionResult ReportPost(string ReportPostId, string ReportItem)
        {
            string semail = Session["semail"].ToString();

            //被檢舉貼文編號RID
            int reportpostid = Convert.ToInt32(ReportPostId);

            //違規項目編號PID
            int reportitem = Convert.ToInt32(ReportItem);

            //被檢舉文章
            var reportpost = db.Post.Where(m => m.Post_no == reportpostid).FirstOrDefault();
            Report report = new Report();

            //檢舉單
            var reportlist = db.Report.Where(m => m.Post_no == reportpostid && m.VType_no == reportitem && m.Email == semail).FirstOrDefault();

            //被檢舉次數

            if (reportlist != null)
            {
                var amount = 0;
                amount = reportlist.Report_amount;

                if (reportlist.Email == semail && reportlist.Post_no == reportpostid && amount >= 1)
                {
                    //如果已處理了，回傳訊息
                    TempData["msg"] = "已檢舉過囉，請靜待消息!";
                    return RedirectToAction("PostIndex", "Post");
                }
                else
                {
                    //如果已處理了，回傳訊息
                    TempData["msg"] = "很奇怪喔!!不要亂點";
                    return RedirectToAction("PostIndex", "Post");
                }
            }
            else if (reportlist == null || reportlist.VType_no != reportitem)
            {
                report.VType_no = reportitem;
                report.Email = semail;
                report.Post_no = reportpostid;
                report.Report_amount = 1;
                report.Processing_stsus = false;
                report.Unfreeze_date = null;
                //加一筆資料
                db.Report.Add(report);
                db.SaveChanges();
                TempData["msg"] = "檢舉成功，已回報給管理員!";
            }
            return RedirectToAction("PostIndex", "Post");
        }
    }
}