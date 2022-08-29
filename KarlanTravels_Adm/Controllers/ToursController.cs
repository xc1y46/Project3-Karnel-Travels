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
    public class ToursController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();
        // GET: Tours
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchStringName, string CurrentSearchName, string SearchStringCategory, string CurrentSearchCategory)
        {
            if (SesCheck.SessionChecking())
            {
                var tours = db.Tours.Include(t => t.Category).Include(t => t.Category1);
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
                if (!String.IsNullOrEmpty(SearchStringCategory))
                {
                    Page = 1;
                }
                else
                {
                    SearchStringCategory = CurrentSearchCategory;
                }

                int tempPageSize = (PageSize ?? 5);
                int PageNumber = (Page ?? 1);

                ViewBag.CurrentSearchName = SearchStringName;
                ViewBag.CurrentSearchCategory = SearchStringCategory;
                ViewBag.CurrentSortOpt = String.IsNullOrEmpty(SortOpt) ? "Name" : SortOpt;
                ViewBag.CurrentSortOdr = String.IsNullOrEmpty(SortOdr) ? "Asc" : SortOdr;
                ViewBag.PageSize = tempPageSize;
                ViewBag.CurrentShowDel = ShowDel;
                ViewBag.ShowDelCheck = String.IsNullOrEmpty(ShowDel) ? false : true;

                if (String.IsNullOrEmpty(ShowDel))
                {
                    tours = tours.Where(t => t.Deleted == false);
                }
                if (!String.IsNullOrEmpty(SearchStringName))
                {
                    tours = tours.Where(t => t.TourName.Contains(SearchStringName));
                }
                if (!String.IsNullOrEmpty(SearchStringCategory))
                {
                    tours = tours.Where(t => t.Category.CategoryName.Contains(SearchStringCategory) || t.Category1.CategoryName.Contains(SearchStringCategory));
                }

                switch (SortOpt + SortOdr)
                {
                    case "NameDes":
                        {
                            tours = tours.OrderByDescending(t => t.TourName);
                            break;
                        }
                    case "TourStartDes":
                        {
                            tours = tours.OrderByDescending(t => t.TourStart);
                            break;
                        }
                    case "TourEndDes":
                        {
                            tours = tours.OrderByDescending(t => t.TourEnd);
                            break;
                        }
                    case "CategoryDes":
                        {
                            tours = tours.OrderByDescending(t => t.Category.CategoryName);
                            break;
                        }
                    case "PriceDes":
                        {
                            tours = tours.OrderByDescending(t => t.TourPrice);
                            break;
                        }
                    case "MaxBookDes":
                        {
                            tours = tours.OrderByDescending(t => t.MaxBooking);
                            break;
                        }
                    case "BookTimeDes":
                        {
                            tours = tours.OrderByDescending(t => t.BookTimeLimit);
                            break;
                        }
                    case "RatingDes":
                        {
                            tours = tours.OrderByDescending(t => t.TourRating);
                            break;
                        }
                    case "AvailablityDes":
                        {
                            tours = tours.OrderByDescending(t => t.TourAvailability);
                            break;
                        }
                    case "NameAsc":
                        {
                            tours = tours.OrderBy(t => t.TourName);
                            break;
                        }
                    case "TourStartAsc":
                        {
                            tours = tours.OrderBy(t => t.TourStart);
                            break;
                        }
                    case "TourEndAsc":
                        {
                            tours = tours.OrderBy(t => t.TourEnd);
                            break;
                        }
                    case "CategoryAsc":
                        {
                            tours = tours.OrderBy(t => t.Category.CategoryName);
                            break;
                        }
                    case "PriceAsc":
                        {
                            tours = tours.OrderBy(t => t.TourPrice);
                            break;
                        }
                    case "MaxBookAsc":
                        {
                            tours = tours.OrderBy(t => t.MaxBooking);
                            break;
                        }
                    case "BookTimeAsc":
                        {
                            tours = tours.OrderBy(t => t.BookTimeLimit);
                            break;
                        }
                    case "RatingAsc":
                        {
                            tours = tours.OrderBy(t => t.TourRating);
                            break;
                        }
                    case "AvailablityAsc":
                        {
                            tours = tours.OrderBy(t => t.TourAvailability);
                            break;
                        }
                    default:
                        {
                            tours = tours.OrderBy(t => t.TourName);
                            break;
                        }
                }

                return View(tours.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Tours/Details/5
        public ActionResult Details(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tour tour = db.Tours.Find(id);
                if (tour == null)
                {
                    return HttpNotFound();
                }
                return View(tour);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Tours/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                ViewBag.CategoryId1 = new SelectList(db.Categories.Where(c => c.Deleted == false), "CategoryId", "CategoryName");
                ViewBag.CategoryId2 = new SelectList(db.Categories.Where(c => c.Deleted == false), "CategoryId", "CategoryName");
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Tours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TourId,TourName,TourAvailability,TourStart,TourEnd,TourPrice,CategoryId1,CategoryId2,MaxBooking,BookTimeLimit,CancelDueDate,TourRating,TourImage,TourNote")] Tour tour)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    Tour temp = db.Tours.Find(tour.TourId);
                    if (temp != null)
                    {
                        TempData["IdWarning"] = $"The id \"{tour.TourId}\" already exists";
                        return RedirectToAction("Create");
                    }
                    if (tour.TourEnd <= tour.TourStart)
                    {
                        TempData["TourEndWarning"] = "Tour end time must be after start time";
                        return RedirectToAction("Create");
                    }
                    if (tour.CategoryId1 == tour.CategoryId2 && tour.CategoryId1 != "UNCG")
                    {
                        TempData["CategoryWarning"] = "The 2 categories must be different";
                        return RedirectToAction("Create");
                    }
                    
                    db.Tours.Add(tour);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CategoryId1 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId1);
                ViewBag.CategoryId2 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId2);
                return View(tour);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Tours/Edit/5
        public ActionResult Edit(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tour tour = db.Tours.Find(id);
                if (tour == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CategoryId1 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId1);
                ViewBag.CategoryId2 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId2);
                return View(tour);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Tours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TourId,TourName,TourAvailability,TourStart,TourEnd,TourPrice,CategoryId1,CategoryId2,MaxBooking,BookTimeLimit,CancelDueDate,TourRating,TourImage,TourNote,Deleted")] Tour tour)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    if (tour.TourEnd <= tour.TourStart)
                    {
                        TempData["TourEndWarning"] = "Tour end time must be after start time";
                        return RedirectToAction("Edit");
                    }
                    if (tour.CategoryId1 == tour.CategoryId2 && tour.CategoryId1 != "UNCG")
                    {
                        TempData["CategoryWarning"] = "The 2 categories must be different";
                        return RedirectToAction("Edit");
                    }
                    db.Entry(tour).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CategoryId1 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId1);
                ViewBag.CategoryId2 = new SelectList(db.Categories, "CategoryId", "CategoryName", tour.CategoryId2);
                return View(tour);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Tours/Delete/5
        public ActionResult Delete(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Tour tour = db.Tours.Find(id);
                if (tour == null)
                {
                    return HttpNotFound();
                }
                return View(tour);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (SesCheck.SessionChecking())
            {
                Tour tour = db.Tours.Find(id);
                tour.Deleted = true;
                db.Entry(tour).State = EntityState.Modified;
                //db.Tours.Remove(tour);
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
