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

        public ActionResult Index()
        {
            var model = db.Articles.OrderByDescending(x => x.Id).Take(10).ToList<Article>();

            CurrentAction.currentAction = "home";

            //int pageSize = 20;
            //int pageNumber = (page ?? 1);

            //return View(model.ToPagedList(pageNumber, pageSize));
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult _Carousel()
        {
            var model = db.Carousels
                .OrderByDescending(x => x.Id)
                .ToList<Carousel>();
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult _MenuBar()
        {
            ViewBag.Categories = db.Categories.OrderByDescending(x => x.Id).Take(10).ToList<Category>();
            ViewBag.Articles = db.Articles.OrderByDescending(x => x.Id).Take(10).ToList<Article>();
            ViewBag.Discussions = db.Discussions.OrderByDescending(x => x.Id).Take(10).ToList<Discussion>();
            ViewBag.Tutorials = db.Tutorials.OrderByDescending(x => x.Id).Take(10).ToList<Tutorial>();

            ViewBag.Members = db.Members
                .OrderByDescending(x => x.Articles.Count() + x.Tutorials.Count() + x.Discussions.Count())
                .Take(10).ToList<Member>();

            ViewBag.Count = db.Categories.OrderByDescending(x => x.Id).Count().ToString();
            return PartialView();
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