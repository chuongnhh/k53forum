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
        public JsonResult Create([Bind(Include = "Id,Username,Password,Fullname,Email")] Member member)
        {
            if (ModelState.IsValid)
            {
                var role = db.Roles
                    .Where(x => x.RoleName == "Member")
                    .FirstOrDefault<Role>();
                member.RoleId = role.Id;
                db.Members.Add(member);
                db.SaveChanges();
                (new HomeController()).Login(member.Username, member.Password);
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
        public JsonResult Edit([Bind(Include = "Id,Username,Password,Fullname,Birthday,Gender,Email,Phone,Address,Avatar")] Member member)
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

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Image/"), fileName);
                        file.SaveAs(path);
                    }
                }
                db.SaveChanges();
                return Json(new { status = "success", fullname = member.Fullname, id = member.Id });
            }
            return Json(new { status = "errorr", fullname = member.Fullname, id = member.Id });
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
        public ActionResult Password([Bind(Include = "Id,Password,NewPassword,ConfirmPassword")] Member member)
        {
            CurrentAction.currentAction = "";
            if (ModelState.IsValid)
            {
                var m = db.Members.Find(member.Id);

                if (m.Password == member.Password &&
                    member.NewPassword == member.ConfirmPassword)
                {
                    m.Password = member.NewPassword;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public JsonResult ChangeAvatar()
        {
            if (Membership.GetUser() != null)
            {
                foreach (string fileName in Request.Files)
                {
                    //Lấy ra file trong list các file gởi lên
                    HttpPostedFileBase file = Request.Files[fileName];
                    //Save file content goes here

                    if (file != null && file.ContentLength > 0)
                    {
                        //Định nghĩa đường dẫn lưu file trên server
                        //ở đây mình lưu tại đường dẫn yourdomain.com/Uploads/
                        var originalDirectory =
                            new DirectoryInfo(string.Format("{0}Uploads\\", Server.MapPath(@"\")));
                        //Lưu trữ hình ảnh theo từng tháng trong năm
                        string pathString = Path.Combine(originalDirectory.ToString(), DateTime.Now.ToString("yyyy-MM"));
                        bool isExists = Directory.Exists(pathString);
                        if (!isExists) Directory.CreateDirectory(pathString);
                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        string newFileName = file.FileName;
                        //lấy đường dẫn lưu file sau khi kiểm tra tên file trên server có tồn tại hay không
                        var newPath = GetNewPathForDupes(path, ref newFileName);
                        string serverPath = string.Format("/{0}/{1}/{2}", "Uploads", DateTime.Now.ToString("yyyy-MM"), newFileName);
                        //Lưu hình ảnh Resize từ file sử dụng file.InputStream
                        SaveResizeImage(Image.FromStream(file.InputStream), 500, newPath);

                        var username = Membership.GetUser().UserName;
                        Member mem = db.Members
                            .Where(x => x.Username == username)
                            .FirstOrDefault();
                        mem.Avatar = serverPath;
                        db.SaveChanges();
                        return Json(new { status = true, file = serverPath });
                    }
                }
            }
            return Json(new { status = false, file = "" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Hàm resize hình ảnh
        public bool SaveResizeImage(Image img, int width, string path)
        {
            try
            {
                // lấy chiều rộng và chiều cao ban đầu của ảnh
                int originalW = img.Width;
                int originalH = img.Height;
                // lấy chiều rộng và chiều cao mới tương ứng với chiều rộng truyền vào của ảnh (nó sẽ giúp ảnh của chúng ta sau khi resize vần giứ được độ cân đối của tấm ảnh
                int resizedW = width;
                int resizedH = (originalH * resizedW) / originalW;
                Bitmap b = new Bitmap(resizedW, resizedH);
                Graphics g = Graphics.FromImage((Image)b);
                g.InterpolationMode = InterpolationMode.Bicubic;    // Specify here
                g.DrawImage(img, 0, 0, resizedW, resizedH);
                g.Dispose();
                b.Save(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        private string GetNewPathForDupes(string path, ref string fn)
        {
            string directory = Path.GetDirectoryName(path);
            string filename = Path.GetFileNameWithoutExtension(path);
            string extension = Path.GetExtension(path);
            int counter = 1;
            string newFullPath = path;
            string new_file_name = filename + extension;
            while (System.IO.File.Exists(newFullPath))
            {
                string newFilename = string.Format("{0}({1}){2}", filename, counter, extension);
                new_file_name = newFilename;
                newFullPath = Path.Combine(directory, newFilename);
                counter++;
            };
            fn = new_file_name;
            return newFullPath;
        }

    }
}
