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
    public class BanksController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();

        // GET: Banks
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchStringName, string CurrentSearchName, string SearchStringCountry, string CurrentSearchCountry)
        {
            if (SesCheck.SessionChecking())
            {
                var banks = db.Banks.Include(b => b.Country);
                banks = banks.Where(a => a.BankId != "none");

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

                if (!String.IsNullOrEmpty(SearchStringCountry))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringCountry = CurrentSearchCountry;
                }

                int tempPageSize = (PageSize ?? 5);
                int PageNumber = (Page ?? 1);

                ViewBag.CurrentSearchName = SearchStringName;
                ViewBag.CurrentSearchCountry = SearchStringCountry;
                ViewBag.CurrentSortOpt = String.IsNullOrEmpty(SortOpt) ? "Name" : SortOpt;
                ViewBag.CurrentSortOdr = String.IsNullOrEmpty(SortOdr) ? "Asc" : SortOdr;
                ViewBag.PageSize = tempPageSize;
                ViewBag.CurrentShowDel = ShowDel;
                ViewBag.ShowDelCheck = String.IsNullOrEmpty(ShowDel) ? false : true;


                if (String.IsNullOrEmpty(ShowDel))
                {
                    banks = banks.Where(a => a.Deleted == false);
                }
                if (!String.IsNullOrEmpty(SearchStringName))
                {
                    banks = banks.Where(a => a.BankName.Contains(SearchStringName));
                }
                if (!String.IsNullOrEmpty(SearchStringCountry))
                {
                    banks = banks.Where(a => a.Country.CountryName.Contains(SearchStringCountry));
                }

                switch (SortOpt + SortOdr)
                {
                    case "CountryDes":
                        {
                            banks = banks.OrderByDescending(a => a.Country.CountryName);
                            break;
                        }
                    case "NameDes":
                        {
                            banks = banks.OrderByDescending(a => a.BankName);
                            break;
                        }
                    case "SwiftCodeDes":
                        {
                            banks = banks.OrderByDescending(a => a.SwiftCode);
                            break;
                        }
                    case "CountryAsc":
                        {
                            banks = banks.OrderBy(a => a.Country.CountryName);
                            break;
                        }
                    case "NameAsc":
                        {
                            banks = banks.OrderBy(a => a.BankName);
                            break;
                        }
                    case "SwiftCodeAsc":
                        {
                            banks = banks.OrderBy(a => a.SwiftCode);
                            break;
                        }
                    default:
                        {
                            banks = banks.OrderBy(a => a.BankName);
                            break;
                        }
                }

                return View(banks.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Banks/Details/5
        public ActionResult Details(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Bank bank = db.Banks.Find(id);
                if (bank == null)
                {
                    return HttpNotFound();
                }
                return View(bank);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Banks/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                ViewBag.CountryId = new SelectList(db.Countries.Where(c => c.Deleted == false), "CountryId", "CountryName");
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Banks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankId,BankName,SwiftCode,CountryId")] Bank bank)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.Banks.Add(bank);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", bank.CountryId);
                return View(bank);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Banks/Edit/5
        public ActionResult Edit(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Bank bank = db.Banks.Find(id);
                if (bank == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", bank.CountryId);
                return View(bank);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Banks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BankId,BankName,SwiftCode,CountryId,Deleted")] Bank bank)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(bank).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CountryId = new SelectList(db.Countries, "CountryId", "CountryName", bank.CountryId);
                return View(bank);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Banks/Delete/5
        public ActionResult Delete(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Bank bank = db.Banks.Find(id);
                if (bank == null)
                {
                    return HttpNotFound();
                }
                return View(bank);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Banks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (SesCheck.SessionChecking())
            {
                Bank bank = db.Banks.Find(id);
                bank.Deleted = true;
                db.Entry(bank).State = EntityState.Modified;
                //db.Banks.Remove(bank);
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
