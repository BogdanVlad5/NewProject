using LayoutTest.DataAccessLayer;
using LayoutTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LayoutTest.Controllers
{
    public class LoginController : Controller
    {
        DALAcc dal = new DALAcc();
        //
        // GET: /Login/

        public ActionResult Index()
        {
            Session["USER"] = null;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LayoutTest.Models.Account model)
        {
            if (string.IsNullOrEmpty(model.Username))
            {
                ModelState.AddModelError("Username", "Username is required");
            }else

                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("Password", "Password is required");
                }
                else
                {
                    Account ok = dal.FindAcc(model);
                    if (ok != null)
                    {
                        ViewBag.acc = ok;
                        if (model.Username.Equals(ok.Username) && model.Password.Equals(ok.Password))
                        {
                            Session["USER"] = model;
                            return RedirectToAction("Index", "Home", ok);
                        }
                    }
                }
                return View();
        }

    }
}
