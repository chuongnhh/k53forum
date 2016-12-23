using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using K53Forum.CodeHelper;
using K53Forum.Models;
using System.Web.Security;
using System.Security.Policy;
using PagedList;

namespace K53Forum.Controllers
{
    public class HomeController : Controller
    {
        DbK53Forum db = new DbK53Forum();

        public ActionResult Index(int? page)
        {
            var model = db.Posts.OrderByDescending(x => x.Id).ToList<Post>();

            CurrentAction.currentAction = "home";

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(model.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            CurrentAction.currentAction = "about";
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Term()
        {
            CurrentAction.currentAction = "term";
            ViewBag.Message = "Term in forum";

            return View();
        }

        [ChildActionOnly]
        public ActionResult _Carousel()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult _MenuBar()
        {
            ViewBag.Category = db.Categories.OrderByDescending(x => x.Id).ToList<Category>();
            ViewBag.Post = db.Posts.OrderByDescending(x => x.Id).ToList<Post>();
            ViewBag.Count = db.Categories.OrderByDescending(x => x.Id).Count().ToString();
            return PartialView();
        }

        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            if (Membership.ValidateUser(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, false);
                //Uri url = Request.Url;
                //return Redirect(url.ToString());
                return Json(new { status = true,
                    name = db.Members
                    .Where(x => x.Username == username)
                    .FirstOrDefault().Username });
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
            return RedirectToAction("index", "home");
        }

        [ChildActionOnly]
        public ActionResult _MemberLogin()
        {
            if (Membership.GetUser() != null)
            {
                string username = Membership.GetUser().UserName;
                var model = db.Members
               .Where(x => x.Username == username)
               .FirstOrDefault<Member>();
                return PartialView(model);
            }
            return PartialView();
        }

    }
}