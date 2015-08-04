using LayoutTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace LayoutTest.DataAccessLayer
{
    public class DALAcc
    {
        Cryptography e = new Cryptography();
        AccountContext context = new AccountContext(ConfigurationManager.ConnectionStrings["AccountConnection"].ConnectionString);
        public Boolean AddAccount(Account ac)
        {
            Account temp = context.Accounts.FirstOrDefault(f => f.Username== ac.Username);
            if (temp == null)
            {
                ac.Password = e.md5(ac.Password);
                context.Accounts.Add(ac);
                context.SaveChanges();
                return true;
            }
            else return false;

        }

        public Account FindAcc(Account ac)
        {
            Account p = context.Accounts.FirstOrDefault(f => f.Username == ac.Username);
           // Debug.WriteLine(ac.Password);
            ac.Password = e.md5(ac.Password);
           // Debug.WriteLine(ac.Password);
           // Debug.WriteLine(p.Password);
            if(p!=null)
                if (ac.Password.Equals(p.Password)) return p;
            return null;

        }

        public string GetAccountEmail(Account ac) {
        Account p = context.Accounts.FirstOrDefault(f => f.Username == ac.Username);
        return p.Email;
        }

        public void ChangePass(Account ac, string NewPass) {
            Account original = context.Accounts.Find(ac.Username);
            if (original != null)
            {
                context.Entry(original).Property(u => u.Password).CurrentValue = NewPass; 
                //context.Entry(original).CurrentValues.SetValues(ac.Password = NewPass);
               context.SaveChanges();
           }
        }

        internal Account FindAccByPass(string Password)
        {
            Account p = context.Accounts.FirstOrDefault(f => f.Password == Password);
            if (p!=null) return p;
            else return null;
        }
    }
}