using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using K53Forum.Models;
using PagedList;
using K53Forum.CodeHelper;

namespace K53Forum.Controllers
{
    public class CategoryController : Controller
    {
        private DbK53Forum db = new DbK53Forum();

        // GET: Category
        public ActionResult Index()
        {
            CurrentAction.currentAction = "";
            return View(db.Categories.OrderByDescending(x => x.Id).ToList());
        }

        // GET: Category/Details/5
        public ActionResult Details(long? id)
        {
            CurrentAction.currentAction = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.Categories.Find(id);
            ViewBag.Title = model.Name;
            return View(model);
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
