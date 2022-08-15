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
    public class TransactionRecordsController : Controller
    {
        private ContextModel db = new ContextModel();

        // GET: TransactionRecords
        public ActionResult Index()
        {
            var transactionRecord = db.TransactionRecord.Include(t => t.Admin).Include(t => t.Customer).Include(t => t.Tour).Include(t => t.TransactionType);
            return View(transactionRecord.ToList());
        }

        // GET: TransactionRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionRecord transactionRecord = db.TransactionRecord.Find(id);
            if (transactionRecord == null)
            {
                return HttpNotFound();
            }
            return View(transactionRecord);
        }

        // GET: TransactionRecords/Create
        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.Admin, "AdminId", "AdminName");
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerId", "Username");
            ViewBag.TourId = new SelectList(db.Tour, "TourId", "TourName");
            ViewBag.TransactionTypeId = new SelectList(db.TransactionType, "TransactionTypeId", "TransactionTypeName");
            return View();
        }

        // POST: TransactionRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionRecordId,TransactionTypeId,TourId,CustomerID,TransactionFee,Paid,AdminID,TransactionNote,DeleteFlag")] TransactionRecord transactionRecord)
        {
            if (ModelState.IsValid)
            {
                db.TransactionRecord.Add(transactionRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminID = new SelectList(db.Admin, "AdminId", "AdminName", transactionRecord.AdminID);
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerId", "Username", transactionRecord.CustomerID);
            ViewBag.TourId = new SelectList(db.Tour, "TourId", "TourName", transactionRecord.TourId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionType, "TransactionTypeId", "TransactionTypeName", transactionRecord.TransactionTypeId);
            return View(transactionRecord);
        }

        // GET: TransactionRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionRecord transactionRecord = db.TransactionRecord.Find(id);
            if (transactionRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.Admin, "AdminId", "AdminName", transactionRecord.AdminID);
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerId", "Username", transactionRecord.CustomerID);
            ViewBag.TourId = new SelectList(db.Tour, "TourId", "TourName", transactionRecord.TourId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionType, "TransactionTypeId", "TransactionTypeName", transactionRecord.TransactionTypeId);
            return View(transactionRecord);
        }

        // POST: TransactionRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionRecordId,TransactionTypeId,TourId,CustomerID,TransactionFee,Paid,AdminID,TransactionNote,DeleteFlag")] TransactionRecord transactionRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactionRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminID = new SelectList(db.Admin, "AdminId", "AdminName", transactionRecord.AdminID);
            ViewBag.CustomerID = new SelectList(db.Customer, "CustomerId", "Username", transactionRecord.CustomerID);
            ViewBag.TourId = new SelectList(db.Tour, "TourId", "TourName", transactionRecord.TourId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionType, "TransactionTypeId", "TransactionTypeName", transactionRecord.TransactionTypeId);
            return View(transactionRecord);
        }

        // GET: TransactionRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionRecord transactionRecord = db.TransactionRecord.Find(id);
            if (transactionRecord == null)
            {
                return HttpNotFound();
            }
            return View(transactionRecord);
        }

        // POST: TransactionRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TransactionRecord transactionRecord = db.TransactionRecord.Find(id);
            db.TransactionRecord.Remove(transactionRecord);
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
