using K53Forum.CodeHelper;
using K53Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace K53Forum.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        DbK53Forum db = new DbK53Forum();
        // GET: Admin/Home
        public ActionResult Index()
        {
            CurrentAction.currentAction = "home";
            return View();
        }
        [ChildActionOnly]
        public ActionResult _MemberLogin()
        {
            CurrentAction.currentAction = "home";
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