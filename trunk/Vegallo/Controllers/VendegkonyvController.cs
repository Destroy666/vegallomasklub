using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vegallomas.Models;
using CaptchaMvc.Attributes;

namespace Vegallomas.Controllers
{
    public class VendegkonyvController : Controller
    {
        vegDB db = new vegDB();
        //
        // GET: /Vendegkonyv/

        public ActionResult Index()
        {
            return View(db.Vendegkonyv.OrderByDescending(v => v.datum));
        }

        [ValidateInput(false)]
        public ActionResult Uj_bejegyzes()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Uj_bejegyzes(Vendeg model)
        {
            if (ModelState.IsValid)
            {
                model.datum = DateTime.Now;
                db.Vendegkonyv.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index", "Vendegkonyv");
            }
            return View(model);
        }

    }
}
