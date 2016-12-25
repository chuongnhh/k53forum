using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using K53Forum.Models;
using K53Forum.CodeHelper;
using System.Web.Security;

namespace K53Forum.Areas.Admin.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private DbK53Forum db = new DbK53Forum();

        // GET: Admin/Articles
        public async Task<ActionResult> Index()
        {
            CurrentAction.currentAction = "article";
            var articles = db.Articles.Include(a => a.Category).Include(a => a.Member);
            return View(await articles.ToListAsync());
        }

        // GET: Admin/Articles/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            CurrentAction.currentAction = "article";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Admin/Articles/Create
        public ActionResult Create()
        {
            CurrentAction.currentAction = "article";
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username");
            return View();
        }

        // POST: Admin/Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Content,DateCreated,Count,MemberId,CatagoryId")] Article article)
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
                await db.SaveChangesAsync();
                return RedirectToAction("index");
            }

            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", article.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", article.MemberId);
            return View(article);
        }

        // GET: Admin/Articles/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            CurrentAction.currentAction = "article";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", article.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", article.MemberId);
            return View(article);
        }

        // POST: Admin/Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Content,DateCreated,Count,MemberId,CatagoryId")] Article article)
        {
            CurrentAction.currentAction = "article";
            if (ModelState.IsValid)
            {
                string username = Membership.GetUser().UserName;
                long id = db.Members
                    .Where(x => x.Username == username)
                    .FirstOrDefault<Member>().Id;
                article.MemberId = id;

                db.Entry(article).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("index");
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", article.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", article.MemberId);
            return View(article);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            CurrentAction.currentAction = "article";
            Article article = await db.Articles.FindAsync(id);

            if (article == null) return Json(new { status = false });

            // assuming this is your DbSet
            db.Comments.RemoveRange(article.Comments);
            db.Likeds.RemoveRange(article.Likeds);

            await db.SaveChangesAsync();

            db.Articles.Remove(article);
            await db.SaveChangesAsync();

            return Json(new { status = true, id = id });//RedirectToAction("index");
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
