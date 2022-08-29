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
    public class FacilitiesController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();
        // GET: Facilities
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchStringName, string CurrentSearchName, string SearchStringCity, string CurrentSearchCity, string SearchStringFacilityType, string CurrentSearchFacilityType)
        {
            if (SesCheck.SessionChecking())
            {
                var facilities = db.Facilities.Include(f => f.City).Include(f => f.FacilityType).Where(f => f.FacilityId != "none");
                facilities = facilities.Where(f => f.FacilityId != "none");
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

                if (!String.IsNullOrEmpty(SearchStringCity))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringCity = CurrentSearchCity;
                }

                if (!String.IsNullOrEmpty(SearchStringFacilityType))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringFacilityType = CurrentSearchFacilityType;
                }

                int tempPageSize = (PageSize ?? 5);
                int PageNumber = (Page ?? 1);

                ViewBag.CurrentSearchName = SearchStringName;
                ViewBag.CurrentSearchCity = SearchStringCity;
                ViewBag.CurrentSearchFacilityType = SearchStringFacilityType;
                ViewBag.CurrentSortOpt = String.IsNullOrEmpty(SortOpt) ? "Name" : SortOpt;
                ViewBag.CurrentSortOdr = String.IsNullOrEmpty(SortOdr) ? "Asc" : SortOdr;
                ViewBag.PageSize = tempPageSize;
                ViewBag.CurrentShowDel = ShowDel;
                ViewBag.ShowDelCheck = String.IsNullOrEmpty(ShowDel) ? false : true;


                if (String.IsNullOrEmpty(ShowDel))
                {
                    facilities = facilities.Where(f => f.Deleted == false);
                }

                if (!String.IsNullOrEmpty(SearchStringName))
                {
                    facilities = facilities.Where(f => f.FacilityName.Contains(SearchStringName));
                }
                if (!String.IsNullOrEmpty(SearchStringCity))
                {
                    facilities = facilities.Where(f => f.City.CityName.Contains(SearchStringCity));
                }
                if (!String.IsNullOrEmpty(SearchStringFacilityType))
                {
                    facilities = facilities.Where(f => f.FacilityType.FacilityTypeName.Contains(SearchStringFacilityType));
                }

                switch (SortOpt + SortOdr)
                {
                    case "NameDes":
                        {
                            facilities = facilities.OrderByDescending(f => f.FacilityName);
                            break;
                        }
                    case "TypeDes":
                        {
                            facilities = facilities.OrderByDescending(f => f.FacilityType.FacilityTypeName);
                            break;
                        }
                    case "LocationDes":
                        {
                            facilities = facilities.OrderByDescending(f => f.FacilityLocation);
                            break;
                        }
                    case "CityDes":
                        {
                            facilities = facilities.OrderByDescending(f => f.City.CityName);
                            break;
                        }
                    case "QuantityDes":
                        {
                            facilities = facilities.OrderByDescending(f => f.Quantity);
                            break;
                        }
                    case "NoteDes":
                        {
                            facilities = facilities.OrderByDescending(f => f.ServiceNote);
                            break;
                        }
                    case "AvailabilityDes":
                        {
                            facilities = facilities.OrderByDescending(f => f.FacilityAvailability);
                            break;
                        }
                    case "NameAsc":
                        {
                            facilities = facilities.OrderBy(f => f.FacilityName);
                            break;
                        }
                    case "TypeAsc":
                        {
                            facilities = facilities.OrderBy(f => f.FacilityType.FacilityTypeName);
                            break;
                        }
                    case "LocationAsc":
                        {
                            facilities = facilities.OrderBy(f => f.FacilityLocation);
                            break;
                        }
                    case "CityAsc":
                        {
                            facilities = facilities.OrderBy(f => f.City.CityName);
                            break;
                        }
                    case "QuantityAsc":
                        {
                            facilities = facilities.OrderBy(f => f.Quantity);
                            break;
                        }
                    case "NoteAsc":
                        {
                            facilities = facilities.OrderBy(f => f.ServiceNote);
                            break;
                        }
                    case "AvailabilityAsc":
                        {
                            facilities = facilities.OrderBy(f => f.FacilityAvailability);
                            break;
                        }
                    default:
                        {
                            facilities = facilities.OrderBy(f => f.FacilityName);
                            break;
                        }
                }

                return View(facilities.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Facilities/Details/5
        public ActionResult Details(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Facility facility = db.Facilities.Find(id);
                if (facility == null || facility.FacilityId == "none")
                {
                    return HttpNotFound();
                }
                return View(facility);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Facilities/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                ViewBag.CityId = new SelectList(db.Cities.Where(c => c.Deleted == false), "CityId", "CityName");
                ViewBag.FacilityTypeId = new SelectList(db.FacilityTypes.Where(f => f.Deleted == false), "FacilityTypeId", "FacilityTypeName");
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Facilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacilityId,FacilityName,FacilityTypeId,FacilityLocation,FacilitySocials,FacilityPhone,FacilityEmail,CityId,Quantity,FacilityImage,ServiceNote,FacilityAvailability")] Facility facility)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    Facility temp = db.Facilities.Find(facility.FacilityId);
                    if (temp != null)
                    {
                        TempData["IdWarning"] = $"The id \"{facility.FacilityId}\" already exists";
                        return RedirectToAction("Create");
                    }
                    db.Facilities.Add(facility);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", facility.CityId);
                ViewBag.FacilityTypeId = new SelectList(db.FacilityTypes, "FacilityTypeId", "FacilityTypeName", facility.FacilityTypeId);
                return View(facility);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Facilities/Edit/5
        public ActionResult Edit(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Facility facility = db.Facilities.Find(id);
                if (facility == null || facility.FacilityId == "none")
                {
                    return HttpNotFound();
                }
                ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", facility.CityId);
                ViewBag.FacilityTypeId = new SelectList(db.FacilityTypes, "FacilityTypeId", "FacilityTypeName", facility.FacilityTypeId);
                return View(facility);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FacilityId,FacilityName,FacilityTypeId,FacilityLocation,FacilitySocials,FacilityPhone,FacilityEmail,CityId,Quantity,FacilityImage,ServiceNote,FacilityAvailability,Deleted")] Facility facility)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(facility).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CityId = new SelectList(db.Cities, "CityId", "CityName", facility.CityId);
                ViewBag.FacilityTypeId = new SelectList(db.FacilityTypes, "FacilityTypeId", "FacilityTypeName", facility.FacilityTypeId);
                return View(facility);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Facilities/Delete/5
        public ActionResult Delete(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Facility facility = db.Facilities.Find(id);
                if (facility == null || facility.FacilityId == "none")
                {
                    return HttpNotFound();
                }
                return View(facility);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (SesCheck.SessionChecking())
            {
                Facility facility = db.Facilities.Find(id);
                facility.Deleted = true;
                db.Entry(facility).State = EntityState.Modified;
                //db.Facilities.Remove(facility);
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
