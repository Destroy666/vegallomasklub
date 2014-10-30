using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vegallomas.Models;

namespace Vegallomas.Controllers
{
    public class HomeController : Controller
    {
        vegDB db = new vegDB();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(db.Koncert.Where(k => EntityFunctions.AddDays(k.datum, 1) >= DateTime.Now).OrderBy(k => k.datum).Take(2));
        }

    }
}
