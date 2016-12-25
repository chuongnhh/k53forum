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
    public class DiscussionController : Controller
    {
        private DbK53Forum db = new DbK53Forum();

        // GET: Admin/Discussions
        public async Task<ActionResult> Index()
        {
            CurrentAction.currentAction = "discussion";
            var discussions = db.Discussions.Include(d => d.Category).Include(d => d.Member);
            return View(await discussions.ToListAsync());
        }

        // GET: Admin/Discussions/Details/5
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
            return View(discussion);
        }

        // GET: Admin/Discussions/Create
        public ActionResult Create()
        {
            CurrentAction.currentAction = "discussion";
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username");
            return View();
        }

        // POST: Admin/Discussions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                return RedirectToAction("index");
            }

            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", discussion.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", discussion.MemberId);
            return View(discussion);
        }

        // GET: Admin/Discussions/Edit/5
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

        // POST: Admin/Discussions/Edit/5
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
                return RedirectToAction("index");
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", discussion.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", discussion.MemberId);
            return View(discussion);
        }

        // GET: Admin/Discussions/Delete/5
        //public async Task<ActionResult> Delete(long? id)
        //{
        //    CurrentAction.currentAction = "discussion";
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Discussion discussion = await db.Discussions.FindAsync(id);
        //    if (discussion == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(discussion);
        //}

        //// POST: Admin/Discussions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(long id)
        //{
        //    CurrentAction.currentAction = "discussion";
        //    Discussion discussion = await db.Discussions.FindAsync(id);
        //    db.Discussions.Remove(discussion);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("index");
        //}

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            CurrentAction.currentAction = "discussion";
            Discussion discussion = await db.Discussions.FindAsync(id);
            if (discussion == null) return Json(new { status = false });

            db.Replies.RemoveRange(discussion.Replies);
            await db.SaveChangesAsync();

            db.Discussions.Remove(discussion);
            await db.SaveChangesAsync();

            return Json(new { status = true, id = id });
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
