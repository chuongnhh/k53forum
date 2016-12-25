using K53Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace K53Forum.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        DbK53Forum db = new DbK53Forum();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            if (Membership.ValidateUser(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, false);


                return Json(new
                {
                    status = true,
                    name = db.Members
                    .Where(x => x.Username == username)
                    .FirstOrDefault().Username
                });
            }
            else
            {
                ModelState.AddModelError("", "Đăng nhập không thành công.");
            }
            return Json(new { status = false });
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index", "login");
        }
    }
}