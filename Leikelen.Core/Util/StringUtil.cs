using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Util
{
    /// <summary>
    /// Utility class with functions related to strings.
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

        /// <summary>
        /// Sets the seed for the random instance.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public static void SetSeed(int seed)
        {
            random = new Random(seed);
        }

        /// <summary>
        /// Return a double number as string with the specified decimals.
        /// </summary>
        /// <param name="value">The value to convert to string.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <returns>A string of the value with the specified decimal places</returns>
        public static string DoubleAsString(double value, int decimalPlaces)
        {
            return Math.Round(value, decimalPlaces).ToString($"F{decimalPlaces}");
        }
    }
}
