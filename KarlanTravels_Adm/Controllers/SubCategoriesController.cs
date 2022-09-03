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
    public class SubCategoriesController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();
        // GET: SubCategories
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int? Page, int? PageSize, string SearchStringName, string CurrentSearchName, string SearchStringCategory, string CurrentSearchCategory)
        {
            if (SesCheck.SessionChecking())
            {
                var subCategories = db.SubCategories.Include(s => s.Category);

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
                    subCategories = subCategories.Where(s => s.Deleted == false);
                }
                if (!String.IsNullOrEmpty(SearchStringName))
                {
                    subCategories = subCategories.Where(a => a.SubCategoryName.Contains(SearchStringName));
                }
                if (!String.IsNullOrEmpty(SearchStringCategory))
                {
                    subCategories = subCategories.Where(a => a.Category.CategoryName.Contains(SearchStringCategory));
                }

                switch (SortOpt + SortOdr)
                {
                    case "NameDes":
                        {
                            subCategories = subCategories.OrderByDescending(s => s.SubCategoryName);
                            break;
                        }
                    case "CategoryDes":
                        {
                            subCategories = subCategories.OrderByDescending(s => s.Category.CategoryName);
                            break;
                        }
                    case "NameAsc":
                        {
                            subCategories = subCategories.OrderBy(s => s.SubCategoryName);
                            break;
                        }
                    case "CategoryAsc":
                        {
                            subCategories = subCategories.OrderBy(s => s.Category.CategoryName);
                            break;
                        }
                    default:
                        {
                            subCategories = subCategories.OrderBy(s => s.SubCategoryName);
                            break;
                        }
                }

                return View(subCategories.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: SubCategories/Details/5
        public ActionResult Details(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SubCategory subCategory = db.SubCategories.Find(id);
                if (subCategory == null)
                {
                    return HttpNotFound();
                }
                return View(subCategory);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: SubCategories/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.Deleted == false), "CategoryId", "CategoryName");
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubCategoryId,SubCategoryName,SubCategoryNote,CategoryId")] SubCategory subCategory)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    SubCategory temp = db.SubCategories.Find(subCategory.SubCategoryId);
                    if (temp != null)
                    {
                        TempData["IdWarning"] = $"The id \"{subCategory.SubCategoryId}\" already exists";
                        return RedirectToAction("Create");
                    }
                    db.SubCategories.Add(subCategory);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CategoryId = new SelectList(db.Categories.Where(a => !a.Deleted), "CategoryId", "CategoryName", subCategory.CategoryId);
                return View(subCategory);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: SubCategories/Edit/5
        public ActionResult Edit(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SubCategory subCategory = db.SubCategories.Find(id);
                if (subCategory == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CategoryId = new SelectList(db.Categories.Where(a => !a.Deleted), "CategoryId", "CategoryName", subCategory.CategoryId);
                return View(subCategory);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubCategoryId,SubCategoryName,SubCategoryNote,CategoryId,Deleted")] SubCategory subCategory)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(subCategory).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CategoryId = new SelectList(db.Categories.Where(a => !a.Deleted), "CategoryId", "CategoryName", subCategory.CategoryId);
                return View(subCategory);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: SubCategories/Delete/5
        public ActionResult Delete(string id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SubCategory subCategory = db.SubCategories.Find(id);
                if (subCategory == null)
                {
                    return HttpNotFound();
                }
                return View(subCategory);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (SesCheck.SessionChecking())
            {
                SubCategory subCategory = db.SubCategories.Find(id);
                subCategory.Deleted = true;
                db.Entry(subCategory).State = EntityState.Modified;
                //db.SubCategories.Remove(subCategory);
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
