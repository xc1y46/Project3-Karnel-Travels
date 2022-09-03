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
    public class TouristSpotsController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();
        // GET: TouristSpots
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchStringName, string CurrentSearchName, string SearchStringCity, string CurrentSearchCity, string SearchStringSubCategory, string CurrentSearchSubCategory)
        {
            if (SesCheck.SessionChecking())
            {
                var touristSpots = db.TouristSpots.Include(t => t.City).Include(t => t.SubCategory).Where(t => t.TouristSpotId != "none");
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

                if (!String.IsNullOrEmpty(SearchStringSubCategory))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringSubCategory = CurrentSearchSubCategory;
                }

                int tempPageSize = (PageSize ?? 5);
                int PageNumber = (Page ?? 1);

                ViewBag.CurrentSearchName = SearchStringName;
                ViewBag.CurrentSearchCity = SearchStringCity;
                ViewBag.CurrentSearchSubCategory = SearchStringSubCategory;
                ViewBag.CurrentSortOpt = String.IsNullOrEmpty(SortOpt) ? "Name" : SortOpt;
                ViewBag.CurrentSortOdr = String.IsNullOrEmpty(SortOdr) ? "Asc" : SortOdr;
                ViewBag.PageSize = tempPageSize;
                ViewBag.CurrentShowDel = ShowDel;
                ViewBag.ShowDelCheck = String.IsNullOrEmpty(ShowDel) ? false : true;

                if (String.IsNullOrEmpty(ShowDel))
                {
                    touristSpots = touristSpots.Where(t => t.Deleted == false);
                }
                if (!String.IsNullOrEmpty(SearchStringName))
                {
                    touristSpots = touristSpots.Where(t => t.TouristSpotName.Contains(SearchStringName));
                }
                if (!String.IsNullOrEmpty(SearchStringCity))
                {
                    touristSpots = touristSpots.Where(t => t.City.CityName.Contains(SearchStringCity));
                }
                if (!String.IsNullOrEmpty(SearchStringSubCategory))
                {
                    touristSpots = touristSpots.Where(t => t.SubCategory.SubCategoryName.Contains(SearchStringSubCategory));
                }

                switch (SortOpt + SortOdr)
                {
                    case "NameDes":
                        {
                            touristSpots = touristSpots.OrderByDescending(t => t.TouristSpotName);
                            break;
                        }
                    case "SubCategoryDes":
                        {
                            touristSpots = touristSpots.OrderByDescending(t => t.SubCategory.SubCategoryName);
                            break;
                        }
                    case "CategoryDes":
                        {
                            touristSpots = touristSpots.OrderByDescending(t => t.SubCategory.Category.CategoryName);
                            break;
                        }
                    case "CityDes":
                        {
                            touristSpots = touristSpots.OrderByDescending(t => t.City.CityName);
                            break;
                        }
                    case "RatingDes":
                        {
                            touristSpots = touristSpots.OrderByDescending(t => t.TouristSpotRating);
                            break;
                        }
                    case "OpenHourDes":
                        {
                            touristSpots = touristSpots.OrderByDescending(t => t.OpenHour);
                            break;
                        }
                    case "AvailabilityDes":
                        {
                            touristSpots = touristSpots.OrderByDescending(t => t.TouristSpotAvailability);
                            break;
                        }
                    case "NameAsc":
                        {
                            touristSpots = touristSpots.OrderBy(t => t.TouristSpotName);
                            break;
                        }
                    case "SubCategoryAsc":
                        {
                            touristSpots = touristSpots.OrderBy(t => t.SubCategory.SubCategoryName);
                            break;
                        }
                    case "CategoryAsc":
                        {
                            touristSpots = touristSpots.OrderBy(t => t.SubCategory.Category.CategoryName);
                            break;
                        }
                    case "CityAsc":
                        {
                            touristSpots = touristSpots.OrderBy(t => t.City.CityName);
                            break;
                        }
                    case "RatingAsc":
                        {
                            touristSpots = touristSpots.OrderBy(t => t.TouristSpotRating);
                            break;
                        }
                    case "OpenHourAsc":
                        {
                            touristSpots = touristSpots.OrderBy(t => t.OpenHour);
                            break;
                        }
                    case "AvailabilityAsc":
                        {
                            touristSpots = touristSpots.OrderBy(t => t.TouristSpotAvailability);
                            break;
                        }

                    default:
                        {
                            touristSpots = touristSpots.OrderBy(t => t.TouristSpotName);
                            break;
                        }
                }

                return View(touristSpots.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: TouristSpots/Details/5
        public ActionResult Details(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TouristSpot touristSpot = db.TouristSpots.Find(id);
                if (touristSpot == null || touristSpot.TouristSpotId == "none")
                {
                    return HttpNotFound();
                }
                return View(touristSpot);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: TouristSpots/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                ViewBag.CityId = new SelectList(db.Cities.Where(c => c.Deleted == false), "CityId", "CityName");
                ViewBag.SubCategoryId = new SelectList(db.SubCategories.Where(s => s.Deleted == false), "SubCategoryId", "SubCategoryName");
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: TouristSpots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TouristSpotId,TouristSpotName,CityId,SubCategoryId,TouristSpotLocation,TouristSpotRating,OpenHourVald,ClosingHourVald,TouristSpotAvailability,TouristSpotImage,TouristSpotNote,Cord_Lat,Cord_Long")] TouristSpot touristSpot)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    TouristSpot temp = db.TouristSpots.Find(touristSpot.TouristSpotId);
                    if (temp != null)
                    {
                        TempData["IdWarning"] = $"The id \"{touristSpot.TouristSpotId}\" already exists";
                        return RedirectToAction("Create");
                    }
                    db.TouristSpots.Add(touristSpot);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CityId = new SelectList(db.Cities.Where(a => !a.Deleted), "CityId", "CityName", touristSpot.CityId);
                ViewBag.SubCategoryId = new SelectList(db.SubCategories.Where(a => !a.Deleted), "SubCategoryId", "SubCategoryName", touristSpot.SubCategoryId);
                return View(touristSpot);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: TouristSpots/Edit/5
        public ActionResult Edit(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TouristSpot touristSpot = db.TouristSpots.Find(id);
                if (touristSpot == null || touristSpot.TouristSpotId == "none")
                {
                    return HttpNotFound();
                }
                ViewBag.CityId = new SelectList(db.Cities.Where(a => !a.Deleted), "CityId", "CityName", touristSpot.CityId);
                ViewBag.SubCategoryId = new SelectList(db.SubCategories.Where(a => !a.Deleted), "SubCategoryId", "SubCategoryName", touristSpot.SubCategoryId);
                return View(touristSpot);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: TouristSpots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TouristSpotId,TouristSpotName,CityId,SubCategoryId,TouristSpotLocation,TouristSpotRating,OpenHourVald,ClosingHourVald,TouristSpotAvailability,TouristSpotImage,TouristSpotNote,Cord_Lat,Cord_Long,Deleted")] TouristSpot touristSpot)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(touristSpot).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CityId = new SelectList(db.Cities.Where(a => !a.Deleted), "CityId", "CityName", touristSpot.CityId);
                ViewBag.SubCategoryId = new SelectList(db.SubCategories.Where(a => !a.Deleted), "SubCategoryId", "SubCategoryName", touristSpot.SubCategoryId);
                return View(touristSpot);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
           
        }

        // GET: TouristSpots/Delete/5
        public ActionResult Delete(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TouristSpot touristSpot = db.TouristSpots.Find(id);
                if (touristSpot == null || touristSpot.TouristSpotId == "none")
                {
                    return HttpNotFound();
                }
                return View(touristSpot);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: TouristSpots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (SesCheck.SessionChecking())
            {
                TouristSpot touristSpot = db.TouristSpots.Find(id);
                touristSpot.Deleted = true;
                db.Entry(touristSpot).State = EntityState.Modified;
                //db.TouristSpots.Remove(touristSpot);
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
