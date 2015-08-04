using LayoutTest.DataAccessLayer;
using LayoutTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LayoutTest.Controllers
{
    public class AccountController : Controller
    {
        DALAcc dal = new DALAcc();
        //
        // GET: /Account/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LayoutTest.Models.Account model)
        {
            if (string.IsNullOrEmpty(model.Username))
            {
                ModelState.AddModelError("Name", "Name is required");
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Password is required");
            }

            if (!string.IsNullOrEmpty(model.Email))
            {
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(model.Email))
                {
                    ModelState.AddModelError("Email", "Email is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email is required");
            }
            if (ModelState.IsValid)
            {
                ViewBag.Name = model.Username;
                ViewBag.Email = model.Email;
                ViewBag.Password = model.Password;
                Boolean ok = dal.AddAccount(model);
                ViewBag.d = ok;
            }
        return View(model);
        }
        [HttpGet]
        public ActionResult RecoverPass() {

            return View();
        }

        [HttpPost]
        public ActionResult RecoverPass(LayoutTest.Models.Account model)
        {
           SendKey(model);
           return View(model);
        }
        [HttpGet]
        public ActionResult ManageAcc() 
        {
            if (Session["USER"] != null)
                return View();
            else return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult ManageAcc(Account model)
        {
                return View(model);
        }

        public void SendKey(LayoutTest.Models.Account model) {
            String NPass = GetUniqueKey(8);
            Debug.Write(NPass);
            Cryptography e = new Cryptography();
            dal.ChangePass(model,e.md5(NPass));
            string sender = dal.GetAccountEmail(model);
            SendMail(sender,"Your new password" ,NPass);
        }

        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars ="ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public void SendMail(string mailTo, string subject, string body)
        {

            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;

            SmtpServer.Credentials = new System.Net.NetworkCredential("practicanspyre2015@gmail.com", "Nspyre2015");
            SmtpServer.EnableSsl = true;
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("practicanspyre2015@gmail.com");
            mail.To.Add(mailTo);
            mail.Subject = subject;

            string bodyMessage = "Your new password is: "+body+" and you can change it on the website!";
            mail.Body = bodyMessage;
            SmtpServer.Send(mail);
        }
    }
}
