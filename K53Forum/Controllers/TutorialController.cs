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

namespace K53Forum.Controllers
{
    public class TutorialController : Controller
    {
        private DbK53Forum db = new DbK53Forum();

        // GET: Tutorials
        public async Task<ActionResult> Index()
        {
            CurrentAction.currentAction = "tutorial";
            var tutorials = db.Tutorials.Include(t => t.Category).Include(t => t.Member);
            return View(await tutorials.ToListAsync());
        }

        // GET: Tutorials/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            CurrentAction.currentAction = "tutorial";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutorial tutorial = await db.Tutorials.FindAsync(id);
            if (tutorial == null)
            {
                return HttpNotFound();
            }
            tutorial.Count++;
            db.SaveChanges();
            return View(tutorial);
        }

        // GET: Tutorials/Create
        public ActionResult Create()
        {
            CurrentAction.currentAction = "tutorial";
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username");
            return View();
        }

        // POST: Tutorials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Content,DateCreated,Count,MemberId,CatagoryId")] Tutorial tutorial)
        {
            CurrentAction.currentAction = "tutorial";
            if (ModelState.IsValid)
            {
                db.Tutorials.Add(tutorial);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", tutorial.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", tutorial.MemberId);
            return View(tutorial);
        }

        // GET: Tutorials/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            CurrentAction.currentAction = "tutorial";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutorial tutorial = await db.Tutorials.FindAsync(id);
            if (tutorial == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", tutorial.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", tutorial.MemberId);
            return View(tutorial);
        }

        // POST: Tutorials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Content,DateCreated,Count,MemberId,CatagoryId")] Tutorial tutorial)
        {
            CurrentAction.currentAction = "tutorial";
            if (ModelState.IsValid)
            {
                db.Entry(tutorial).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CatagoryId = new SelectList(db.Categories, "Id", "Name", tutorial.CatagoryId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Username", tutorial.MemberId);
            return View(tutorial);
        }

        // GET: Tutorials/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            CurrentAction.currentAction = "tutorial";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutorial tutorial = await db.Tutorials.FindAsync(id);
            if (tutorial == null)
            {
                return HttpNotFound();
            }
            return View(tutorial);
        }

        // POST: Tutorials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            CurrentAction.currentAction = "tutorial";
            Tutorial tutorial = await db.Tutorials.FindAsync(id);
            db.Tutorials.Remove(tutorial);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
