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
using System.IO;

namespace K53Forum.Areas.Admin.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private DbK53Forum db = new DbK53Forum();

        // GET: Admin/Member
        public async Task<ActionResult> Index()
        {
            CurrentAction.currentAction = "member";
            var members = db.Members.Include(m => m.Role);
            return View(await members.ToListAsync());
        }

        // GET: Admin/Member/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            CurrentAction.currentAction = "member";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = await db.Members.FindAsync(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Admin/Member/Create
        public ActionResult Create()
        {
            CurrentAction.currentAction = "member";
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName");
            return View();
        }

        // POST: Admin/Member/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Username,Password,Fullname,Birthday,Gender,Email,Phone,Address,DateCreated,Count,RoleId,NewPassword,ConfirmPassword")] Member member)
        {
            CurrentAction.currentAction = "member";
            if (ModelState.IsValid)
            {
                try
                {
                    if (Request.Files.Count > 0)
                    {
                        var original = new DirectoryInfo(string.Format("{0}Uploads\\", Server.MapPath(@"\")));
                        //Lưu trữ hình ảnh theo từng tháng trong năm
                        string path = Path.Combine(original.ToString(), DateTime.Now.ToString("MM-yyyy"));

                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                        string filePathSave = "/Uploads/" + DateTime.Now.ToString("MM-yyyy") + "/" + Request.Files[0].FileName;
                        Request.Files[0].SaveAs(Server.MapPath(filePathSave));

                        member.Avatar = filePathSave;
                    }

                    db.Members.Add(member);
                }
                catch { }
                await db.SaveChangesAsync();
                return RedirectToAction("details", "member", new { id = member.Id });
            }

            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", member.RoleId);
            return View(member);
        }

        // GET: Admin/Member/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            CurrentAction.currentAction = "member";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = await db.Members.FindAsync(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", member.RoleId);
            return View(member);
        }

        // POST: Admin/Member/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,Username,Password,Fullname,Birthday,Gender,Email,Phone,Address,DateCreated,Count,RoleId,NewPassword,ConfirmPassword")] Member member)
        //{
        //    CurrentAction.currentAction = "member";
        //    if (ModelState.IsValid)
        //    {
        //        Member m = db.Members.Find(member.Id);
        //        try
        //        {

        //            if (Request.Files.Count > 0&& !string.IsNullOrEmpty(Request.Files[0].FileName))
        //            {
        //                var original = new DirectoryInfo(string.Format("{0}Uploads\\", Server.MapPath(@"\")));
        //                //Lưu trữ hình ảnh theo từng tháng trong năm
        //                string path = Path.Combine(original.ToString(), DateTime.Now.ToString("MM-yyyy"));

        //                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        //                string filePathSave = "/Uploads/" + DateTime.Now.ToString("MM-yyyy") + "/" + Request.Files[0].FileName;
        //                Request.Files[0].SaveAs(Server.MapPath(filePathSave));
        //                try
        //                {
        //                    System.IO.File.Delete(Server.MapPath(member.Avatar));
        //                }
        //                catch(Exception) { }

        //                m.Avatar = filePathSave;
        //            }
        //        }
        //        catch { }
        //        m.Fullname = member.Fullname;
        //        m.Birthday = member.Birthday;
        //        m.Gender = member.Gender;
        //        m.Email = member.Email;
        //        m.Phone = member.Phone;
        //        m.Address = member.Address;
        //        m.RoleId = member.RoleId;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("details", "member", new { id = member.Id });
        //    }
        //    ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", member.RoleId);
        //    return View(member);
        //}

        //// GET: Admin/Member/Delete/5
        //public async Task<ActionResult> Delete(long? id)
        //{
        //    CurrentAction.currentAction = "member";
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Member member = await db.Members.FindAsync(id);
        //    if (member == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(member);
        //}

        //// POST: Admin/Member/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(long id)
        //{
        //    CurrentAction.currentAction = "member";
        //    Member member = await db.Members.FindAsync(id);
        //    db.Members.Remove(member);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("index");
        //}

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            CurrentAction.currentAction = "member";
            Member member = await db.Members.FindAsync(id);

            if (member == null) return Json(new { status = false });

            db.Replies.RemoveRange(member.Replies);
            db.Articles.RemoveRange(member.Articles);
            db.Tutorials.RemoveRange(member.Tutorials);
            db.Discussions.RemoveRange(member.Discussions);
            db.Comments.RemoveRange(member.Comments);

            await db.SaveChangesAsync();

            db.Members.Remove(member);
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
