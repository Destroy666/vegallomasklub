using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vegallomas.Models;
using WebMatrix.WebData;

namespace Vegallomas.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }
            {
                ModelState.AddModelError("", "Hibás felhasználónév és/vagy jelszó!");
                return View(model);
            }

        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
