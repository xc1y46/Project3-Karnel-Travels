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
    public class ToursController : Controller
    {
        private ContextModel db = new ContextModel();

        // GET: Tours
        public ActionResult Index()
        {
            var tours = db.Tours.Include(t => t.Category).Include(t => t.Category1);
            return View(tours.ToList());
        }

        // GET: Tours/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // GET: Tours/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId1 = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.CategoryId2 = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Tours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TourId,TourName,TourAvailability,TourStart,TourEnd,TourPrice,CategoryId1,CategoryId2,MaxBooking,BookTimeLimit,TourRating,TourImage,TourNote,Deleted")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Tours.Add(tour);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId1 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId1);
            ViewBag.CategoryId2 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId2);
            return View(tour);
        }

        // GET: Tours/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId1 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId1);
            ViewBag.CategoryId2 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId2);
            return View(tour);
        }

        // POST: Tours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TourId,TourName,TourAvailability,TourStart,TourEnd,TourPrice,CategoryId1,CategoryId2,MaxBooking,BookTimeLimit,TourRating,TourImage,TourNote,Deleted")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId1 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId1);
            ViewBag.CategoryId2 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId2);
            return View(tour);
        }

        // GET: Tours/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tour tour = db.Tours.Find(id);
            db.Tours.Remove(tour);
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
