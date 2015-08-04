using LayoutTest.DataAccessLayer;
using LayoutTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LayoutTest.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        DALAcc dal = new DALAcc();
        public ActionResult Index(Account model)
        {
            if (Session["USER"] == null) 
               return RedirectToAction("Index", "Login");
            else return View(model);
        }

        [HttpPost]
        public ActionResult ResetPass(Account model,string NewPassword) {
            Cryptography ch = new Cryptography();
            Account acc = dal.FindAcc(model);
            dal.ChangePass(acc, ch.md5(NewPassword));

        return RedirectToAction("Index","Login",acc);
        }

    }
}
