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
    public class UrlController : Controller
    {
        private iCrawlerEntities db = new iCrawlerEntities();

        //
        // GET: /Url/

        public ActionResult Index()
        {
            return View(db.Urls.ToList());
        }

        public ActionResult IndexOf(int userId)
        {
            return View("IndexOf",db.UrlOfUsers.Where(c=>c.UserId == userId).ToList());
        }
        //
        // GET: /Url/Details/5

        public ActionResult Details(Guid id)
        {
            Url url = db.Urls.Find(id);
            if (url == null)
            {
                return HttpNotFound();
            }
            return View(url);
        }

        public ActionResult Like(Guid urlId)
        {       
            int _userId;
            try
            {
                UrlOfUser _object = new UrlOfUser();
                 _userId = WebSecurity.GetUserId(User.Identity.Name);
                _object.UserId = _userId;
                _object.UrlId = urlId;

                db.UrlOfUsers.Add(_object);
                db.SaveChanges();
                return RedirectToAction("IndexOf", "Url", new {userId = _userId});           
            }
            catch
            {
                return RedirectToAction("Error");
            }

            
        }
        //
        // GET: /Url/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Url/Create

        [HttpPost]
        [Authorize]
        public ActionResult Create(Url url)
        {
            if (ModelState.IsValid)
            {
                url.Id = Guid.NewGuid();
                db.Urls.Add(url);
                try
                {
                    db.SaveChanges();

                    UrlOfUser _object = new UrlOfUser();
                    int _userId = WebSecurity.GetUserId(User.Identity.Name);
                    _object.UserId = _userId;
                    _object.UrlId = url.Id;

                    db.UrlOfUsers.Add(_object);
                    db.SaveChanges();
                }
                catch
                {
                    return RedirectToAction("Error");
                }
                return RedirectToAction("Index");
            }

            return View(url);
        }

        //
        // GET: /Url/Edit/5

        public ActionResult Edit(Guid id)
        {
            Url url = db.Urls.Find(id);
            if (url == null)
            {
                return HttpNotFound();
            }
            return View(url);
        }

        //
        // POST: /Url/Edit/5

        [HttpPost]
        public ActionResult Edit(Url url)
        {
            if (ModelState.IsValid)
            {
                db.Entry(url).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(url);
        }

        //
        // GET: /Url/Delete/5

        public ActionResult Delete(Guid id)
        {
            Url url = db.Urls.Find(id);
            if (url == null)
            {
                return HttpNotFound();
            }
            return View(url);
        }

        //
        // POST: /Url/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Url url = db.Urls.Find(id);
            db.Urls.Remove(url);
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