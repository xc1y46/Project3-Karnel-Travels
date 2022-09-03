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
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchStringCustomer, string SearchStringTour, string CurrentSearchCustomer, string CurrentSearchTour)
        {
            if (SesCheck.SessionChecking())
            {
                var transactionRecords = db.TransactionRecords.Include(t => t.Customer).Include(t => t.Tour);

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


                int tempPageSize = (PageSize ?? 5);
                int PageNumber = (Page ?? 1);

                ViewBag.CurrentSearchCustomer = SearchStringCustomer;
                ViewBag.CurrentSearchTour = SearchStringTour;
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

                switch (SortOpt + SortOdr)
                {
                    case "CustomerDes":
                        {
                            transactionRecords = transactionRecords.OrderByDescending(t => t.Customer.Username);
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
                    case "CancelDes":
                        {
                            transactionRecords = transactionRecords.OrderByDescending(t => t.Canceled);
                            break;
                        }
                    case "CustomerAsc":
                        {
                            transactionRecords = transactionRecords.OrderBy(t => t.Customer.Username);
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
                    case "CancelAsc":
                        {
                            transactionRecords = transactionRecords.OrderBy(t => t.Canceled);
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
                ViewBag.CustomerID = new SelectList(db.Customers.Where(c => !c.Deleted), "CustomerId", "Username");
                ViewBag.TourId = new SelectList(db.Tours.Where(t => t.TourAvailability && !t.Deleted), "TourId", "TourName");
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
        public ActionResult Create([Bind(Include = "TransactionRecordId,TourId,CustomerID,Quantity,TransactionNote")] TransactionRecord transactionRecord)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    Customer customer = db.Customers.AsNoTracking().Where(c => c.CustomerId == transactionRecord.CustomerID && !c.Deleted ).First();

                    if (customer.Violations == customer.maxViolations)
                    {
                        TempData["TourWarning"] = $"The {customer.Username} account has reach max violations ({customer.maxViolations}) and has been blacklisted";
                        return RedirectToAction("Create");
                    }

                    if(customer.BankAccount.AccountNumber == "0")
                    {
                        TempData["TourWarning"] = $"The {customer.Username} doesn't have a verified bank account";
                        return RedirectToAction("Create");
                    }

                    Tour tours = db.Tours.Where(t => t.TourId == transactionRecord.TourId && !t.Deleted).First();
                    List<TransactionRecord> temp = db.TransactionRecords.AsNoTracking().Where(t => t.TourId == tours.TourId && !t.Deleted && !t.Canceled).ToList();
                    DateTime limitDate = tours.TourStart.Subtract(new TimeSpan(tours.BookTimeLimit, 0, 0, 0));

                    if (DateTime.Now > limitDate)
                    {
                        TempData["TourWarning"] = $"The \"{tours.TourName}\" tour booking period has ended after {limitDate.ToString("HH:mm MM/dd/yyyy")})";
                        return RedirectToAction("Create");
                    }

                    int count = 0;
                    for(int i = 0; i < temp.Count; i++)
                    {
                        count += temp[i].Quantity;
                    }

                    if (count + transactionRecord.Quantity > tours.MaxBooking)
                    {
                        TempData["TourWarning"] = $"The \"{tours.TourName}\" tour current booking status is {count}/{tours.MaxBooking}";
                        return RedirectToAction("Create");
                    }

                    temp = db.TransactionRecords.AsNoTracking().Where(t => t.TourId == tours.TourId && t.CustomerID == transactionRecord.CustomerID && !t.Deleted && !t.Canceled).ToList();

                    if (temp.Count != 0)
                    {
                        TempData["TourWarning"] = $"The user \"{customer.Username}\" has already purchased the \"{tours.TourName}\" tour, modify the record's quantity to add";
                        return RedirectToAction("Create");
                    }

                    BankAccount bankAccount = db.BankAccounts.AsNoTracking().Where(t => t.BankAccountId == customer.BankAccountId && !t.Deleted).First();
                    transactionRecord.TransactionFee = tours.TourPrice * (decimal)0.7 * transactionRecord.Quantity;
                    bankAccount.Balance -= tours.TourPrice * (decimal)0.3 * transactionRecord.Quantity;
                    transactionRecord.RecordedTime = DateTime.Now;
                    transactionRecord.DueDate = limitDate;

                    db.Entry(bankAccount).State = EntityState.Modified;
                    db.TransactionRecords.Add(transactionRecord);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CustomerID = new SelectList(db.Customers.Where(c => !c.Deleted), "CustomerId", "Username", transactionRecord.CustomerID);
                ViewBag.TourId = new SelectList(db.Tours.Where(t => t.TourAvailability && !t.Deleted), "TourId", "TourName", transactionRecord.TourId);
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
                ViewBag.CustomerID = new SelectList(db.Customers.Where(c => !c.Deleted), "CustomerId", "Username", transactionRecord.CustomerID);
                ViewBag.TourId = new SelectList(db.Tours.Where(t => t.TourAvailability && !t.Deleted), "TourId", "TourName", transactionRecord.TourId);
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
        public ActionResult Edit([Bind(Include = "TransactionRecordId,TransactionTypeId,TourId,CustomerID,Quantity,TransactionFee,Paid,RecordedTime,DueDate,TransactionNote,Canceled,Deleted")] TransactionRecord transactionRecord)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    TransactionRecord temp = db.TransactionRecords.AsNoTracking().Where(t => t.TransactionRecordId == transactionRecord.TransactionRecordId && !t.Deleted).First();
                    Tour tour = db.Tours.Where(t => t.TourId == transactionRecord.TourId && !t.Deleted).First();
                    Customer customer = db.Customers.AsNoTracking().Where(c => c.CustomerId == transactionRecord.CustomerID).First();
                    BankAccount bankAccount = db.BankAccounts.AsNoTracking().Where(t => t.BankAccountId == customer.BankAccountId && !t.Deleted).First();

                    if (temp.Quantity != transactionRecord.Quantity)
                    {
                        if (customer.Violations == customer.maxViolations)
                        {
                            TempData["TourWarning"] = $"The {customer.Username} account has reach max violations ({customer.maxViolations}) and has been blacklisted";
                            return RedirectToAction("Edit");
                        }

                        if (temp.Canceled)
                        {
                            TempData["QuantityWarning"] = $"The booking has already been canceled";
                            return RedirectToAction("Edit");
                        }

                        if (temp.Paid)
                        {
                            TempData["QuantityWarning"] = $"Booking transaction has completed and cannot be modify, please cancel and rebook if there's any mistake";
                            return RedirectToAction("Edit");
                        }

                        List<TransactionRecord> tempList = db.TransactionRecords.AsNoTracking().Where(t => t.TourId == transactionRecord.TourId && t.TransactionRecordId != transactionRecord.TransactionRecordId && !t.Deleted && !t.Canceled).ToList();

                        int count = 0;
                        for (int i = 0; i < tempList.Count; i++)
                        {
                            count += tempList[i].Quantity;
                        }

                        if (count + transactionRecord.Quantity > tour.MaxBooking)
                        {
                            TempData["QuantityWarning"] = $"The \"{tour.TourName}\" tour current booking status is {count}/{tour.MaxBooking}";
                            return RedirectToAction("Edit");
                        }

                        transactionRecord.TransactionFee = tour.TourPrice * (decimal)0.7 * transactionRecord.Quantity;
                        bankAccount.Balance += tour.TourPrice * (decimal)0.3 * temp.Quantity;
                        bankAccount.Balance -= tour.TourPrice * (decimal)0.3 * transactionRecord.Quantity;

                        db.Entry(bankAccount).State = EntityState.Modified;
                        db.Entry(transactionRecord).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }


                    if (temp.Paid != transactionRecord.Paid)
                    {
                        if (temp.Canceled)
                        {
                            TempData["PaidWarning"] = $"The booking has already been canceled";
                            return RedirectToAction("Edit");
                        }
                        if (!temp.Paid)
                        {
                            bankAccount.Balance -= transactionRecord.TransactionFee;
                        }
                        else
                        {
                            bankAccount.Balance += transactionRecord.TransactionFee;
                        }

                        db.Entry(bankAccount).State = EntityState.Modified;
                        db.Entry(transactionRecord).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }


                    if (temp.Canceled != transactionRecord.Canceled)
                    {
                        if (temp.Paid)
                        {
                            if(DateTime.Now > transactionRecord.DueDate)
                            {
                                customer = SesCheck.CustomerViolation(customer);
                                bankAccount.Balance += transactionRecord.TransactionFee;
                            }
                            else
                            {
                                bankAccount.Balance += tour.TourPrice * transactionRecord.Quantity;
                            }
                        }
                        else
                        {
                            bankAccount.Balance += tour.TourPrice * (decimal)0.3 * transactionRecord.Quantity;
                        }

                        db.Entry(customer).State = EntityState.Modified;
                        db.Entry(bankAccount).State = EntityState.Modified;
                        db.Entry(transactionRecord).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    if (!temp.Deleted && transactionRecord.Deleted && !transactionRecord.Paid && !transactionRecord.Canceled)
                    {
                        TempData["DelWaring"] = "Transaction has not been paid and currently cannot be deleted";
                        return RedirectToAction("Edit");
                    }

                    db.Entry(transactionRecord).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CustomerID = new SelectList(db.Customers.Where(c => !c.Deleted), "CustomerId", "Username", transactionRecord.CustomerID);
                ViewBag.TourId = new SelectList(db.Tours.Where(t => t.TourAvailability && !t.Deleted), "TourId", "TourName", transactionRecord.TourId);
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
                if (!transactionRecord.Paid && !transactionRecord.Canceled)
                {
                    TempData["DelWaring"] = "Transaction has not been paid and currently cannot be deleted";
                    return RedirectToAction("Delete");
                }
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
