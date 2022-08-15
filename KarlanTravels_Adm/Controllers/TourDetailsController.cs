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
    public class TourDetailsController : Controller
    {
        private ContextModel db = new ContextModel();

        // GET: TourDetails
        public ActionResult Index()
        {
            var tourDetail = db.TourDetail.Include(t => t.Facility).Include(t => t.Tour).Include(t => t.TouristSpot);
            return View(tourDetail.ToList());
        }

        // GET: TourDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourDetail tourDetail = db.TourDetail.Find(id);
            if (tourDetail == null)
            {
                return HttpNotFound();
            }
            return View(tourDetail);
        }

        // GET: TourDetails/Create
        public ActionResult Create()
        {
            ViewBag.FacilityId = new SelectList(db.Facility, "FacilityId", "FacilityName");
            ViewBag.TourId = new SelectList(db.Tour, "TourId", "TourName");
            ViewBag.TouristSpotId = new SelectList(db.TouristSpot, "TouristSpotId", "TouristSpotName");
            return View();
        }

        // POST: TourDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TourDetailId,TourDetailName,Activity,ActivityTimeStart,ActivityTimeEnd,TouristSpotId,FacilityId,ActivityNote,TourId,DeleteFlag")] TourDetail tourDetail)
        {
            if (ModelState.IsValid)
            {
                db.TourDetail.Add(tourDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FacilityId = new SelectList(db.Facility, "FacilityId", "FacilityName", tourDetail.FacilityId);
            ViewBag.TourId = new SelectList(db.Tour, "TourId", "TourName", tourDetail.TourId);
            ViewBag.TouristSpotId = new SelectList(db.TouristSpot, "TouristSpotId", "TouristSpotName", tourDetail.TouristSpotId);
            return View(tourDetail);
        }

        // GET: TourDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourDetail tourDetail = db.TourDetail.Find(id);
            if (tourDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.FacilityId = new SelectList(db.Facility, "FacilityId", "FacilityName", tourDetail.FacilityId);
            ViewBag.TourId = new SelectList(db.Tour, "TourId", "TourName", tourDetail.TourId);
            ViewBag.TouristSpotId = new SelectList(db.TouristSpot, "TouristSpotId", "TouristSpotName", tourDetail.TouristSpotId);
            return View(tourDetail);
        }

        // POST: TourDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TourDetailId,TourDetailName,Activity,ActivityTimeStart,ActivityTimeEnd,TouristSpotId,FacilityId,ActivityNote,TourId,DeleteFlag")] TourDetail tourDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tourDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FacilityId = new SelectList(db.Facility, "FacilityId", "FacilityName", tourDetail.FacilityId);
            ViewBag.TourId = new SelectList(db.Tour, "TourId", "TourName", tourDetail.TourId);
            ViewBag.TouristSpotId = new SelectList(db.TouristSpot, "TouristSpotId", "TouristSpotName", tourDetail.TouristSpotId);
            return View(tourDetail);
        }

        // GET: TourDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourDetail tourDetail = db.TourDetail.Find(id);
            if (tourDetail == null)
            {
                return HttpNotFound();
            }
            return View(tourDetail);
        }

        // POST: TourDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TourDetail tourDetail = db.TourDetail.Find(id);
            db.TourDetail.Remove(tourDetail);
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
