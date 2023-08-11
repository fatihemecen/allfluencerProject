using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public static class SessionManager
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes(Statics.ApplicationStrings.SessionSaltText);
        public static string EncryptStringAES(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException("plainText");
            }
            if (string.IsNullOrEmpty(sharedSecret))
            {
                throw new ArgumentNullException("sharedSecret");
            }

            string outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }
        public static string DecryptStringAES(string cipherText, string sharedSecret)
        {
            //int a = 0;
            //if (string.IsNullOrEmpty(cipherText))
            //{
            //    return "-5"; //throw new ArgumentNullException("cipherText");
            //}
            //if (string.IsNullOrEmpty(sharedSecret))
            //{
            //    return "-10"; //throw new ArgumentNullException("sharedSecret");
            //}

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            //a++;
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                //a++;
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);
                //a++;
                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(cipherText);
                //a++;
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    //a++;
                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                return ""; //"hata:" + ex.ToString() + ", text:" + cipherText;
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }
        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }
        public static string CreateUserSessionKey(string username, string password, int userid,
            int orgid, int userrole, string orgcode, string usertoken)
        {
            string usersessionkey = EncryptStringAES(username + Statics.ApplicationStrings.KeySeperator
                + password + Statics.ApplicationStrings.KeySeperator + userid.ToString() + Statics.ApplicationStrings.KeySeperator
                + DateTime.Now.ToString() + Statics.ApplicationStrings.KeySeperator + orgid.ToString()
                + Statics.ApplicationStrings.KeySeperator + userrole.ToString()
                + Statics.ApplicationStrings.KeySeperator + orgcode
                + Statics.ApplicationStrings.KeySeperator + usertoken, Statics.ApplicationStrings.SessionSharedKey);

            return usersessionkey;
        }
        public static int GetOrganizationIDBySessionKey(string sessionKey)
        {
            string[] parts = GetSessionParts(sessionKey);
            if (parts.Length >= 5)
            {
                return Convert.ToInt32(parts[4]);
            }

            return 0;
        }
        public static int GetUserRoleBySessionKey(string sessionKey)
        {
            string[] parts = GetSessionParts(sessionKey);
            if (parts.Length >= 6)
            {
                return Convert.ToInt32(parts[5]);
            }

            return 0;
        }
        public static string[] GetSessionParts(string sessionKey)
        {
            try
            {
                if (sessionKey != null)
                {
                    sessionKey = sessionKey.Replace(" ", "+");
                    string decpritedKey = DecryptStringAES(sessionKey, Statics.ApplicationStrings.SessionSharedKey);
                    string[] parts = decpritedKey.Split(new string[] { Statics.ApplicationStrings.KeySeperator }, StringSplitOptions.None);
                    return parts;
                }
            }
            catch (Exception ex)
            {

            }


            return new string[0];
        }
        public static bool CheckSessionUser(string sessionKey, int userid)
        {
            string[] parts = GetSessionParts(sessionKey);

            if (parts.Length >= 4)
            {
                int usid = Convert.ToInt32(parts[2]);
                if (usid == userid)
                {
                    return true;
                }
            }

            return false;
        }
        public static bool CheckSessionUserRole(string sessionKey, int userrole)
        {
            string[] parts = GetSessionParts(sessionKey);

            if (parts.Length >= 6)
            {
                int usroleid = Convert.ToInt32(parts[5]);
                if (usroleid == userrole)
                {
                    return true;
                }
            }

            return false;
        }
        public static bool CompareKeys(List<string> vals, string key)
        {
            string val = String.Empty;
            foreach (string param in vals)
            {
                val += param;
            }

            val = CreateKey(val);

            if (val == key)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        public static string CreateKey(string keytext)
        {
            string keyvalue = Utility.MD5Sifrele(keytext + "-" + DateTime.UtcNow.Year.ToString() + "-" + DateTime.UtcNow.Month.ToString().PadLeft(2, '0')
                + "-" + DateTime.UtcNow.Day.ToString().PadLeft(2, '0') + "-" + Statics.SystemValues.Key);
            return keyvalue;
        }
    }
}
