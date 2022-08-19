using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KarlanTravels_Adm.Models;

namespace KarlanTravels_Adm.Controllers
{
    public class AdminRolesController : Controller
    {
        private ContextModel db = new ContextModel();

        // GET: AdminRoles
        public ActionResult Index()
        {
            return View(db.AdminRoles.ToList());
        }

        // GET: AdminRoles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminRole adminRole = db.AdminRoles.Find(id);
            if (adminRole == null)
            {
                return HttpNotFound();
            }
            return View(adminRole);
        }

        // GET: AdminRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleId,RoleName,RoleNote")] AdminRole adminRole)
        {
            if (ModelState.IsValid)
            {
                db.AdminRoles.Add(adminRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adminRole);
        }

        // GET: AdminRoles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminRole adminRole = db.AdminRoles.Find(id);
            if (adminRole == null)
            {
                return HttpNotFound();
            }
            return View(adminRole);
        }

        // POST: AdminRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleId,RoleName,RoleNote")] AdminRole adminRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adminRole);
        }

        // GET: AdminRoles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminRole adminRole = db.AdminRoles.Find(id);
            if (adminRole == null)
            {
                return HttpNotFound();
            }
            return View(adminRole);
        }

        // POST: AdminRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AdminRole adminRole = db.AdminRoles.Find(id);
            db.AdminRoles.Remove(adminRole);
            db.SaveChanges();
            return RedirectToAction("Index");
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
