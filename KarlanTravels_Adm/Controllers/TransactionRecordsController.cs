using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KarlanTravels_Adm.Models;
using PagedList;

namespace KarlanTravels_Adm.Controllers
{
    public class TransactionRecordsController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();

        // GET: TransactionRecords
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchStringCustomer, string SearchStringTour, string CurrentSearchCustomer, string CurrentSearchTour, string SearchStringTransactionType, string CurrentSearchTransactionType)
        {
            if (SesCheck.SessionChecking())
            {
                var transactionRecords = db.TransactionRecords.Include(t => t.Customer).Include(t => t.Tour).Include(t => t.TransactionType);

                if (String.IsNullOrEmpty(ShowDel))
                {
                    ShowDel = CurrentShowDel;
                }

                if (!String.IsNullOrEmpty(SearchStringCustomer))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringCustomer = CurrentSearchCustomer;
                }

                if (!String.IsNullOrEmpty(SearchStringTour))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringTour = CurrentSearchTour;
                }

                if (!String.IsNullOrEmpty(SearchStringTransactionType))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringTransactionType = CurrentSearchTransactionType;
                }

                int tempPageSize = (PageSize ?? 5);
                int PageNumber = (Page ?? 1);

                ViewBag.CurrentSearchCustomer = SearchStringCustomer;
                ViewBag.CurrentSearchTour = SearchStringTour;
                ViewBag.CurrentSearchTransactionType = SearchStringTransactionType;
                ViewBag.CurrentSortOpt = String.IsNullOrEmpty(SortOpt) ? "RecordedTime" : SortOpt;
                ViewBag.CurrentSortOdr = String.IsNullOrEmpty(SortOdr) ? "Des" : SortOdr;
                ViewBag.PageSize = tempPageSize;
                ViewBag.CurrentShowDel = ShowDel;
                ViewBag.ShowDelCheck = String.IsNullOrEmpty(ShowDel) ? false : true;


                if (String.IsNullOrEmpty(ShowDel))
                {
                    transactionRecords = transactionRecords.Where(t => t.Deleted == false);
                }

                if (!String.IsNullOrEmpty(SearchStringCustomer))
                {
                    transactionRecords = transactionRecords.Where(t => t.Customer.Username.Contains(SearchStringCustomer));
                }

                if (!String.IsNullOrEmpty(SearchStringTour))
                {
                    transactionRecords = transactionRecords.Where(t => t.Tour.TourName.Contains(SearchStringTour));
                }

                if (!String.IsNullOrEmpty(SearchStringTransactionType))
                {
                    transactionRecords = transactionRecords.Where(t => t.TransactionType.TransactionTypeName.Contains(SearchStringTransactionType));
                }

                switch (SortOpt + SortOdr)
                {
                    case "CustomerDes":
                        {
                            transactionRecords = transactionRecords.OrderByDescending(t => t.Customer.Username);
                            break;
                        }
                    case "TypeDes":
                        {
                            transactionRecords = transactionRecords.OrderByDescending(t => t.TransactionType.TransactionTypeName);
                            break;
                        }
                    case "FeeDes":
                        {
                            transactionRecords = transactionRecords.OrderByDescending(t => t.TransactionFee);
                            break;
                        }
                    case "TourDes":
                        {
                            transactionRecords = transactionRecords.OrderByDescending(t => t.Tour.TourName);
                            break;
                        }
                    case "RecordedTimeDes":
                        {
                            transactionRecords = transactionRecords.OrderByDescending(t => t.RecordedTime);
                            break;
                        }
                    case "PaidDes":
                        {
                            transactionRecords = transactionRecords.OrderByDescending(t => t.Paid);
                            break;
                        }
                    case "CustomerAsc":
                        {
                            transactionRecords = transactionRecords.OrderBy(t => t.Customer.Username);
                            break;
                        }
                    case "TypeAsc":
                        {
                            transactionRecords = transactionRecords.OrderBy(t => t.TransactionType.TransactionTypeName);
                            break;
                        }
                    case "FeeAsc":
                        {
                            transactionRecords = transactionRecords.OrderBy(t => t.TransactionFee);
                            break;
                        }
                    case "TourAsc":
                        {
                            transactionRecords = transactionRecords.OrderBy(t => t.Tour.TourName);
                            break;
                        }
                    case "RecordedTimeAsc":
                        {
                            transactionRecords = transactionRecords.OrderBy(t => t.RecordedTime);
                            break;
                        }
                    case "PaidAsc":
                        {
                            transactionRecords = transactionRecords.OrderBy(t => t.Paid);
                            break;
                        }
                    default:
                        {
                            transactionRecords = transactionRecords.OrderByDescending(t => t.RecordedTime);
                            break;
                        }
                }

                return View(transactionRecords.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: TransactionRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (SesCheck.SessionChecking())
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
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }

        }

        // GET: TransactionRecords/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                ViewBag.CustomerID = new SelectList(db.Customers.Where(c => c.Deleted == false), "CustomerId", "Username");
                ViewBag.TourId = new SelectList(db.Tours.Where(t => t.TourAvailability == true && t.Deleted == false), "TourId", "TourName");
                ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes.Where(t => t.Deleted == false), "TransactionTypeId", "TransactionTypeName");
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: TransactionRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionRecordId,TransactionTypeId,TourId,CustomerID,Paid,TransactionNote")] TransactionRecord transactionRecord)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    Tour tours = db.Tours.Where(t => t.TourId == transactionRecord.TourId).First();
                    List<TransactionRecord> temp = db.TransactionRecords.AsNoTracking().Where(t => t.TourId == tours.TourId && t.TransactionTypeId == "DEPOSIT").ToList();
                    DateTime limitDate = tours.TourStart.Subtract(new TimeSpan(tours.BookTimeLimit, 0, 0, 0));

                    if (DateTime.Now.Date > limitDate.Date && transactionRecord.TransactionTypeId != "CANCL_EARL" && transactionRecord.TransactionTypeId != "CANCL_LATE")
                    {
                        TempData["TourWarning"] = $"The \"{tours.TourName}\" tour booking period has been closed (before {limitDate.Date})";
                        return RedirectToAction("Create");
                    }

                    if (temp.Count >= tours.MaxBooking && transactionRecord.TransactionTypeId != "CANCL_EARL" && transactionRecord.TransactionTypeId != "CANCL_LATE")
                    {
                        TempData["TourWarning"] = $"The \"{tours.TourName}\" tour has already reach the booking limit ({tours.MaxBooking})";
                        return RedirectToAction("Create");
                    }
                    TransactionType transactionTypes = db.TransactionTypes.Where(t => t.TransactionTypeId == transactionRecord.TransactionTypeId).First();
                    transactionRecord.TransactionFee = tours.TourPrice * (decimal)transactionTypes.TransactionPriceRate;
                    transactionRecord.RecordedTime = DateTime.Now;
                    transactionRecord.AdminId = (int)Session["AdminId"];
                    db.TransactionRecords.Add(transactionRecord);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CustomerID = new SelectList(db.Customers, "CustomerId", "Username", transactionRecord.CustomerID);
                ViewBag.TourId = new SelectList(db.Tours, "TourId", "TourName", transactionRecord.TourId);
                ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "TransactionTypeId", "TransactionTypeName", transactionRecord.TransactionTypeId);
                return View(transactionRecord);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }

        }

        // GET: TransactionRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (SesCheck.SessionChecking())
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
                ViewBag.CustomerID = new SelectList(db.Customers, "CustomerId", "Username", transactionRecord.CustomerID);
                ViewBag.TourId = new SelectList(db.Tours, "TourId", "TourName", transactionRecord.TourId);
                ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "TransactionTypeId", "TransactionTypeName", transactionRecord.TransactionTypeId);
                return View(transactionRecord);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }


        }

        // POST: TransactionRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionRecordId,TransactionTypeId,TourId,CustomerID,TransactionFee,Paid,RecordedTime,TransactionNote,Deleted")] TransactionRecord transactionRecord)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    Tour tours = db.Tours.Where(t => t.TourId == transactionRecord.TourId).First();
                    List<TransactionRecord> temp = db.TransactionRecords.AsNoTracking().Where(t => t.TourId == tours.TourId && t.TransactionTypeId == "DEPOSIT").ToList();
                    DateTime limitDate = tours.TourStart.Subtract(new TimeSpan(tours.BookTimeLimit, 0, 0, 0));

                    if (DateTime.Now.Date > limitDate.Date && transactionRecord.TransactionTypeId != "CANCL_EARL" && transactionRecord.TransactionTypeId != "CANCL_LATE")
                    {
                        TempData["TourWarning"] = $"The \"{tours.TourName}\" tour booking period has been closed (before {limitDate.Date})";
                        return RedirectToAction("Edit");
                    }

                    if (temp.Count >= tours.MaxBooking && transactionRecord.TransactionTypeId != "CANCL_EARL" && transactionRecord.TransactionTypeId != "CANCL_LATE")
                    {
                        TempData["TourWarning"] = $"The \"{tours.TourName}\" tour has already reach the booking limit ({tours.MaxBooking})";
                        return RedirectToAction("Edit");
                    }
                    TransactionType transactionTypes =  db.TransactionTypes.AsNoTracking().Where(t => t.TransactionTypeId == transactionRecord.TransactionTypeId).First();
                    transactionRecord.TransactionFee = tours.TourPrice * (decimal)transactionTypes.TransactionPriceRate;
                    transactionRecord.AdminId = (int)Session["AdminId"];
                    db.Entry(transactionRecord).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CustomerID = new SelectList(db.Customers, "CustomerId", "Username", transactionRecord.CustomerID);
                ViewBag.TourId = new SelectList(db.Tours, "TourId", "TourName", transactionRecord.TourId);
                ViewBag.TransactionTypeId = new SelectList(db.TransactionTypes, "TransactionTypeId", "TransactionTypeName", transactionRecord.TransactionTypeId);
                return View(transactionRecord);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }

        }

        // GET: TransactionRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (SesCheck.SessionChecking())
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
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }

        }

        // POST: TransactionRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (SesCheck.SessionChecking())
            {
                TransactionRecord transactionRecord = db.TransactionRecords.Find(id);
                transactionRecord.Deleted = true;
                db.Entry(transactionRecord).State = EntityState.Modified;
                //db.TransactionRecords.Remove(transactionRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
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
