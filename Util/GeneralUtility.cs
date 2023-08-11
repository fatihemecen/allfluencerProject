using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;

namespace Util
{
    public class GeneralUtility
    {
        public static string MD5Sifrele(string input)
        {
            SHA1 sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
        public static string CreateKey(string keytext)
        {
            string keyvalue = MD5Sifrele(keytext + "-" + DateTime.UtcNow.Year.ToString() + "-" + DateTime.UtcNow.Month.ToString().PadLeft(2, '0')
                + "-" + DateTime.UtcNow.Day.ToString().PadLeft(2, '0') + "-" + Statics.SystemValues.Key);
            return keyvalue;
        }
        public static string CreateKey(Dictionary<string, string> parameters)
        {
            List<string> allKeys = parameters.Keys.ToList();
            allKeys = allKeys.OrderBy(x => x).ToList();

            string keytext = "";

            foreach (string item in allKeys)
            {
                string val = parameters[item];
                keytext += val;
            }

            return CreateKey(keytext);
        }
        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public static string RandomString(int length)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
