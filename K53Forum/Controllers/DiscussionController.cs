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

namespace K53Forum.Controllers
{
    public class DiscussionController : Controller
    {
        private DbK53Forum db = new DbK53Forum();

        // GET: Discussion
        public async Task<ActionResult> Index()
        {
            CurrentAction.currentAction = "discussion";
            var discussions = db.Discussions.Include(d => d.Category).Include(d => d.Member);
            return View(await discussions.ToListAsync());
        }

        // GET: Discussion/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            CurrentAction.currentAction = "discussion";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discussion discussion = await db.Discussions.FindAsync(id);
            if (discussion == null)
            {
                return HttpNotFound();
            }
            discussion.Count++;
            db.SaveChanges();
            return View(discussion);
        }

        // GET: Discussion/Create
        public ActionResult Create()
        {
            CurrentAction.currentAction = "discussion";
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username");
            return View();
        }

        // POST: Discussion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Content,DateCreated,Count,MemberId,CatagoryId")] Discussion discussion)
        {
            CurrentAction.currentAction = "discussion";
            if (ModelState.IsValid)
            {
                string username = Membership.GetUser().UserName;
                long id = db.Members
                    .Where(x => x.Username == username)
                    .FirstOrDefault<Member>().Id;
                discussion.MemberId = id;

                db.Discussions.Add(discussion);
                await db.SaveChangesAsync();
                return RedirectToAction("details", "discussion", new { id = discussion.Id });
            }

            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", discussion.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", discussion.MemberId);
            return View(discussion);
        }

        // GET: Discussion/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            CurrentAction.currentAction = "discussion";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discussion discussion = await db.Discussions.FindAsync(id);
            if (discussion == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", discussion.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", discussion.MemberId);
            return View(discussion);
        }

        // POST: Discussion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Content,DateCreated,Count,MemberId,CatagoryId")] Discussion discussion)
        {
            CurrentAction.currentAction = "discussion";
            if (ModelState.IsValid)
            {
                string username = Membership.GetUser().UserName;
                long id = db.Members
                    .Where(x => x.Username == username)
                    .FirstOrDefault<Member>().Id;
                discussion.MemberId = id;

                db.Entry(discussion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("details", "discussion", new { id = discussion.Id });
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", discussion.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", discussion.MemberId);
            return View(discussion);
        }

        // GET: Discussion/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            CurrentAction.currentAction = "discussion";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discussion discussion = await db.Discussions.FindAsync(id);
            if (discussion == null)
            {
                return HttpNotFound();
            }
            return View(discussion);
        }

        // POST: Discussion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            CurrentAction.currentAction = "discussion";
            Discussion discussion = await db.Discussions.FindAsync(id);
            db.Discussions.Remove(discussion);
            await db.SaveChangesAsync();
            return RedirectToAction("index");
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Reply(long id, string reply)
        {
            if (ModelState.IsValid)
            {
                string username = Membership.GetUser().UserName;
                var memberId = db.Members
                    .Where(x => x.Username == username)
                    .FirstOrDefault<Member>().Id;

                Reply r = new Reply()
                {
                    MemberId = memberId,
                    DiscussId = id,
                    Content = reply
                };

                db.Replies.Add(r);
                db.SaveChanges();
                return Json(new
                {
                    status = true,
                    id = r.Id,
                    fullname = r.Member.Fullname,
                    avatar = r.Member.Avatar,
                    content = r.Content,
                    dateCreated = r.DateCreated.ToString(),
                    memberId = r.MemberId
                });
            }

            return Json(new { status = false });
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult DeleteReply(long id)
        {
            if (ModelState.IsValid)
            {
                Reply r = db.Replies.Find(id);

                db.Replies.Remove(r);
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
