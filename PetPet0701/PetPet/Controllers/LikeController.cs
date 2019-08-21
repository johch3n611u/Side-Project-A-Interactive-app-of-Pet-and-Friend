using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;
using System.IO;

namespace PetPet.Controllers
{
    public class LikeController : Controller
    {
        petpetEntities db = new petpetEntities();

        // GET: Like
        [ChildActionOnly]//無法在瀏覽器上用URL存取此action
        public ActionResult _Like(int PostId)
        {
            string semail = Session["semail"].ToString();
            var like = db.Like_record.Where(m => m.Post_no == PostId).ToList();
            ViewBag.PostId = PostId;
            return PartialView(like);
        }

        [HttpPost]
        public ActionResult _Like(string PostId)
        {
            string fEmail = Session["semail"].ToString();
            int postno = Convert.ToInt32(PostId);
            string user = db.Member.Where(m => m.Email == fEmail).FirstOrDefault().Email;
            var LikeItem = db.Like_record.Where(m => m.Email == fEmail && m.Post_no == postno).FirstOrDefault();
            Like_record like = new Like_record();

            if (LikeItem == null)
            {
                like.Email = user;
                like.Post_no = Convert.ToInt32(PostId);
                db.Like_record.Add(like);
                db.SaveChanges();
            }
            else
            {
                db.Like_record.Remove(LikeItem);
                db.SaveChanges();

            }
            //7.6-1 在CommentController加入_CommentsForPhoto POST Action(第二個畫面)
            ViewBag.PostId = PostId;
            var likes = db.Like_record.Where(m => m.Post_no == postno).ToList();
            return PartialView("_Like", likes);
        }
    }
}