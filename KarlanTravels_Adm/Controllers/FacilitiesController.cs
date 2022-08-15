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
    public class FacilitiesController : Controller
    {
        private ContextModel db = new ContextModel();

        // GET: Facilities
        public ActionResult Index()
        {
            var facility = db.Facility.Include(f => f.City).Include(f => f.FacilityType);
            return View(facility.ToList());
        }

        // GET: Facilities/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facility.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // GET: Facilities/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.City, "CityId", "CityName");
            ViewBag.FacilityTypeId = new SelectList(db.FacilityType, "FacilityTypeId", "FacilityTypeName");
            return View();
        }

        // POST: Facilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacilityId,FacilityName,FacilityTypeId,FacilityLocation,CityId,FacilityPrice,FacilityQuality,Quantity,FacilityNote,FacilityAvailability,DeleteFlag")] Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Facility.Add(facility);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.City, "CityId", "CityName", facility.CityId);
            ViewBag.FacilityTypeId = new SelectList(db.FacilityType, "FacilityTypeId", "FacilityTypeName", facility.FacilityTypeId);
            return View(facility);
        }

        // GET: Facilities/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facility.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.City, "CityId", "CityName", facility.CityId);
            ViewBag.FacilityTypeId = new SelectList(db.FacilityType, "FacilityTypeId", "FacilityTypeName", facility.FacilityTypeId);
            return View(facility);
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FacilityId,FacilityName,FacilityTypeId,FacilityLocation,CityId,FacilityPrice,FacilityQuality,Quantity,FacilityNote,FacilityAvailability,DeleteFlag")] Facility facility)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facility).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.City, "CityId", "CityName", facility.CityId);
            ViewBag.FacilityTypeId = new SelectList(db.FacilityType, "FacilityTypeId", "FacilityTypeName", facility.FacilityTypeId);
            return View(facility);
        }

        // GET: Facilities/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Facility facility = db.Facility.Find(id);
            if (facility == null)
            {
                return HttpNotFound();
            }
            return View(facility);
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Facility facility = db.Facility.Find(id);
            db.Facility.Remove(facility);
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
