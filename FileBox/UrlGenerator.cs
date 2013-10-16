using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBox
{
    public class UrlGenerator
    {
        public const string ALLOWED_CHARACTERS = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string CreateShortUrlFromId(long Id)
        {
            long value = Id;
            string output = string.Empty;
            while (value > ALLOWED_CHARACTERS.Length - 1)
            {
                output = ALLOWED_CHARACTERS[(int)(value % ALLOWED_CHARACTERS.Length)] + output;
                value = (long)Math.Floor((double)value / (double)ALLOWED_CHARACTERS.Length);
            }
            return ALLOWED_CHARACTERS[(int)value] + output;
        }
    }
}