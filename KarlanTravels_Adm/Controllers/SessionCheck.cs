using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KarlanTravels_Adm.Models;
using System.Security.Cryptography;
using System.Text;

namespace KarlanTravels_Adm.Controllers
{
    public class SessionCheck
    {
        public string HashPW(string pass)
        {
            SHA1Managed sha1 = new SHA1Managed();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(pass));
            var pw = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                pw.Append(b.ToString("x2"));
            }

            return pw.ToString();
        }

        public bool AdminCheck(Admin admin)
        {
            return (
                admin.AdminId == (int)HttpContext.Current.Session["AdminId"] &&
                admin.AdminName == HttpContext.Current.Session["AdminName"].ToString() &&
                admin.RoleId == HttpContext.Current.Session["AdminRoleId"].ToString()
                );
        }

        public bool SessionChecking()
        {
            return (
                HttpContext.Current != null &&
                HttpContext.Current.Session != null &&
                HttpContext.Current.Session["AdminName"] != null &&
                HttpContext.Current.Session["AdminRole"] != null
                );
        }

        public void AutoLogOut()
        {
            
            if (this.SessionChecking())
            {
                ContextModel db = new ContextModel();
                Admin adm = db.Admins.Find((int)HttpContext.Current.Session["AdminId"]);
                adm.IsActive = false;
                db.Entry(adm).State = EntityState.Modified;
                db.SaveChanges();
                HttpContext.Current.Session.Clear();
            }
        }
    }
}