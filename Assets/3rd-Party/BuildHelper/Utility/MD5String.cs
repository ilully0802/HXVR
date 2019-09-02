using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace MyEntryption
{
    public class RealString 
    {
        static public string MySecSum(string input)
        {
            string temMD5 = SecSum(input);
            temMD5 = temMD5.Substring(0, 18);
            return SecSum(temMD5).Substring(0, 28);
        }

        static public string SecSum(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
