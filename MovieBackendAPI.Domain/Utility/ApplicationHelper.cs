using iText.Layout.Element;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MovieBackendAPI.Domain.Utility
{
    public static class ApplicationHelper
    {
        public static string GenerateCode(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                string chars = $"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                ch = chars[random.Next(chars.Length)];
                builder.Append(ch);
            }
            return $"{DateTime.Now.ToString("ddMMyyHHmmss")}{builder.ToString()}".Substring(0, 20);
        }



        public static int get_age(DateTime dob)
        {
            int age = 0;
            age = DateTime.Now.Subtract(dob).Days;
            age = age / 365;
            return age;
        }

        public static string FileStreamToBase64(FileStream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            string base64 = Convert.ToBase64String(bytes);

            return base64;
        }



        /// <summary>
        /// Convert file stream to Base64 string should you want to send as an attachment
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string FileStreamToBase64(MemoryStream stream)
        {
            byte[] bytes = stream.ToArray();


            string base64 = Convert.ToBase64String(bytes);

            return base64;
        }

    }
}