using Microsoft.Web.WebPages.OAuth;
using Microsoft.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Razor;
using System.Web.WebPages.Deployment;
using System.Web.WebPages.Razor;
using Vegallomas.Models;
using WebMatrix.WebData;
using System.Data.Objects;

namespace Vegallomas.Controllers
{
    public class AdminController : Controller
    {
        vegDB db = new vegDB();
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Regisztracio()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Regisztracio(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    return RedirectToAction("Index", "Admin");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View(model);
        }

        public ActionResult Kijelentkezes()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Jelszo_csere(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "A jelszavad sikeresen meg lett változtatva."
                : message == ManageMessageId.SetPasswordSuccess ? "A jelszavad be lett állítva."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Jelszo_csere");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Jelszo_csere(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Jelszo_csere");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Jelszo_csere", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Hibásan adtad meg a jelenlegi, vagy az új jelszavad.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Jelszo_csere", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Koncert()
        {
            return View(db.Koncert.Where(k => EntityFunctions.AddDays( k.datum, 1 ) >= DateTime.Now).OrderBy(k => k.datum));
        }

        public ActionResult Koncert_modositas(int id = 0)
        {
            program p = db.Koncert.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }

        [HttpPost]
        public ActionResult Koncert_modositas(program model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Koncert");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Ujkoncert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ujkoncert(program model)
        {
            if (ModelState.IsValid)
            {
                db.Koncert.Add(model);
                db.SaveChanges();
                return RedirectToAction("Koncert");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Vendegkonyv()
        {
            return View(db.Vendegkonyv.OrderByDescending(v => v.datum));
        }

        [HttpGet]
        public ActionResult Vendegkonyv_torles(int id)
        {
            return View(db.Vendegkonyv.First(v => v.vendegID == id));
        }

        [HttpPost]
        public ActionResult Vendegkonyv_torles(int id, Vendeg v )
        {
            var torles = db.Vendegkonyv.First(i => i.vendegID == id);
            db.Vendegkonyv.Remove(torles);
            db.SaveChanges();

            return RedirectToAction("Vendegkonyv");
        }

        #region Helpers
        private string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Ez a felhasználónév már regisztrálva van. Kérlek válassz egy másikat.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Ezt az e-mail címet már használja egy másik felhasználó. Kérlek adj meg egy másik e-mail címet.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }
        #endregion
    }
}
