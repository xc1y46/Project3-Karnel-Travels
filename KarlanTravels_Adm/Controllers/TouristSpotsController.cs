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
    public class TouristSpotsController : Controller
    {
        private ContextModel db = new ContextModel();

        // GET: TouristSpots
        public ActionResult Index()
        {
            var touristSpot = db.TouristSpot.Include(t => t.Category).Include(t => t.City);
            return View(touristSpot.ToList());
        }

        // GET: TouristSpots/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TouristSpot touristSpot = db.TouristSpot.Find(id);
            if (touristSpot == null)
            {
                return HttpNotFound();
            }
            return View(touristSpot);
        }

        // GET: TouristSpots/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName");
            ViewBag.CityId = new SelectList(db.City, "CityId", "CityName");
            return View();
        }

        // POST: TouristSpots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TouristSpotId,TouristSpotName,CityId,CategoryId,Rating,TouristSpotAvailability,TouristSpotNote,DeleteFlag")] TouristSpot touristSpot)
        {
            if (ModelState.IsValid)
            {
                db.TouristSpot.Add(touristSpot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", touristSpot.CategoryId);
            ViewBag.CityId = new SelectList(db.City, "CityId", "CityName", touristSpot.CityId);
            return View(touristSpot);
        }

        // GET: TouristSpots/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TouristSpot touristSpot = db.TouristSpot.Find(id);
            if (touristSpot == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", touristSpot.CategoryId);
            ViewBag.CityId = new SelectList(db.City, "CityId", "CityName", touristSpot.CityId);
            return View(touristSpot);
        }

        // POST: TouristSpots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TouristSpotId,TouristSpotName,CityId,CategoryId,Rating,TouristSpotAvailability,TouristSpotNote,DeleteFlag")] TouristSpot touristSpot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(touristSpot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "CategoryName", touristSpot.CategoryId);
            ViewBag.CityId = new SelectList(db.City, "CityId", "CityName", touristSpot.CityId);
            return View(touristSpot);
        }

        // GET: TouristSpots/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TouristSpot touristSpot = db.TouristSpot.Find(id);
            if (touristSpot == null)
            {
                return HttpNotFound();
            }
            return View(touristSpot);
        }

        // POST: TouristSpots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TouristSpot touristSpot = db.TouristSpot.Find(id);
            db.TouristSpot.Remove(touristSpot);
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
