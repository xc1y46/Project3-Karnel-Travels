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
            var transactionRecords = db.TransactionRecords.Include(t => t.Admin).Include(t => t.Customer).Include(t => t.Tour).Include(t => t.TransactionType);
            return View(transactionRecords.ToList());
        }

        // GET: TransactionRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionRecord transactionRecord = db.TransactionRecords.Find(id);
            if (transactionRecord == null)
            {
                return HttpNotFound();
            }
            return View(transactionRecord);
        }

        // GET: TransactionRecords/Create
        public ActionResult Create()
        {
            ViewBag.AdminID = new SelectList(db.Admins, "AdminId", "AdminName");
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerId", "Username");
            ViewBag.TourId = new SelectList(db.Tours, "TourId", "TourName");
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "TransactionTypeId", "TransactionTypeName");
            return View();
        }

        // POST: TransactionRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionRecordId,TransactionTypeId,TourId,CustomerID,TransactionFee,Paid,RecorededTime,AdminID,TransactionNote,Deleted")] TransactionRecord transactionRecord)
        {
            if (ModelState.IsValid)
            {
                db.TransactionRecords.Add(transactionRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdminID = new SelectList(db.Admins, "AdminId", "AdminName", transactionRecord.AdminID);
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerId", "Username", transactionRecord.CustomerID);
            ViewBag.TourId = new SelectList(db.Tours, "TourId", "TourName", transactionRecord.TourId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "TransactionTypeId", "TransactionTypeName", transactionRecord.TransactionTypeId);
            return View(transactionRecord);
        }

        // GET: TransactionRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionRecord transactionRecord = db.TransactionRecords.Find(id);
            if (transactionRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdminID = new SelectList(db.Admins, "AdminId", "AdminName", transactionRecord.AdminID);
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerId", "Username", transactionRecord.CustomerID);
            ViewBag.TourId = new SelectList(db.Tours, "TourId", "TourName", transactionRecord.TourId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "TransactionTypeId", "TransactionTypeName", transactionRecord.TransactionTypeId);
            return View(transactionRecord);
        }

        // POST: TransactionRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionRecordId,TransactionTypeId,TourId,CustomerID,TransactionFee,Paid,RecorededTime,AdminID,TransactionNote,Deleted")] TransactionRecord transactionRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactionRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdminID = new SelectList(db.Admins, "AdminId", "AdminName", transactionRecord.AdminID);
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerId", "Username", transactionRecord.CustomerID);
            ViewBag.TourId = new SelectList(db.Tours, "TourId", "TourName", transactionRecord.TourId);
            ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "TransactionTypeId", "TransactionTypeName", transactionRecord.TransactionTypeId);
            return View(transactionRecord);
        }

        // GET: TransactionRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionRecord transactionRecord = db.TransactionRecords.Find(id);
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
            TransactionRecord transactionRecord = db.TransactionRecords.Find(id);
            db.TransactionRecords.Remove(transactionRecord);
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
