using Newtonsoft.Json.Linq;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Util
{
    public static class Utility
    {
        public static string TrimString(string text)
        {
            text = text.Trim().Trim(new char[] { '\n', '\r' });
            return text;
        }

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
        public static bool checkRegex(string text, string regexText)
        {
            Regex regex = new Regex(regexText);
            Match match = regex.Match(text);
            return match.Success;
        }
        public static bool ConvertIntResultToBool(int res)
        {
            if (res > 0)
            {
                return true;
            }
            return false;
        }
        public static DateTime? ConvertShortDateStringtoDateTime(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return null;
            }

            int year = Convert.ToInt32(input.Substring(0, 4));
            int month = Convert.ToInt32(input.Substring(5, 2).TrimStart(new char[] { '0' }));
            int day = Convert.ToInt32(input.Substring(8, 2).TrimStart(new char[] { '0' }));

            return new DateTime(year, month, day);
        }
        public static byte[] ToByteArray(Stream stream)
        {
            stream.Position = 0;
            byte[] buffer = new byte[stream.Length];
            for (int totalBytesCopied = 0; totalBytesCopied < stream.Length;)
            {
                totalBytesCopied += stream.Read(buffer, totalBytesCopied, Convert.ToInt32(stream.Length) - totalBytesCopied);
            }
            return buffer;
        }
        public static Bitmap CreateThumbnail(Bitmap bmp, int width, int height)
        {
            Bitmap? bmpOut = null;
            try
            {
                ImageFormat loFormat = bmp.RawFormat;

                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                if (bmp.Width > bmp.Height)
                {
                    lnRatio = (decimal)width / bmp.Width;
                    lnNewWidth = width;
                    decimal lnTemp = bmp.Height * lnRatio;
                    lnNewHeight = (int)lnTemp;
                }
                else
                {
                    lnRatio = (decimal)height / bmp.Height;
                    lnNewHeight = height;
                    decimal lnTemp = bmp.Width * lnRatio;
                    lnNewWidth = (int)lnTemp;
                }
                bmpOut = new Bitmap(lnNewWidth, lnNewHeight, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.Clear(Color.Transparent);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, 0, 0, lnNewWidth, lnNewHeight);

                bmp.Dispose();
            }
            catch
            {
                return null;
            }

            return bmpOut;
        }
        public static string DecodeString(string input)
        {
            if (input == null)
            {
                return null;
            }
            byte[] data = Base32.Decode(input.Replace("=", "").ToUpper(new CultureInfo("en-US", false)));
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
        public static string EncodeString(string input)
        {
            if (input == null)
            {
                return "";
            }

            string encodedString = Base32.EncodeAsBase32String(input);
            return encodedString;
        }
        public static String toUpper(string text)
        {
            CultureInfo turkey = new CultureInfo("tr-TR");
            return text.ToUpper(turkey);
        }
        public static string UserRoleText(int UserRole)
        {
            if (UserRole == (int)Statics.Enums.UserRoles.Admin)
            {
                return "Admin";
            }
            else if (UserRole == (int)Statics.Enums.UserRoles.Uzman)
            {
                return "Uzman";
            }
            else if (UserRole == (int)Statics.Enums.UserRoles.Assigner)
            {
                return "Atayıcı";
            }
            else if (UserRole == (int)Statics.Enums.UserRoles.Reporter)
            {
                return "Uzm. Yard.";
            }
            else if (UserRole == (int)Statics.Enums.UserRoles.Operation)
            {
                return "Raportör";
            }
            return "";
        }
        public static string ReportStateText(int State)
        {
            if (State == 0)
            {
                return "Uzm. çalışıyor";
            }
            else if (State == 1)
            {
                return "Uzm. tamamladı";
            }
            else if (State == 2)
            {
                return "Atandı";
            }
            else if (State == 3)
            {
                return "Tamamlandı";
            }
            else if (State == 4)
            {
                return "Oluşturuldu";
            }

            return "";
        }
        public static string CreateDescText(string content, string step)
        {
            return content + " --- [Step: " + step + " , Date: " + DateTime.Now.ToString() + "] ";
        }
        public static String DateTimetoShortString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
        public static int calculatePage(string pagegingValue, int currentPage, int totalpage)
        {
            if (pagegingValue == "-1")
            {
                if (currentPage > 0)
                {
                    currentPage--;
                }
            }
            else if (pagegingValue == "+1")
            {
                if (currentPage < totalpage)
                {
                    currentPage++;
                }
            }
            else if (pagegingValue == "-")
            {
                currentPage = 0;
            }
            else if (pagegingValue == "+")
            {
                currentPage = totalpage;
            }
            else
            {
                currentPage = Convert.ToInt32(pagegingValue);
            }

            return currentPage;
        }
        public static string getFilePrefixByType(string reportcode, int type, int subtype, string extension, bool iskapak)
        {
            string fileName = reportcode + "-";
            string subtypename = "";

            string[] subtypetexts = new string[] { "ic_mekan", "dis_mekan", "kurum", "belediye", "ozcekim" };

            if (subtype <= 5 && subtype > 0)
            {
                subtypename = subtypetexts[subtype - 1];
            }

            if (type == 1)
            {
                fileName += "tasinmaz";
            }
            else if (type == 2)
            {
                fileName += "resmi";
            }
            else if (type == 3)
            {
                fileName += "saha";
            }
            else if (type == 4)
            {
                fileName += "lokasyon";
            }

            if (!string.IsNullOrEmpty(subtypename))
            {
                fileName += "_" + subtypename;
            }

            if (iskapak)
            {
                fileName = "kapak";
            }

            fileName += extension;

            return fileName;
        }
        public static DateTime? ConvertStringtoDateTime(string date)
        {
            try
            {
                if (!String.IsNullOrEmpty(date))
                {
                    string year = date.Substring(0, 4);
                    string month = date.Substring(5, 2);
                    string day = date.Substring(8, 2);

                    string hourst = "00";
                    string minutest = "00";

                    int hour = 0;
                    int minute = 0;

                    if (date.Length > 11)
                    {
                        hourst = date.Substring(11, 2);
                        minutest = date.Substring(14, 2);
                        hour = Convert.ToInt32(hourst);
                        minute = Convert.ToInt32(minutest);
                    }

                    DateTime dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month.TrimStart(new char[] { '0' })),
                        Convert.ToInt32(day.TrimStart(new char[] { '0' })), hour, minute, 0);
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public static string ConvertLowerCaseEnglish(string text)
        {
            return text.ToLower().Replace("ç", "c").Replace("ğ", "g").Replace("ı", "i").Replace("ö", "o").Replace("ş", "s").Replace("ü", "u");
        }
        public static string JTokenToString(JObject jobject, string key)
        {
            if (jobject != null && jobject.ContainsKey(key))
            {
                if (jobject[key] != null)
                {
                    return jobject[key].ToString();
                }
            }

            return "";
        }
        public static bool checkIsNumber(string text)
        {
            int n;
            bool valid = int.TryParse(text, out n);
            return valid;
        }
        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
        public static float ConvertFloat(JToken num)
        {
            if (num != null && !string.IsNullOrEmpty(num.ToString()))
            {
                try
                {
                    var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    culture.NumberFormat.NumberDecimalSeparator = ".";

                    float retVal = float.Parse(num.ToString(), culture);
                    return retVal;
                }
                catch
                {

                }
            }

            return 0;
        }
    }
}
