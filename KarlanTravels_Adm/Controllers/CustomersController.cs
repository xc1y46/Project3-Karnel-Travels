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
    public class CustomersController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();
        // GET: Customers
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchString, string CurrentSearch)
        {
            if (SesCheck.SessionChecking())
            {
                var customers = db.Customers.Include(c => c.BankAccount).Include(c => c.City);

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
                    customers = customers.Where(c => c.Deleted == false);
                }
                if (!String.IsNullOrEmpty(SearchString))
                {
                    customers = customers.Where(c => c.Username.Contains(SearchString));
                }

                switch (SortOpt + SortOdr)
                {
                    case "NameDes":
                        {
                            customers = customers.OrderByDescending(c => c.Username);
                            break;
                        }
                    case "EmailDes":
                        {
                            customers = customers.OrderByDescending(c => c.Email);
                            break;
                        }
                    case "PhoneDes":
                        {
                            customers = customers.OrderByDescending(c => c.Phone);
                            break;
                        }
                    case "CityDes":
                        {
                            customers = customers.OrderByDescending(c => c.City.CityName);
                            break;
                        }
                    case "BankAccDes":
                        {
                            customers = customers.OrderByDescending(c => c.BankAccount.AccountName);
                            break;
                        }
                    case "AmountPayDes":
                        {
                            customers = customers.OrderByDescending(c => c.AmountToPay);
                            break;
                        }
                    case "NameAsc":
                        {
                            customers = customers.OrderBy(c => c.Username);
                            break;
                        }
                    case "EmailAsc":
                        {
                            customers = customers.OrderBy(c => c.Email);
                            break;
                        }
                    case "PhoneAsc":
                        {
                            customers = customers.OrderBy(c => c.Phone);
                            break;
                        }
                    case "CityAsc":
                        {
                            customers = customers.OrderBy(c => c.City.CityName);
                            break;
                        }
                    case "BankAccAsc":
                        {
                            customers = customers.OrderBy(c => c.BankAccount.AccountName);
                            break;
                        }
                    case "AmountPayAsc":
                        {
                            customers = customers.OrderBy(c => c.AmountToPay);
                            break;
                        }
                    default:
                        {
                            customers = customers.OrderBy(c => c.Username);
                            break;
                        }
                }

                return View(customers.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Customer customer = db.Customers.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                return View(customer);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                ViewBag.BankAccountId = new SelectList(db.BankAccounts.Where(b => b.Deleted == false), "BankAccountId", "AccountName");
                ViewBag.CityId = new SelectList(db.Cities.Where(c => c.Deleted == false), "CityId", "CityName");
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerId,Username,Email,Phone,BankAccountId,CityId,UserPassword,AmountToPay,CustomerNote")] Customer customer)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    customer.UserPassword = SesCheck.HashPW(customer.UserPassword);
                    db.Customers.Add(customer);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                ViewBag.BankAccountId = new SelectList(db.BankAccounts, "BankAccountId", "AccountName", customer.BankAccountId);
                ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", customer.CityId);
                return View(customer);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Customer customer = db.Customers.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                ViewBag.BankAccountId = new SelectList(db.BankAccounts, "BankAccountId", "AccountName", customer.BankAccountId);
                ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", customer.CityId);
                return View(customer);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            

        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,Username,Email,Phone,BankAccountId,CityId,UserPassword,AmountToPay,CustomerNote,Deleted")] Customer customer)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    customer.UserPassword = SesCheck.HashPW(customer.UserPassword);
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.BankAccountId = new SelectList(db.BankAccounts, "BankAccountId", "AccountName", customer.BankAccountId);
                ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", customer.CityId);
                return View(customer);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Customer customer = db.Customers.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                return View(customer);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (SesCheck.SessionChecking())
            {
                Customer customer = db.Customers.Find(id);
                customer.Deleted = true;
                db.Entry(customer).State = EntityState.Modified;
                //db.Customers.Remove(customer);
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
