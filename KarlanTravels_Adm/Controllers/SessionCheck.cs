using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace KarlanTravels_Adm.Controllers
{
    public class SessionCheck
    {
        public bool SessionChecking()
        {
            return (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["AdminName"] != null);
        }
    }
}