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
            DbAutoAction();
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
                HttpContext.Current.Session.Clear();
            }
        }

        public Customer CustomerRefundCheck(Customer c)
        {
            Customer temp = c;
            if (temp.AmountToPay < 0)
            {
                temp.AmountToRefund += (-1 * temp.AmountToPay);
                temp.AmountToPay = 0;
            }
            return temp;
        }

        public Customer CustomerViolation(Customer c)
        {
            Customer temp = c;
            if (temp.Violations < temp.maxViolations)
            {
                temp.Violations++;
            }
            else if(temp.Violations > temp.maxViolations)
            {
                temp.Violations = temp.maxViolations;
            }
            return temp;
        }

        public void DbAutoAction()
        {
            ContextModel db = new ContextModel();

            List<Tour> tours = db.Tours.AsNoTracking().ToList();
            for(int i = 0; i < tours.Count; i++)
            {
                if(DateTime.Now > tours[i].TourEnd && tours[i].TourAvailability)
                {
                    tours[i].TourAvailability = false;
                    db.Entry(tours[i]).State = EntityState.Modified;
                }
            }
            
            tours = null;

            List<TransactionRecord> transactions = db.TransactionRecords.AsNoTracking().Where(t => t.Deleted == false && t.Canceled == false).ToList();
            for(int i = 0; i < transactions.Count; i++)
            {
                if (DateTime.Now > transactions[i].DueDate && transactions[i].Paid == false)
                {
                    int customerId = transactions[i].CustomerID;
                    Customer customer = db.Customers.AsNoTracking().Where(c => c.CustomerId == customerId).First();
                    customer = CustomerViolation(customer);
                    transactions[i].Canceled = true;
                    transactions[i].TransactionNote += " Failed to pay before the due date and was canceled";
                    db.Entry(customer).State = EntityState.Modified;
                    db.Entry(transactions[i]).State = EntityState.Modified;
                }
            }

            transactions = null;

            db.SaveChanges();
        }

    }
}