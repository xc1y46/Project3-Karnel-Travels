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
    public class BankAccountsController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();

        // GET: BankAccounts
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchStringName, string CurrentSearchName, string SearchStringBank, string CurrentSearchBank)
        {
            if (SesCheck.SessionChecking())
            {
                var bankAccounts = db.BankAccounts.Include(b => b.Bank);


                if (String.IsNullOrEmpty(ShowDel))
                {
                    ShowDel = CurrentShowDel;
                }

                if (!String.IsNullOrEmpty(SearchStringName))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringName = CurrentSearchName;
                }

                if (!String.IsNullOrEmpty(SearchStringBank))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringBank = CurrentSearchBank;
                }

                int tempPageSize = (PageSize ?? 5);
                int PageNumber = (Page ?? 1);

                ViewBag.CurrentSearchName = SearchStringName;
                ViewBag.CurrentSearchBank = SearchStringBank;
                ViewBag.CurrentSortOpt = String.IsNullOrEmpty(SortOpt) ? "Name" : SortOpt;
                ViewBag.CurrentSortOdr = String.IsNullOrEmpty(SortOdr) ? "Asc" : SortOdr;
                ViewBag.PageSize = tempPageSize;
                ViewBag.CurrentShowDel = ShowDel;
                ViewBag.ShowDelCheck = String.IsNullOrEmpty(ShowDel) ? false : true;


                if (String.IsNullOrEmpty(ShowDel))
                {
                    bankAccounts = bankAccounts.Where(a => a.Deleted == false);
                }
                if (!String.IsNullOrEmpty(SearchStringName))
                {
                    bankAccounts = bankAccounts.Where(a => a.AccountName.Contains(SearchStringName));
                }

                if (!String.IsNullOrEmpty(CurrentSearchBank))
                {
                    bankAccounts = bankAccounts.Where(a => a.Bank.BankName.Contains(CurrentSearchBank));
                }

                switch (SortOpt + SortOdr)
                {
                    case "BankDes":
                        {
                            bankAccounts = bankAccounts.OrderByDescending(a => a.Bank.BankName);
                            break;
                        }
                    case "NameDes":
                        {
                            bankAccounts = bankAccounts.OrderByDescending(a => a.AccountName);
                            break;
                        }
                    case "BankAsc":
                        {
                            bankAccounts = bankAccounts.OrderBy(a => a.Bank.BankName);
                            break;
                        }
                    case "NameAsc":
                        {
                            bankAccounts = bankAccounts.OrderBy(a => a.AccountName);
                            break;
                        }
                    default:
                        {
                            bankAccounts = bankAccounts.OrderBy(a => a.AccountName);
                            break;
                        }
                }

                return View(bankAccounts.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: BankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BankAccount bankAccount = db.BankAccounts.Find(id);
                if (bankAccount == null)
                {
                    return HttpNotFound();
                }
                return View(bankAccount);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: BankAccounts/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                ViewBag.BankId = new SelectList(db.Banks.Where(b => b.Deleted == false), "BankId", "BankName");
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: BankAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankAccountId,AccountName,AccountNumber,BankId")] BankAccount bankAccount)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.BankAccounts.Add(bankAccount);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.BankId = new SelectList(db.Banks, "BankId", "BankName", bankAccount.BankId);
                return View(bankAccount);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: BankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                BankAccount bankAccount = db.BankAccounts.Find(id);
                if (bankAccount == null)
                {
                    return HttpNotFound();
                }
                ViewBag.BankId = new SelectList(db.Banks, "BankId", "BankName", bankAccount.BankId);
                return View(bankAccount);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }

        // POST: BankAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BankAccountId,AccountName,AccountNumber,BankId,Deleted")] BankAccount bankAccount)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(bankAccount).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.BankId = new SelectList(db.Banks, "BankId", "BankName", bankAccount.BankId);
                return View(bankAccount);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
           
        }

        // GET: BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                BankAccount bankAccount = db.BankAccounts.Find(id);
                if (bankAccount == null)
                {
                    return HttpNotFound();
                }
                return View(bankAccount);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (SesCheck.SessionChecking())
            {
                BankAccount bankAccount = db.BankAccounts.Find(id);
                bankAccount.Deleted = true;
                db.Entry(bankAccount).State = EntityState.Modified;
                //db.BankAccounts.Remove(bankAccount);
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
