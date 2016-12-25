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
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace K53Forum.Controllers
{
    public class MemberController : Controller
    {
        private DbK53Forum db = new DbK53Forum();

        // GET: Members
        public ActionResult Index()
        {
            CurrentAction.currentAction = "member";
            var members = db.Members.Include(m => m.Role);
            return View(members.OrderByDescending(x => x.Id).ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(long? id)
        {
            CurrentAction.currentAction = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            if (Membership.GetUser() != null && Membership.GetUser().UserName != member.Username)
            {
                member.Count++;
                db.SaveChanges();
            }
            return View(member);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create([Bind(Include = "Id,Username,Password,ConfirmPassword,Fullname,Email")] Member member)
        {
            if (ModelState.IsValid)
            {
                string password = member.Password;

                if (member.Password != member.ConfirmPassword)
                {
                    return Json(new { status = false });
                }
                var role = db.Roles
                    .Where(x => x.RoleName == "Member")
                    .FirstOrDefault<Role>();
                member.RoleId = role.Id;
                // mã  hóa mật khẩu
                member.Password = Encryption.GenerateMD5(member.Password);
                member.Avatar = "/Uploads/avatar.png";
                db.Members.Add(member);
                db.SaveChanges();
                (new HomeController()).Login(member.Username, password);
                return Json(new { status = true });
            }

            ViewBag.RoleId = new SelectList(db.Roles, "Id", "RoleName", member.RoleId);
            return Json(new { status = false });
        }

        // GET: Members/Edit/5
        public ActionResult Edit(long? id)
        {
            CurrentAction.currentAction = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,Fullname,Birthday,Gender,Email,Phone,Address")] Member member)
        {
            CurrentAction.currentAction = "";
            if (ModelState.IsValid)
            {
                var m = db.Members.Find(member.Id);
                m.Fullname = member.Fullname;
                m.Birthday = member.Birthday;
                m.Gender = member.Gender;
                m.Email = member.Email;
                m.Phone = member.Phone;
                m.Address = member.Address;

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
                        try
                        {
                            System.IO.File.Delete(Server.MapPath(m.Avatar));
                        }
                        catch { }

                        m.Avatar = filePathSave;
                    }
                    db.SaveChanges();
                    return RedirectToAction("details", "member", new { id = m.Id });
                }
                catch { }
                //return Json(new { status = "success", fullname = member.Fullname, id = member.Id });
            }
            return View(member);
            //return Json(new { status = "errorr", fullname = member.Fullname, id = member.Id });
        }

        // GET: Members/Edit/5
        public ActionResult Password(long? id)
        {
            CurrentAction.currentAction = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Password([Bind(Include = "Id,Password,NewPassword,ConfirmPassword")] Member member)
        {
            CurrentAction.currentAction = "";
            if (ModelState.IsValid)
            {
                var m = db.Members.Find(member.Id);

                if (member.NewPassword == member.ConfirmPassword)
                {
                    m.Password = Encryption.GenerateMD5(member.NewPassword);
                    db.SaveChanges();
                    return Json(new { status = true });
                }
            }
            return Json(new { status = false });//RedirectToAction("index", "home");
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
