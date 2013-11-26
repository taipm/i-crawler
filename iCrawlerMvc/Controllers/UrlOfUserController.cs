using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iCrawlerMvc.Models;
using WebMatrix.WebData;
using iCrawlerMvc.Filters;

namespace iCrawlerMvc.Controllers
{
    [InitializeSimpleMembership]
    public class UrlOfUserController : Controller
    {
        private iCrawlerEntities db = new iCrawlerEntities();

        //
        // GET: /UrlOfUser/

        public ActionResult Index()
        {
            int _userId = WebSecurity.GetUserId(User.Identity.Name);
            return View(db.UrlOfUsers.Where(c=>c.UserId == _userId).ToList());
        }

        //
        // GET: /UrlOfUser/Details/5

        public ActionResult Details(Guid urlId)
        {
            int _userId = WebSecurity.GetUserId(User.Identity.Name);
            UrlOfUser urlofuser = db.UrlOfUsers.Where(c => c.UrlId == urlId && c.UserId == _userId).FirstOrDefault();
            if (urlofuser == null)
            {
                return HttpNotFound();
            }
            return View(urlofuser);
        }

        //
        // GET: /UrlOfUser/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /UrlOfUser/Create

        [HttpPost]
        public ActionResult Create(UrlOfUser urlofuser)
        {
            if (ModelState.IsValid)
            {
                db.UrlOfUsers.Add(urlofuser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(urlofuser);
        }

        //
        // GET: /UrlOfUser/Edit/5

        public ActionResult AddEmail(Guid urlId)
        {
            int _userId = WebSecurity.GetUserId(User.Identity.Name);
            UrlOfUser urlofuser = db.UrlOfUsers.Where(c => c.UrlId == urlId && c.UserId == _userId).FirstOrDefault();
            if (urlofuser == null)
            {
                return HttpNotFound();
            }
            return View(urlofuser);
        }

        //
        // POST: /UrlOfUser/Edit/5

        [HttpPost]
        public ActionResult AddEmail(UrlOfUser urlofuser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(urlofuser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(urlofuser);
        }

        //
        // GET: /UrlOfUser/Edit/5

        public ActionResult Edit(Guid  urlId)
        {
            int _userId = WebSecurity.GetUserId(User.Identity.Name);
            UrlOfUser urlofuser = db.UrlOfUsers.Where(c => c.UrlId == urlId && c.UserId == _userId).FirstOrDefault();
            if (urlofuser == null)
            {
                return HttpNotFound();
            }
            return View(urlofuser);
        }

        //
        // POST: /UrlOfUser/Edit/5

        [HttpPost]
        public ActionResult Edit(UrlOfUser urlofuser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(urlofuser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(urlofuser);
        }

        //
        // GET: /UrlOfUser/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UrlOfUser urlofuser = db.UrlOfUsers.Find(id);
            if (urlofuser == null)
            {
                return HttpNotFound();
            }
            return View(urlofuser);
        }

        //
        // POST: /UrlOfUser/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UrlOfUser urlofuser = db.UrlOfUsers.Find(id);
            db.UrlOfUsers.Remove(urlofuser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}