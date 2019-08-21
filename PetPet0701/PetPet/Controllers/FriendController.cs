using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetPet.Models;

namespace PetPet.Controllers
{
    public class FriendController : Controller
    {
        petpetEntities db = new petpetEntities();
        // GET: Friend
        public ActionResult FriendIndex()
        {
            string MyEmail = Session["semail"].ToString();
            //計算交友邀請數量
            ViewBag.AddFriendNotice = db.Friend.Where(m => m.F_Email == MyEmail && m.Add_ststus == false).Count().ToString();
            return View(db.Friend.Where(m => m.Email == MyEmail).ToList());

        }
        //送交友邀請
        public ActionResult AddFriend()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddFriend(string FriendEmail)
        {
            string MyEmail = Session["semail"].ToString();
            TempData["Rmsg"] = "成功送出交友邀請!!";
            var FriendData = db.Member.Where(m => m.Email == FriendEmail).FirstOrDefault();

            Friend newFriend = new Friend();
            newFriend.Email = MyEmail;
            newFriend.F_Email = FriendEmail;
            newFriend.Nickname = FriendData.Name.ToString();
            newFriend.Change_time = DateTime.Now;
            newFriend.Add_ststus = false;

            db.Friend.Add(newFriend);
            db.SaveChanges();

            return RedirectToAction("FriendIndex", "Friend");
        }
        //申請交友的清單
        public ActionResult NewFriendNotice()
        {
            string MyEmail = Session["semail"].ToString();
            var NewFriendList = db.Friend.Where(f => f.F_Email == MyEmail && f.Add_ststus == false).ToList();
            return View(NewFriendList);
        }
        //同意加入好友
        public ActionResult AddYes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddYes(string newfriend)
        {
            string MyEmail = Session["semail"].ToString();
            var NewFriendData = db.Member.Where(f => f.Email == newfriend).FirstOrDefault();
            var friendvisit = db.Friend.Where(m => m.Email == newfriend && m.F_Email == MyEmail).FirstOrDefault();

            friendvisit.Add_ststus = true;
            db.SaveChanges();
            Friend addnew = new Friend();
            addnew.Email = MyEmail;
            addnew.F_Email = newfriend;
            addnew.Add_ststus = true;
            addnew.Change_time = DateTime.Now;
            addnew.Nickname = NewFriendData.Name.ToString();

            db.Friend.Add(addnew);
            db.SaveChanges();

            TempData["Rmsg"] = "加入成功!!";
            return RedirectToAction("NewFriendNotice", "Friend");
        }
        //拒絕加入好友
        //public ActionResult AddNo()
        //{
        //    return View();
        //}
        //[HttpPost]
        public ActionResult AddNo(string removeid)
        {
            string MyEmail = Session["semail"].ToString();
            var NewFriendData = db.Member.Where(f => f.Email == removeid).FirstOrDefault();
            var removevisit = db.Friend.Where(m => m.Email == removeid && m.F_Email == MyEmail).FirstOrDefault();

            db.Friend.Remove(removevisit);
            db.SaveChanges();

            TempData["Rmsg"] = "拒絕成功!!";
            return RedirectToAction("NewFriendNotice", "Friend");
        }
        //改好友暱稱
        public ActionResult EditFName()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditFName(string Fno, string FriendNewName)
        {
            string MyEmail = Session["semail"].ToString();
            int f_no = Convert.ToInt32(Fno);
            var editfName = db.Friend.Where(m => m.Friend_no == f_no && m.Email == MyEmail).FirstOrDefault();
            editfName.Nickname = FriendNewName;
            db.SaveChanges();
            TempData["Rmsg"] = "修改成功!!";
            return RedirectToAction("FriendIndex", "Friend");
        }
        //刪除好友
        public ActionResult DeleteFriend(string FriendEmail)
        {
            string MyEmail = Session["semail"].ToString();
            var myFriendData = db.Friend.Where(f => f.Email == MyEmail && f.F_Email == FriendEmail).FirstOrDefault();
            var friendMyData = db.Friend.Where(f => f.Email == FriendEmail && f.F_Email == MyEmail).FirstOrDefault();
            db.Friend.Remove(myFriendData);
            db.Friend.Remove(friendMyData);
            db.SaveChanges();
            TempData["Rmsg"] = "刪除成功!!";
            return RedirectToAction("FriendIndex", "Friend");
        }
    }
}