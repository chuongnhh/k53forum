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
    public class ArticleController : Controller
    {
        private DbK53Forum db = new DbK53Forum();

        // GET: Post
        public ActionResult Index()
        {
            CurrentAction.currentAction = "article";
            var articles = db.Articles.Include(p => p.Category).Include(p => p.Member);
            return View(articles.ToList());
        }

        // GET: Post/Details/5
        public ActionResult Details(long? id)
        {
            //ViewBag.Comment = db.Posts.Find(id).Comments;
            CurrentAction.currentAction = "article";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            article.Count++;
            db.SaveChanges();
            return View(article);
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            CurrentAction.currentAction = "article";
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Content,DateCreated,Liked,Disliked,MemberId,CatagoryId")] Article article)
        {
            CurrentAction.currentAction = "article";
            if (ModelState.IsValid)
            {
                string username = Membership.GetUser().UserName;
                long id = db.Members
                    .Where(x => x.Username == username)
                    .FirstOrDefault<Member>().Id;
                article.MemberId = id;
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("details", "article", new { id = article.Id });
            }

            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", article.CatagoryId);
            return View(article);
        }

        // GET: Post/Edit/5
        public ActionResult Edit(long? id)
        {
            CurrentAction.currentAction = "article";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", article.CatagoryId);
            return View(article);
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
        public JsonResult Edit([Bind(Include = "Id,Title,Content,CatagoryId")] Article article)
        {
            CurrentAction.currentAction = "article";
            if (ModelState.IsValid)
            {
                var p = db.Articles.Find(article.Id);
                p.Title = article.Title;
                p.Content = article.Content;
                p.CatagoryId = article.CatagoryId;

                db.SaveChanges();
                return Json(new { status = true, name = article.Title,id=article.Id });
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", article.CatagoryId);
            return Json(new { status = false, name = article.Title, id = article.Id });
        }

        // GET: Post/Delete/5
        public ActionResult Delete(long? id)
        {
            CurrentAction.currentAction = "article";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CurrentAction.currentAction = "article";
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
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

            Liked liked = db.Likeds
                .Where(x => x.ArticleId == id && x.MemberId == memberId)
                .FirstOrDefault<Liked>();
            if (liked != null)
            {
                db.Likeds.Remove(liked);
                db.SaveChanges();
            }
            else
            {
                liked = new Liked
                {
                    MemberId = memberId,
                    ArticleId = id
                };

                db.Likeds.Add(liked);
                db.SaveChanges();
                status = true;
            }
            var article = db.Articles.Where(x => x.Id == id).FirstOrDefault<Article>();

            return Json(new { status = status, count = article != null ? article.Likeds.Count() : 0 });
        }



        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Commented(long id, string commented)
        {
            if (ModelState.IsValid)
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
                return Json(new
                {
                    status = true,
                    fullname = comment.Member.Fullname,
                    avatar = comment.Member.Avatar,
                    content = comment.Content,
                    dateCreated = comment.DateCreated.ToString(),
                    memberId = comment.MemberId
                });
            }

            return Json(new { status = false });//RedirectToAction("details", "article", new { id = id });
        }

        [HttpPost]
        public JsonResult DeleteComment(long id)
        {
            if (ModelState.IsValid)
            {
                Comment r = db.Comments.Find(id);

                db.Comments.Remove(r);
                db.SaveChanges();
                return Json(new
                {
                    status = true,
                    id = id
                });
            }

            return Json(new { status = false, id = id });

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
