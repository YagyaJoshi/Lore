using Loregroup.Core.Interfaces;
using Loregroup.Core.Interfaces.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Loregroup.Core.Utilities
{
    public class Security : ISecurity
    {
        public string ComputeHash(String input, String salt)
        {
            string encode = salt + input;
            ASCIIEncoding ae = new ASCIIEncoding();
            byte[] hashValue, messageBytes = ae.GetBytes(encode);
            SHA1Managed sha1Hash = new SHA1Managed();
            string strHex = "";

            hashValue = sha1Hash.ComputeHash(messageBytes);
            foreach (byte b in hashValue)
            {
                strHex += String.Format("{0:x2}", b);
            }

            return strHex;
        }
        public String GenerateNewPassword()
        {
            String randomStringLower;
            String randomStringUpper;
            String newPassword = String.Empty;
            Int32 index1 = 0;
            Int32 index2 = 0;
            Int32 index3 = 0;

            randomStringLower = Guid.NewGuid().ToString().Replace("-", String.Empty).ToLower();
            randomStringUpper = Guid.NewGuid().ToString().Replace("-", String.Empty).ToUpper();

            while (newPassword.Length < 6)
            {
                for (Int32 index = index1; index < randomStringLower.Length; index++)
                {
                    if (Char.IsDigit(randomStringLower, index))
                    {
                        newPassword = String.Format("{0}{1}", newPassword, randomStringLower.Substring(index, 1));
                        index1 = index + 1;
                        break;
                    }
                }

                for (Int32 index = index2; index < randomStringLower.Length; index++)
                {
                    if (Char.IsLower(randomStringLower, index))
                    {
                        newPassword = String.Format("{0}{1}", newPassword, randomStringLower.Substring(index, 1));
                        index2 = index + 1;
                        break;
                    }
                }

                for (Int32 index = index3; index < randomStringUpper.Length; index++)
                {
                    if (Char.IsUpper(randomStringUpper, index))
                    {
                        newPassword = String.Format("{0}{1}", newPassword, randomStringUpper.Substring(index, 1));
                        index3 = index + 1;
                        break;
                    }
                }
            }

            return newPassword;
        }
    }
}
