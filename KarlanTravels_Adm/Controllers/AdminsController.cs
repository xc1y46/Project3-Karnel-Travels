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
    public class AdminsController : Controller
    {
        private ContextModel db = new ContextModel();
        private SessionCheck SesCheck = new SessionCheck();

        // GET: Admins
        public ActionResult Index(string SortOpt, string SortOdr, string ShowDel, string CurrentShowDel, int ? Page, int ? PageSize, string SearchString, string CurrentSearch)
        {
            if (SesCheck.SessionChecking())
            {
                var admins = db.Admins.Include(a => a.AdminRole);

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
                ViewBag.CurrentSortOpt = String.IsNullOrEmpty(SortOpt)? "Name" : SortOpt;
                ViewBag.CurrentSortOdr = String.IsNullOrEmpty(SortOdr)? "Asc" : SortOdr;
                ViewBag.PageSize = tempPageSize;
                ViewBag.CurrentShowDel = ShowDel;
                ViewBag.ShowDelCheck = String.IsNullOrEmpty(ShowDel) ? false : true;


                if (String.IsNullOrEmpty(ShowDel))
                {
                    admins = admins.Where(a => a.Deleted == false);
                }

                if (!String.IsNullOrEmpty(SearchString))
                {
                    admins = admins.Where(a => a.AdminName.Contains(SearchString));
                }

                switch (SortOpt+SortOdr)
                {
                    case "NameDes":
                        {
                            admins = admins.OrderByDescending(a => a.AdminName);
                            break;
                        }
                    case "RoleDes":
                        {
                            admins = admins.OrderByDescending(a => a.AdminRole.RoleName);
                            break;
                        }
                    case "NameAsc":
                        {
                            admins = admins.OrderBy(a => a.AdminName);
                            break;
                        }
                    case "RoleAsc":
                        {
                            admins = admins.OrderBy(a => a.AdminRole.RoleName);
                            break;
                        }
                    default:
                        {
                            admins = admins.OrderBy(a => a.AdminName);
                            break;
                        }
                }

                return View(admins.ToPagedList(PageNumber, tempPageSize));
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            if (SesCheck.SessionChecking())
            {
                ViewBag.RoleId = new SelectList(db.AdminRoles.Where(a => !a.Deleted), "RoleId", "RoleName");
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AdminId,AdminName,AdminPassword,RoleId,AdminNote")] Admin admin)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    Admin temp = db.Admins.Where(a => a.AdminName == admin.AdminName && a.Deleted == false).FirstOrDefault();
                    if (temp != null)
                    {
                        TempData["NameWarning"] = $"The name \"{admin.AdminName}\" already exists";
                        return RedirectToAction("Create");
                    }
                    admin.AdminPassword = SesCheck.HashPW(admin.AdminPassword);
                    db.Admins.Add(admin);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.RoleId = new SelectList(db.AdminRoles.Where(a => !a.Deleted), "RoleId", "RoleName", admin.RoleId);
                return View(admin);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
            
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                ViewBag.RoleId = new SelectList(db.AdminRoles.Where(a => !a.Deleted), "RoleId", "RoleName", admin.RoleId);
                return View(admin);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdminId,AdminName,AdminPassword,RoleId,AdminNote,Deleted")] Admin admin)
        {
            if (SesCheck.SessionChecking())
            {
                if (ModelState.IsValid)
                {
                    Admin temp = db.Admins.Where(a => a.AdminName == admin.AdminName && a.Deleted == false && a.AdminId != admin.AdminId).FirstOrDefault();
                    if (temp != null)
                    {
                        TempData["NameWarning"] = $"The name \"{admin.AdminName}\" already exists";
                        return RedirectToAction("Edit");
                    }
                    admin.AdminPassword = SesCheck.HashPW(admin.AdminPassword);
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.RoleId = new SelectList(db.AdminRoles.Where(a => !a.Deleted), "RoleId", "RoleName", admin.RoleId);
                return View(admin);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (SesCheck.SessionChecking())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admins.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            else
            {
                TempData["LoginResult"] = "Invalid access";
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (SesCheck.SessionChecking())
            {
                Admin admin = db.Admins.Find(id);
                if (SesCheck.AdminCheck(admin))
                {
                    TempData["LoginResult"] = "Please don't try to delete yourself next time";
                    return RedirectToAction("LogOut", "Home");
                }
                else
                {
                    admin.Deleted = true;
                    db.Entry(admin).State = EntityState.Modified;
                    //db.Admins.Remove(admin);
                }
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
