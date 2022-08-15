using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using KarlanTravels_Adm.Models;
using System.Security.Cryptography;
using System.Data.Entity;
using KarlanTravels_Adm.Controllers;

namespace KarlanTravels.Controllers
{
    public class HomeController : Controller
    {
        private SessionCheck SesCheck = new SessionCheck();
        private ContextModel db = new ContextModel();
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Login()
        {
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult LoginStart(string AdminName, string AdminPassword)
        {

            TempData["LoginResult"] = "";
            var CurrentAdmin = db.Admin.Include(a => a.AdminRole);

            if (String.IsNullOrEmpty(AdminPassword))
            {
                TempData["LoginResult"] = "Account not found";
                return RedirectToAction("Login");
            }
            else
            {
                SHA1Managed sha1 = new SHA1Managed();
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(AdminPassword));
                var pw = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    pw.Append(b.ToString("x2"));
                }

                string tempPw = pw.ToString();

                CurrentAdmin = CurrentAdmin.Where(a => a.AdminName == AdminName)
                                         .Where(a => a.AdminPassword == tempPw);

                if (CurrentAdmin.FirstOrDefault() != null)
                {
                    Session.Timeout = 200;
                    Session["AdminName"] = CurrentAdmin.FirstOrDefault().AdminName;
                    Session["AdminRole"] = CurrentAdmin.FirstOrDefault().AdminRole.RoleName;
                    Session["CurrentAdminId"] = CurrentAdmin.FirstOrDefault().AdminId;

                    return RedirectToAction("Welcome");
                }
                else
                {
                    TempData["LoginResult"] = "Account not found";
                    return RedirectToAction("Login");
                }
            }
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Welcome()
        {
            if (SesCheck.SessionChecking())
            {
                TempData["WelcomeMessage"] = $"Welcome {Session["AdminRole"]} {Session["AdminName"]}";

                Admin adm = db.Admin.Find(Session["CurrentAdminId"]);
                adm.IsActive = true;
                db.Entry(adm).State = EntityState.Modified;
                db.SaveChanges();
                return View("Management");
            }
            else
            {
                TempData["LoginResult"] = "Something went wrong";
                return RedirectToAction("Login");
            }
        }
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Management()
        {
            if (SesCheck.SessionChecking())
            {
                return View();
            }
            else
            {
                TempData["LoginResult"] = "Something went wrong";
                return RedirectToAction("Login");
            }
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult LogOut()
        {
            Admin adm = db.Admin.Find(Session["CurrentAdminId"]);
            adm.IsActive = false;
            db.Entry(adm).State = EntityState.Modified;
            db.SaveChanges();
            Session.Clear();
            return View("Login");
        }
    }
}