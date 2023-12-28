using System;
using System.Text;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string Strip(this string s)
        {
            StringBuilder sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != '\n' && s[i] != '\r' && s[i] != '\t')
                    sb.Append(s[i]);
            }

            return sb.ToString();
        }
    }
}