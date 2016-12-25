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
using System.IO;
using System.Web.Security;
using K53Forum.CodeHelper;

namespace K53Forum.Areas.Admin.Controllers
{
    [Authorize]
    public class CarouselController : Controller
    {
        private DbK53Forum db = new DbK53Forum();

        // GET: Admin/Carousel
        public async Task<ActionResult> Index()
        {
            CurrentAction.currentAction = "carousel";
            return View(await db.Carousels.ToListAsync());
        }

        // GET: Admin/Carousel/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            CurrentAction.currentAction = "carousel";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = await db.Carousels.FindAsync(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }
            return View(carousel);
        }

        // GET: Admin/Carousel/Create
        public ActionResult Create()
        {
            CurrentAction.currentAction = "carousel";
            return View();
        }

        // POST: Admin/Carousel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Image,Content,Title")] Carousel carousel)
        {
            CurrentAction.currentAction = "carousel";
            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Files.Count > 0&&!string.IsNullOrEmpty(Request.Files[0].FileName))
                    {
                        var original = new DirectoryInfo(string.Format("{0}Uploads\\", Server.MapPath(@"\")));
                        //Lưu trữ hình ảnh theo từng tháng trong năm
                        string path = Path.Combine(original.ToString(), DateTime.Now.ToString("MM-yyyy"));

                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                        string filePathSave = "/Uploads/" + DateTime.Now.ToString("MM-yyyy") + "/" + Request.Files[0].FileName;
                        Request.Files[0].SaveAs(Server.MapPath(filePathSave));

                        carousel.Image = filePathSave;
                    }
                }
                catch { }
                carousel.CreatedBy = Membership.GetUser().UserName;
                db.Carousels.Add(carousel);
                await db.SaveChangesAsync();
                return RedirectToAction("details", "carousel", new { id = carousel.Id });
            }

            return View(carousel);
        }

        // GET: Admin/Carousel/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            CurrentAction.currentAction = "carousel";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carousel carousel = await db.Carousels.FindAsync(id);
            if (carousel == null)
            {
                return HttpNotFound();
            }
            return View(carousel);
        }

        // POST: Admin/Carousel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Image,Content,Title")] Carousel carousel)
        {
            CurrentAction.currentAction = "carousel";
            if (ModelState.IsValid)
            {
                Carousel c = db.Carousels.Find(carousel.Id);
                try
                {
                    if (Request.Files.Count > 0 && !string.IsNullOrEmpty(Request.Files[0].FileName))
                    {
                        var original = new DirectoryInfo(string.Format("{0}Uploads\\", Server.MapPath(@"\")));
                        //Lưu trữ hình ảnh theo từng tháng trong năm
                        string path = Path.Combine(original.ToString(), DateTime.Now.ToString("MM-yyyy"));

                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                        string filePathSave = "/Uploads/" + DateTime.Now.ToString("MM-yyyy") + "/" + Request.Files[0].FileName;
                        Request.Files[0].SaveAs(Server.MapPath(filePathSave));

                        c.Image = filePathSave;
                    }
                }
                catch { }
                c.Title = carousel.Title;
                c.Content = carousel.Content;
                c.CreatedBy = Membership.GetUser().UserName;

                await db.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(carousel);
        }

        //// GET: Admin/Carousel/Delete/5
        //public async Task<ActionResult> Delete(long? id)
        //{
        //    CurrentAction.currentAction = "carousel";
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Carousel carousel = await db.Carousels.FindAsync(id);
        //    if (carousel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(carousel);
        //}

        //// POST: Admin/Carousel/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(long id)
        //{
        //    CurrentAction.currentAction = "carousel";
        //    Carousel carousel = await db.Carousels.FindAsync(id);
        //    db.Carousels.Remove(carousel);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("index");
        //}

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            CurrentAction.currentAction = "carousel";
            Carousel carousel = await db.Carousels.FindAsync(id);
            if (carousel == null) return Json(new { status = false });

            db.Carousels.Remove(carousel);
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
