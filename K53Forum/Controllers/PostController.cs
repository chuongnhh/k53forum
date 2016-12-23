using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using K53Forum.Models;
using K53Forum.CodeHelper;
using System.Web.Security;

namespace K53Forum.Controllers
{
    public class PostController : Controller
    {
        private DbK53Forum db = new DbK53Forum();

        // GET: Post
        public ActionResult Index()
        {
            CurrentAction.currentAction = "";
            var post = db.Posts.Include(p => p.Category).Include(p => p.Member);
            return View(post.ToList());
        }

        // GET: Post/Details/5
        public ActionResult Details(long? id)
        {
            //ViewBag.Comment = db.Posts.Find(id).Comments;
            CurrentAction.currentAction = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            post.Count++;
            db.SaveChanges();
            return View(post);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            CurrentAction.currentAction = "";
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,DateCreated,Liked,Disliked,MemberId,CatagoryId")] Post post)
        {
            CurrentAction.currentAction = "";
            if (ModelState.IsValid)
            {
                string username = Membership.GetUser().UserName;
                long id = db.Members
                    .Where(x => x.Username == username)
                    .FirstOrDefault<Member>().Id;
                post.MemberId = id;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("details", "post", new { id = post.Id });
            }

            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", post.CatagoryId);
            return View(post);
        }

        // GET: Post/Edit/5
        public ActionResult Edit(long? id)
        {
            CurrentAction.currentAction = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", post.CatagoryId);
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Title,Content,CatagoryId")] Post post)
        //{
        //    CurrentAction.currentAction = "";
        //    if (ModelState.IsValid)
        //    {
        //        var p = db.Posts.Find(post.Id);
        //        p.Title = post.Title;
        //        p.Content = post.Content;
        //        p.CatagoryId = post.CatagoryId;

        //        db.SaveChanges();
        //        return RedirectToAction("details", "post", new { id = post.Id });
        //    }
        //    ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", post.CatagoryId);
        //    return View(post);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit([Bind(Include = "Id,Title,Content,CatagoryId")] Post post)
        {
            CurrentAction.currentAction = "";
            if (ModelState.IsValid)
            {
                var p = db.Posts.Find(post.Id);
                p.Title = post.Title;
                p.Content = post.Content;
                p.CatagoryId = post.CatagoryId;

                db.SaveChanges();
                return Json(new { status = true, name = post.Title,id=post.Id });
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", post.CatagoryId);
            return Json(new { status = false, name = post.Title, id = post.Id });
        }

        // GET: Post/Delete/5
        public ActionResult Delete(long? id)
        {
            CurrentAction.currentAction = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CurrentAction.currentAction = "";
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("index");
        }

        [HttpPost]
        public JsonResult Liked(long id)
        {
            bool status = false;
            string username = Membership.GetUser().UserName;
            var memberId = db.Members
                .Where(x => x.Username == username)
                .FirstOrDefault<Member>().Id;

            Liked disliked = db.Likeds
                .Where(x => x.PostId == id && x.MemberId == memberId)
                .FirstOrDefault<Liked>();
            if (disliked != null)
            {
                db.Likeds.Remove(disliked);
                db.SaveChanges();
            }
            else
            {
                disliked = new Liked
                {
                    MemberId = memberId,
                    PostId = id
                };

                db.Likeds.Add(disliked);
                db.SaveChanges();
                status = true;
            }
            var post = db.Posts.Where(x => x.Id == id).FirstOrDefault<Post>();

            return Json(new { status = status, count = post != null ? post.Likeds.Count() : 0 });
        }


        [HttpPost]
        public JsonResult DisLiked(long id)
        {
            bool status = false;
            string username = Membership.GetUser().UserName;
            var memberId = db.Members
                .Where(x => x.Username == username)
                .FirstOrDefault<Member>().Id;

            DisLiked disliked = db.DisLikeds
                .Where(x => x.PostId == id && x.MemberId == memberId)
                .FirstOrDefault<DisLiked>();
            if (disliked != null)
            {
                db.DisLikeds.Remove(disliked);
                db.SaveChanges();
            }
            else
            {
                disliked = new DisLiked
                {
                    MemberId = memberId,
                    PostId = id
                };

                db.DisLikeds.Add(disliked);
                db.SaveChanges();
                status = true;
            }
            var post = db.Posts.Where(x => x.Id == id).FirstOrDefault<Post>();

            return Json(new { status = status, count = post != null ? post.DisLikeds.Count() : 0 });
        }

        [HttpPost]
        public ActionResult Commented(long id, string commented)
        {
            string username = Membership.GetUser().UserName;
            var memberId = db.Members
                .Where(x => x.Username == username)
                .FirstOrDefault<Member>().Id;

            Comment comment = new Comment()
            {
                MemberId = memberId,
                PostId = id,
                Content = commented
            };

            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("details", "post", new { id = id });
        }

        //[HttpPost]
        //public JsonResult Commented(long id, string commented)
        //{
        //    string username = Membership.GetUser().UserName;
        //    var memberId = db.Members
        //        .Where(x => x.Username == username)
        //        .FirstOrDefault<Member>().Id;

        //    Comment comment = new Comment()
        //    {
        //        MemberId = memberId,
        //        PostId = id,
        //        Content = commented
        //    };

        //    db.Comments.Add(comment);
        //    db.SaveChanges();
        //    return Json(new { data = comment });
        //}

        [HttpPost]
        public ActionResult Replied(long id, long commentId, string replied)
        {
            string username = Membership.GetUser().UserName;
            var memberId = db.Members
                .Where(x => x.Username == username)
                .FirstOrDefault<Member>().Id;

            Reply reply = new Reply()
            {
                MemberId = memberId,
                CommentID = commentId,
                Content = replied
            };

            db.Replies.Add(reply);
            db.SaveChanges();
            return RedirectToAction("details", "post", new { id = id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
