using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Util
{
    /// <summary>
    /// Class with string utilities
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// The random instance
        /// </summary>
        private static Random random = new Random();
        /// <summary>
        /// The characters used to make the random string
        /// </summary>
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        /// <summary>
        /// Creates a random string.
        /// </summary>
        /// <param name="length">The length of resulting string.</param>
        /// <returns>A random string</returns>
        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
