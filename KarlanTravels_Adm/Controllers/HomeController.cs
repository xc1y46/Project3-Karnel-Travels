using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KarlanTravels_Adm.Models;
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
            var CurrentAdmin = db.Admins.Include(a => a.AdminRole);

            if (String.IsNullOrEmpty(AdminPassword))
            {
                return RedirectToAction("Login");
            }
            else
            {
                string tempPw = SesCheck.HashPW(AdminPassword);
                CurrentAdmin = CurrentAdmin.Where(a => a.AdminName == AdminName && a.AdminPassword == tempPw && a.Deleted == false);

                if (CurrentAdmin.FirstOrDefault() != null)
                {
                    Session.Timeout = 180;
                    Session["AdminId"] = CurrentAdmin.FirstOrDefault().AdminId;
                    Session["AdminName"] = CurrentAdmin.FirstOrDefault().AdminName;
                    Session["AdminRoleId"] = CurrentAdmin.FirstOrDefault().RoleId;
                    Session["AdminRole"] = CurrentAdmin.FirstOrDefault().AdminRole.RoleName;

                    return RedirectToAction("Management");
                }
                else
                {
                    TempData["LoginResult"] = "Account not found";
                    return RedirectToAction("Login");
                }
            }
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Management()
        {
            if (SesCheck.SessionChecking())
            {
                Session["WelcomeMessage"] = $"Welcome {Session["AdminRole"]} {Session["AdminName"]}";
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
            SesCheck.AutoLogOut();
            return RedirectToAction("Login");
        }
    }
}