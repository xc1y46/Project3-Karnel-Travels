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
    public class FacilityTypesController : Controller
    {
        private ContextModel db = new ContextModel();

        // GET: FacilityTypes
        public ActionResult Index()
        {
            return View(db.FacilityTypes.ToList());
        }

        // GET: FacilityTypes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacilityType facilityType = db.FacilityTypes.Find(id);
            if (facilityType == null)
            {
                return HttpNotFound();
            }
            return View(facilityType);
        }

        // GET: FacilityTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FacilityTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacilityTypeId,FacilityTypeName,FacilityTypeNote,Deleted")] FacilityType facilityType)
        {
            if (ModelState.IsValid)
            {
                db.FacilityTypes.Add(facilityType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facilityType);
        }

        // GET: FacilityTypes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacilityType facilityType = db.FacilityTypes.Find(id);
            if (facilityType == null)
            {
                return HttpNotFound();
            }
            return View(facilityType);
        }

        // POST: FacilityTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FacilityTypeId,FacilityTypeName,FacilityTypeNote,Deleted")] FacilityType facilityType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facilityType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(facilityType);
        }

        // GET: FacilityTypes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacilityType facilityType = db.FacilityTypes.Find(id);
            if (facilityType == null)
            {
                return HttpNotFound();
            }
            return View(facilityType);
        }

        // POST: FacilityTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            FacilityType facilityType = db.FacilityTypes.Find(id);
            db.FacilityTypes.Remove(facilityType);
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
