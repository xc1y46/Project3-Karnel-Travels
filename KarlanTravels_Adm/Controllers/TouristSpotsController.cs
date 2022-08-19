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
            var touristSpots = db.TouristSpots.Include(t => t.City).Include(t => t.SubCategory);
            return View(touristSpots.ToList());
        }

        // GET: TouristSpots/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TouristSpot touristSpot = db.TouristSpots.Find(id);
            if (touristSpot == null)
            {
                return HttpNotFound();
            }
            return View(touristSpot);
        }

        // GET: TouristSpots/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName");
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName");
            return View();
        }

        // POST: TouristSpots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TouristSpotId,TouristSpotName,CityId,SubCategoryId,TouristSpotLocation,TouristSpotRating,OpenHour,ClosingHour,TouristSpotAvailability,TouristSpotImage,TouristSpotNote,Deleted")] TouristSpot touristSpot)
        {
            if (ModelState.IsValid)
            {
                db.TouristSpots.Add(touristSpot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", touristSpot.CityId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", touristSpot.SubCategoryId);
            return View(touristSpot);
        }

        // GET: TouristSpots/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TouristSpot touristSpot = db.TouristSpots.Find(id);
            if (touristSpot == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", touristSpot.CityId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", touristSpot.SubCategoryId);
            return View(touristSpot);
        }

        // POST: TouristSpots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TouristSpotId,TouristSpotName,CityId,SubCategoryId,TouristSpotLocation,TouristSpotRating,OpenHour,ClosingHour,TouristSpotAvailability,TouristSpotImage,TouristSpotNote,Deleted")] TouristSpot touristSpot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(touristSpot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", touristSpot.CityId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "SubCategoryId", "SubCategoryName", touristSpot.SubCategoryId);
            return View(touristSpot);
        }

        // GET: TouristSpots/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TouristSpot touristSpot = db.TouristSpots.Find(id);
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
            TouristSpot touristSpot = db.TouristSpots.Find(id);
            db.TouristSpots.Remove(touristSpot);
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
