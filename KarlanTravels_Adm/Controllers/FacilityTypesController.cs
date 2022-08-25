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
    public class FacilityTypesController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();

        // GET: FacilityTypes
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchString, string CurrentSearch)
        {
            if (SesCheck.SessionChecking())
            {
                var facilityType = from f in db.FacilityTypes select f;
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
                    facilityType = facilityType.Where(f => f.Deleted == false);
                }

                if (!String.IsNullOrEmpty(CurrentSearch))
                {
                    facilityType = facilityType.Where(f => f.FacilityTypeName.Contains(CurrentSearch));
                }

                switch (SortOpt + SortOdr)
                {
                    case "NameDes":
                        {
                            facilityType = facilityType.OrderByDescending(f => f.FacilityTypeName);
                            break;
                        }
                    case "NameAsc":
                        {
                            facilityType = facilityType.OrderBy(f => f.FacilityTypeName);
                            break;
                        }
                    default:
                        {
                            facilityType = facilityType.OrderBy(f => f.FacilityTypeName);
                            break;
                        }
                }

                return View(facilityType.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: FacilityTypes/Details/5
        public ActionResult Details(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FacilityType facilityType = db.FacilityTypes.Find(id);
                if (facilityType == null)
                {
                    return HttpNotFound();
                }
                return View(facilityType);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: FacilityTypes/Create
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

        // POST: FacilityTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FacilityTypeId,FacilityTypeName,FacilityTypeNote")] FacilityType facilityType)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    if(db.FacilityTypes.Where(f => f.FacilityTypeId == facilityType.FacilityTypeId)!= null)
                    {
                        TempData["IdWarning"] = $"The id \"{facilityType.FacilityTypeId}\" already exists";
                        return View(facilityType);
                    }
                    db.FacilityTypes.Add(facilityType);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(facilityType);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: FacilityTypes/Edit/5
        public ActionResult Edit(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FacilityType facilityType = db.FacilityTypes.Find(id);
                if (facilityType == null)
                {
                    return HttpNotFound();
                }
                return View(facilityType);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: FacilityTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FacilityTypeId,FacilityTypeName,FacilityTypeNote,Deleted")] FacilityType facilityType)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(facilityType).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(facilityType);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: FacilityTypes/Delete/5
        public ActionResult Delete(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FacilityType facilityType = db.FacilityTypes.Find(id);
                if (facilityType == null)
                {
                    return HttpNotFound();
                }
                return View(facilityType);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: FacilityTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (SesCheck.SessionChecking())
            {
                FacilityType facilityType = db.FacilityTypes.Find(id);
                facilityType.Deleted = true;
                db.Entry(facilityType).State = EntityState.Modified;
                //db.FacilityTypes.Remove(facilityType);
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
