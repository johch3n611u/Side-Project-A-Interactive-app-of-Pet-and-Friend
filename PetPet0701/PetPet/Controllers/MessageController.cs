using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;
using System.IO;


namespace PetPet.Controllers
{
    public class MessageController : Controller
    {
        petpetEntities db = new petpetEntities();
        // GET: Message

        [ChildActionOnly]//無法在瀏覽器上用URL存取此action
        public ActionResult _MsgForPost(int PostId)
        {
            string semail = Session["semail"].ToString();
            var msg = db.Messenger.Where(m => m.Post_no == PostId).ToList();
            ViewBag.PostId = PostId;
            ViewBag.Friend = db.Friend.Where(m => m.Email == semail).ToList();
            return PartialView(msg);
        }

        //留言
        public ActionResult _CreateMsg(int PostId)
        {
            Messenger newMsg = new Messenger();
            newMsg.Post_no = PostId;

            ViewBag.Post_no = PostId;
            string usericon = Session["semail"].ToString();
            ViewBag.usericon = db.Member.Where(m => m.Email == usericon).FirstOrDefault().Mem_photo;

            return PartialView("_CreateMsg");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _MsgForPost(string Message, int PostId)
        {
            string semail = Session["semail"].ToString();
            if (Message == "")
            {
                var msg = db.Messenger.Where(m => m.Post_no == PostId).ToList();
                ViewBag.PostId = PostId;
                ViewBag.Friend = db.Friend.Where(m => m.Email == semail).ToList();
                return PartialView("_MsgForPost");
            }
            else
            {
                int postno = Convert.ToInt32(PostId);
                string user = db.Member.Where(m => m.Email == semail).FirstOrDefault().Email;
                Messenger message = new Messenger();
                message.Email = user;
                message.Post_no = postno;
                message.Msg_content = Message;
                message.Mag_time = DateTime.Now;
                db.Messenger.Add(message);
                db.SaveChanges();

                ViewBag.Friend = db.Friend.Where(m => m.Email == semail).ToList();

                //7.6-1 在CommentController加入_CommentsForPhoto POST Action(第二個畫面)
                ViewBag.PostId = PostId;

                var msgs = db.Messenger.Where(m => m.Post_no == PostId).ToList();
                return PartialView("_MsgForPost", msgs);
            }
        }
        [HttpPost]
        public ActionResult EdMessage(int EdMsgId, string edmessage, int EdPostId)
        {
            string semail = Session["semail"].ToString();

            var Message = db.Messenger.Where(m => m.Msg_no == EdMsgId).FirstOrDefault();
            ViewBag.XX = Message.Msg_no;
            Message.Msg_content = edmessage;
            Message.Mag_time = DateTime.Now;
            db.SaveChanges();

            ViewBag.Friend = db.Friend.Where(m => m.Email == semail).ToList();
            ViewBag.PostId = EdPostId;
            var msgs = db.Messenger.Where(m => m.Post_no == EdPostId).ToList();
            return PartialView("_MsgForPost", msgs);
        }

        //刪除
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult ReMessage(int remsg, int id)
        {

            string semail = Session["semail"].ToString();
            var remessage = db.Messenger.Where(m => m.Msg_no == remsg).FirstOrDefault();
            db.Messenger.Remove(remessage);
            db.SaveChanges();

            ViewBag.Friend = db.Friend.Where(m => m.Email == semail).ToList();
            ViewBag.PostId = id;
            var msgs = db.Messenger.Where(m => m.Post_no == id).ToList();
            return PartialView("_MsgForPost", msgs);

            //return RedirectToAction("DetailPost", "Post", new { name = id });
        }

    }
}