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
    public class TransactionTypesController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();
        // GET: TransactionTypes
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchString, string CurrentSearch)
        {
            if (SesCheck.SessionChecking())
            {
                var transactionTypes = from t in db.TransactionTypes select t;

                if (String.IsNullOrEmpty(ShowDel))
                {
                    ShowDel = CurrentShowDel;
                }
                

                if (!String.IsNullOrEmpty(SearchString))
                {
                    Page = 1;
                }
                else
                {
                    SearchString = CurrentSearch;
                }

                int tempPageSize = (PageSize ?? 5);
                int PageNumber = (Page ?? 1);

                ViewBag.CurrentSearch = SearchString;
                ViewBag.CurrentSortOpt = String.IsNullOrEmpty(SortOpt) ? "Name" : SortOpt;
                ViewBag.CurrentSortOdr = String.IsNullOrEmpty(SortOdr) ? "Asc" : SortOdr;
                ViewBag.PageSize = tempPageSize;
                ViewBag.CurrentShowDel = ShowDel;
                ViewBag.ShowDelCheck = String.IsNullOrEmpty(ShowDel) ? false : true;

                if (String.IsNullOrEmpty(ShowDel))
                {
                    transactionTypes = transactionTypes.Where(t => t.Deleted == false);
                }

                if (!String.IsNullOrEmpty(SearchString))
                {
                    transactionTypes = transactionTypes.Where(a => a.TransactionTypeName.Contains(SearchString));
                }

                switch (SortOpt + SortOdr)
                {
                    case "NameDes":
                        {
                            transactionTypes = transactionTypes.OrderByDescending(t => t.TransactionTypeName);
                            break;
                        }
                    case "NoteDes":
                        {
                            transactionTypes = transactionTypes.OrderByDescending(t => t.TransactionTypeNote);
                            break;
                        }
                    case "NameAsc":
                        {
                            transactionTypes = transactionTypes.OrderBy(t => t.TransactionTypeName);
                            break;
                        }
                    case "NoteAsc":
                        {
                            transactionTypes = transactionTypes.OrderBy(t => t.TransactionTypeNote);
                            break;
                        }
                    default:
                        {
                            transactionTypes = transactionTypes.OrderBy(t => t.TransactionTypeName);
                            break;
                        }
                }

                return View(transactionTypes.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: TransactionTypes/Details/5
        public ActionResult Details(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TransactionType transactionType = db.TransactionTypes.Find(id);
                if (transactionType == null)
                {
                    return HttpNotFound();
                }
                return View(transactionType);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: TransactionTypes/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: TransactionTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionTypeId,TransactionTypeName,TransactionTypeNote,TransactionPriceRate")] TransactionType transactionType)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.TransactionTypes.Add(transactionType);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            

            return View(transactionType);
        }

        // GET: TransactionTypes/Edit/5
        public ActionResult Edit(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TransactionType transactionType = db.TransactionTypes.Find(id);
                if (transactionType == null)
                {
                    return HttpNotFound();
                }
                return View(transactionType);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: TransactionTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionTypeId,TransactionTypeName,TransactionTypeNote,TransactionPriceRate,Deleted")] TransactionType transactionType)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(transactionType).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(transactionType);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: TransactionTypes/Delete/5
        public ActionResult Delete(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TransactionType transactionType = db.TransactionTypes.Find(id);
                if (transactionType == null)
                {
                    return HttpNotFound();
                }
                return View(transactionType);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: TransactionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (SesCheck.SessionChecking())
            {
                TransactionType transactionType = db.TransactionTypes.Find(id);
                transactionType.Deleted = true;
                db.Entry(transactionType).State = EntityState.Modified;
                //db.TransactionTypes.Remove(transactionType);
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
