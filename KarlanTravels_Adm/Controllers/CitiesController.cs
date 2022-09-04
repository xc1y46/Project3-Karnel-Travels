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
    public class CitiesController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();
        // GET: Cities
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchStringName, string CurrentSearchName, string SearchStringCountry, string CurrentSearchCountry)
        {
            if (SesCheck.SessionChecking())
            {
                var cities = db.Cities.Include(c => c.Country).Where(c => c.CityId != "none");

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
                    cities = cities.Where(c => c.Deleted == false);
                }
                if (!String.IsNullOrEmpty(SearchStringName))
                {
                    cities = cities.Where(c => c.CityName.Contains(SearchStringName));
                }
                if (!String.IsNullOrEmpty(SearchStringCountry))
                {
                    cities = cities.Where(c => c.Country.CountryName.Contains(SearchStringCountry));
                }

                switch (SortOpt + SortOdr)
                {
                    case "CountryDes":
                        {
                            cities = cities.OrderByDescending(c => c.Country.CountryName);
                            break;
                        }
                    case "NameDes":
                        {
                            cities = cities.OrderByDescending(c => c.CityName);
                            break;
                        }
                    case "PostalCodeDes":
                        {
                            cities = cities.OrderByDescending(c => c.PostalCode);
                            break;
                        }
                    case "CountryAsc":
                        {
                            cities = cities.OrderBy(c => c.Country.CountryName);
                            break;
                        }
                    case "NameAsc":
                        {
                            cities = cities.OrderBy(c => c.CityName);
                            break;
                        }
                    case "PostalCodeAsc":
                        {
                            cities = cities.OrderBy(c => c.PostalCode);
                            break;
                        }
                    default:
                        {
                            cities = cities.OrderBy(c => c.CityName);
                            break;
                        }
                }

                return View(cities.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Cities/Details/5
        public ActionResult Details(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null || id == "none")
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                City city = db.Cities.Find(id);
                if (city == null)
                {
                    return HttpNotFound();
                }
                return View(city);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Cities/Create
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

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CityId,CityName,CountryId,PostalCode,CityNote")] City city)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    City temp = db.Cities.Find(city.CityId);
                    if (temp != null)
                    {
                        TempData["IdWarning"] = $"The id \"{city.CityId}\" already exists";
                        return RedirectToAction("Create");
                    }
                    db.Cities.Add(city);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CountryId = new SelectList(db.Countries.Where(a => !a.Deleted), "CountryId", "CountryName", city.CountryId);
                return View(city);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Cities/Edit/5
        public ActionResult Edit(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null || id == "none")
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                City city = db.Cities.Find(id);
                if (city == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CountryId = new SelectList(db.Countries.Where(a => !a.Deleted), "CountryId", "CountryName", city.CountryId);
                return View(city);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CityId,CityName,CountryId,PostalCode,CityNote,Deleted")] City city)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(city).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CountryId = new SelectList(db.Countries.Where(a => !a.Deleted), "CountryId", "CountryName", city.CountryId);
                return View(city);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Cities/Delete/5
        public ActionResult Delete(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null || id == "none")
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                City city = db.Cities.Find(id);
                if (city == null)
                {
                    return HttpNotFound();
                }
                return View(city);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (SesCheck.SessionChecking())
            {
                City city = db.Cities.Find(id);
                city.Deleted = true;
                db.Entry(city).State = EntityState.Modified;
                //db.Cities.Remove(city);
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
