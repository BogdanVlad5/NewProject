using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace LayoutTest.Models
{
    public class Cryptography
    {

        public string md5(string plainText)
        {
            MD5 enc = MD5.Create();
            byte[] rescBytes = Encoding.ASCII.GetBytes(plainText);
            byte[] hashBytes = enc.ComputeHash(rescBytes);

            StringBuilder str = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                str.Append(hashBytes[i].ToString("X2"));
                // You may use this alternative command to have the hex string  
                // in lower-case letters instead of upper-case  
                // str.Append(hashBytes[i].ToString("x2"));  
            }
            return str.ToString();
        }

        
        

    }
}