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
    public class TourDetailsController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();
        // GET: TourDetails
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchStringName, string CurrentSearchName, string SearchStringTour, string CurrentSearchTour, string SearchStringFacility, string CurrentSearchFacility, string SearchStringTouristSpot, string CurrentSearchTouristSpot)
        {
            if (SesCheck.SessionChecking())
            {
                var tourDetails = db.TourDetails.Include(t => t.Facility).Include(t => t.Tour).Include(t => t.TouristSpot);
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
                if (!String.IsNullOrEmpty(SearchStringTour))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringTour = CurrentSearchTour;
                }
                if (!String.IsNullOrEmpty(SearchStringFacility))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringFacility = CurrentSearchFacility;
                }
                if (!String.IsNullOrEmpty(SearchStringTouristSpot))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringTouristSpot = CurrentSearchTouristSpot;
                }

                int tempPageSize = (PageSize ?? 5);
                int PageNumber = (Page ?? 1);

                
                ViewBag.CurrentSearchName = SearchStringName;
                ViewBag.CurrentSearchTour = SearchStringTour;
                ViewBag.CurrentSearchFacility = SearchStringFacility;
                ViewBag.CurrentSearchTouristSpot = SearchStringTouristSpot;
                ViewBag.CurrentSortOpt = String.IsNullOrEmpty(SortOpt) ? "Name" : SortOpt;
                ViewBag.CurrentSortOdr = String.IsNullOrEmpty(SortOdr) ? "Asc" : SortOdr;
                ViewBag.PageSize = tempPageSize;
                ViewBag.CurrentShowDel = ShowDel;
                ViewBag.ShowDelCheck = String.IsNullOrEmpty(ShowDel) ? false : true;


                if (String.IsNullOrEmpty(ShowDel))
                {
                    tourDetails = tourDetails.Where(t => t.Deleted == false);
                }

                if (!String.IsNullOrEmpty(SearchStringName))
                {
                    tourDetails = tourDetails.Where(t => t.TourDetailName.Contains(SearchStringName));
                }

                if (!String.IsNullOrEmpty(SearchStringTour))
                {
                    tourDetails = tourDetails.Where(t => t.Tour.TourName.Contains(SearchStringTour));
                }

                if (!String.IsNullOrEmpty(SearchStringFacility))
                {
                    tourDetails = tourDetails.Where(t => t.Facility.FacilityName.Contains(SearchStringFacility));
                }

                if (!String.IsNullOrEmpty(SearchStringTouristSpot))
                {
                    tourDetails = tourDetails.Where(t => t.TouristSpot.TouristSpotName.Contains(SearchStringTouristSpot));
                }

                switch (SortOpt + SortOdr)
                {
                    case "NameDes":
                        {
                            tourDetails = tourDetails.OrderByDescending(t => t.TourDetailName);
                            break;
                        }
                    case "TourDes":
                        {
                            tourDetails = tourDetails.OrderByDescending(t => t.Tour.TourName);
                            break;
                        }
                    case "StartDes":
                        {
                            tourDetails = tourDetails.OrderByDescending(t => t.ActivityTimeStart);
                            break;
                        }
                    case "FacilityDes":
                        {
                            tourDetails = tourDetails.OrderByDescending(t => t.Facility.FacilityName);
                            break;
                        }
                    case "TouristSpotDes":
                        {
                            tourDetails = tourDetails.OrderByDescending(t => t.TouristSpot.TouristSpotName);
                            break;
                        }
                    case "NameAsc":
                        {
                            tourDetails = tourDetails.OrderBy(t => t.TourDetailName);
                            break;
                        }
                    case "TourAsc":
                        {
                            tourDetails = tourDetails.OrderBy(t => t.Tour.TourName);
                            break;
                        }
                    case "StartAsc":
                        {
                            tourDetails = tourDetails.OrderBy(t => t.ActivityTimeStart);
                            break;
                        }
                    case "FacilityAsc":
                        {
                            tourDetails = tourDetails.OrderBy(t => t.Facility.FacilityName);
                            break;
                        }
                    case "TouristSpotAsc":
                        {
                            tourDetails = tourDetails.OrderBy(t => t.TouristSpot.TouristSpotName);
                            break;
                        }
                    default:
                        {
                            tourDetails = tourDetails.OrderBy(t => t.Tour.TourName);
                            break;
                        }
                }


                return View(tourDetails.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
           
        }

        // GET: TourDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TourDetail tourDetail = db.TourDetails.Find(id);
                if (tourDetail == null)
                {
                    return HttpNotFound();
                }
                return View(tourDetail);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: TourDetails/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                ViewBag.FacilityId = new SelectList(db.Facilities.Where(f => f.Deleted == false && f.FacilityAvailability == true), "FacilityId", "FacilityName");
                ViewBag.TourId = new SelectList(db.Tours.Where(f => f.Deleted == false && f.TourAvailability == true), "TourId", "TourName");
                ViewBag.TouristSpotId = new SelectList(db.TouristSpots.Where(f => f.Deleted == false && f.TouristSpotAvailability == true), "TouristSpotId", "TouristSpotName");
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: TourDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TourDetailId,TourDetailName,Activity,ActivityTimeStart,ActivityTimeEnd,TouristSpotId,FacilityId,ActivityNote,TourId")] TourDetail tourDetail)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    Tour tour = db.Tours.Find(tourDetail.TourId);
                    TouristSpot touristSpot = db.TouristSpots.Find(tourDetail.TouristSpotId);
                    List<TourDetail> tourDetailTime = db.TourDetails.Where(t => t.TourId == tourDetail.TourId && t.Deleted == false).OrderBy(t => t.ActivityTimeStart).ToList();

                    if ((tourDetail.ActivityTimeStart.TimeOfDay < touristSpot.OpenHourVald || tourDetail.ActivityTimeStart.TimeOfDay > touristSpot.ClosingHourVald || tourDetail.ActivityTimeEnd.TimeOfDay > touristSpot.ClosingHourVald) && (touristSpot.OpenHour != 0 && touristSpot.ClosingHour != 0))
                    {
                        TempData["ActivityEndWarning"] = $"Activity time not suitable for tourist spot's open time ({touristSpot.OpenHourVald} to {touristSpot.ClosingHourVald})";
                        return RedirectToAction("Create");
                    }

                    if (tourDetailTime.Count != 0 && tourDetail.ActivityTimeStart < tourDetailTime[tourDetailTime.Count - 1].ActivityTimeEnd)
                    {
                        TempData["ActivityEndWarning"] = "The tour already has an activity within that time span";
                        return RedirectToAction("Create");
                    }

                    if (tourDetail.ActivityTimeStart < tour.TourStart || tourDetail.ActivityTimeStart > tour.TourEnd || tourDetail.ActivityTimeEnd > tour.TourEnd)
                    {
                        TempData["ActivityEndWarning"] = $"Activity time is outside of the tour schedule ({tour.TourStart} to {tour.TourEnd})";
                        return RedirectToAction("Create");
                    }

                    if (tourDetail.ActivityTimeEnd <= tourDetail.ActivityTimeStart)
                    {
                        TempData["ActivityEndWarning"] = "Activity end time must be after start time";
                        return RedirectToAction("Create");
                    }

                    if ((tourDetail.FacilityId == "none" && tourDetail.TouristSpotId == "none") || (tourDetail.FacilityId != "none" && tourDetail.TouristSpotId != "none"))
                    {
                        TempData["ActivityWarning"] = "Only a facility or tourist spot per activity, the other must be \'none\'";
                        return RedirectToAction("Create");
                    }

                    db.TourDetails.Add(tourDetail);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.FacilityId = new SelectList(db.Facilities, "FacilityId", "FacilityName", tourDetail.FacilityId);
                ViewBag.TourId = new SelectList(db.Tours, "TourId", "TourName", tourDetail.TourId);
                ViewBag.TouristSpotId = new SelectList(db.TouristSpots, "TouristSpotId", "TouristSpotName", tourDetail.TouristSpotId);
                return View(tourDetail);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: TourDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TourDetail tourDetail = db.TourDetails.Find(id);
                if (tourDetail == null)
                {
                    return HttpNotFound();
                }
                ViewBag.FacilityId = new SelectList(db.Facilities, "FacilityId", "FacilityName", tourDetail.FacilityId);
                ViewBag.TourId = new SelectList(db.Tours, "TourId", "TourName", tourDetail.TourId);
                ViewBag.TouristSpotId = new SelectList(db.TouristSpots, "TouristSpotId", "TouristSpotName", tourDetail.TouristSpotId);
                return View(tourDetail);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: TourDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TourDetailId,TourDetailName,Activity,ActivityTimeStart,ActivityTimeEnd,TouristSpotId,FacilityId,ActivityNote,TourId,Deleted")] TourDetail tourDetail)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    Tour tour = db.Tours.Find(tourDetail.TourId);
                    TouristSpot touristSpot = db.TouristSpots.Find(tourDetail.TouristSpotId);
                    List<TourDetail> tourDetailTime = db.TourDetails.Where(t => t.TourId == tourDetail.TourId && t.TourDetailId != tourDetail.TourDetailId && t.Deleted == false).OrderBy(t => t.ActivityTimeStart).ToList();

                    if ((tourDetail.ActivityTimeStart.TimeOfDay < touristSpot.OpenHourVald || tourDetail.ActivityTimeStart.TimeOfDay > touristSpot.ClosingHourVald || tourDetail.ActivityTimeEnd.TimeOfDay > touristSpot.ClosingHourVald) && (touristSpot.OpenHour != 0 && touristSpot.ClosingHour != 0))
                    {
                        TempData["ActivityEndWarning"] = $"Activity time not suitable for tourist spot's open time ({touristSpot.OpenHourVald} to {touristSpot.ClosingHourVald})";
                        return RedirectToAction("Edit");
                    }

                    if (tourDetailTime.Count != 0 && tourDetail.ActivityTimeStart < tourDetailTime[tourDetailTime.Count - 1].ActivityTimeEnd)
                    {
                        TempData["ActivityEndWarning"] = "The tour already has an activity within that time span";
                        return RedirectToAction("Edit");
                    }

                    if (tourDetail.ActivityTimeStart < tour.TourStart || tourDetail.ActivityTimeStart > tour.TourEnd || tourDetail.ActivityTimeEnd > tour.TourEnd)
                    {
                        TempData["ActivityEndWarning"] = $"Activity time is outside of the tour schedule ({tour.TourStart} to {tour.TourEnd})";
                        return RedirectToAction("Edit");
                    }

                    if (tourDetail.ActivityTimeEnd <= tourDetail.ActivityTimeStart)
                    {
                        TempData["ActivityEndWarning"] = "Activity end time must be after start time";
                        return RedirectToAction("Edit");
                    }

                    if ((tourDetail.FacilityId == "none" && tourDetail.TouristSpotId == "none") || (tourDetail.FacilityId != "none" && tourDetail.TouristSpotId != "none"))
                    {
                        TempData["ActivityWarning"] = "Only a facility or tourist spot per activity, the other must be \'none\'";
                        return RedirectToAction("Edit");
                    }

                    db.Entry(tourDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.FacilityId = new SelectList(db.Facilities, "FacilityId", "FacilityName", tourDetail.FacilityId);
                ViewBag.TourId = new SelectList(db.Tours, "TourId", "TourName", tourDetail.TourId);
                ViewBag.TouristSpotId = new SelectList(db.TouristSpots, "TouristSpotId", "TouristSpotName", tourDetail.TouristSpotId);
                return View(tourDetail);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: TourDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TourDetail tourDetail = db.TourDetails.Find(id);
                if (tourDetail == null)
                {
                    return HttpNotFound();
                }
                return View(tourDetail);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: TourDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (SesCheck.SessionChecking())
            {
                TourDetail tourDetail = db.TourDetails.Find(id);
                tourDetail.Deleted = true;
                db.Entry(tourDetail).State = EntityState.Modified;
                //db.TourDetails.Remove(tourDetail);
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
