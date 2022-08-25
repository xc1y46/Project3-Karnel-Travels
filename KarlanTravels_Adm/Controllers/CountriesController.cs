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
    public class CountriesController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();
        // GET: Countries
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchString, string CurrentSearch)
        {
            if (SesCheck.SessionChecking())
            {
                var countries = from c in db.Countries select c;

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
                    countries = countries.Where(c => c.Deleted == false);
                }
                if (!String.IsNullOrEmpty(SearchString))
                {
                    countries = countries.Where(c => c.CountryName.Contains(SearchString));
                }

                switch (SortOpt + SortOdr)
                {
                    case "NameDes":
                        {
                            countries = countries.OrderByDescending(c => c.CountryName);
                            break;
                        }
                    case "ContinentDes":
                        {
                            countries = countries.OrderByDescending(c => c.Continent);
                            break;
                        }
                    case "RegionCodeDes":
                        {
                            countries = countries.OrderByDescending(c => c.RegionCode);
                            break;
                        }
                    case "NameAsc":
                        {
                            countries = countries.OrderBy(c => c.CountryName);
                            break;
                        }
                    case "ContinentAsc":
                        {
                            countries = countries.OrderBy(c => c.Continent);
                            break;
                        }
                    case "RegionCodeAsc":
                        {
                            countries = countries.OrderBy(c => c.RegionCode);
                            break;
                        }
                    default:
                        {
                            countries = countries.OrderBy(c => c.CountryName);
                            break;
                        }
                }

                return View(countries.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Countries/Details/5
        public ActionResult Details(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Country country = db.Countries.Find(id);
                if (country == null)
                {
                    return HttpNotFound();
                }
                return View(country);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Countries/Create
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

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryId,CountryName,Continent,RegionCode")] Country country)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    if (db.Countries.Where(f => f.CountryId == country.CountryId) != null)
                    {
                        TempData["IdWarning"] = $"The id \"{country.CountryId}\" already exists";
                        return View(country);
                    }
                    db.Countries.Add(country);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(country);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Countries/Edit/5
        public ActionResult Edit(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Country country = db.Countries.Find(id);
                if (country == null)
                {
                    return HttpNotFound();
                }
                return View(country);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CountryId,CountryName,Continent,RegionCode,Deleted")] Country country)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(country).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(country);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Countries/Delete/5
        public ActionResult Delete(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Country country = db.Countries.Find(id);
                if (country == null)
                {
                    return HttpNotFound();
                }
                return View(country);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (SesCheck.SessionChecking())
            {
                Country country = db.Countries.Find(id);
                country.Deleted = true;
                db.Entry(country).State = EntityState.Modified;
                //db.Countries.Remove(country);
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
