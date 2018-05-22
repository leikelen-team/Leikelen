using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Module.Input.OpenBCI.Util
{
    /// <summary>
    /// Convert byre arrays between different formats.
    /// </summary>
    public static class Convert
    {
        /// <summary>
        /// Transform a byte array in 16 bit to 32bit integer format.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>array in 32 bit integer</returns>
        public static int Bit16ToInt32(byte[] byteArray)
        {
            int result = (
              ((0xFF & byteArray[0]) << 8) |
               (0xFF & byteArray[1])
              );
            if ((result & 0x00008000) > 0)
            {
                result = (int)((uint)result | (uint)0xFFFF0000);

            }
            else
            {
                result = (int)((uint)result & (uint)0x0000FFFF);
            }
            return result;
        }

        /// <summary>
        /// Transform a byte array in 24 bit to 32bit integer format.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>array in 32 bit integer</returns>
        public static int Bit24ToInt32(byte[] byteArray)
        {
            int result = (
                 ((0xFF & byteArray[0]) << 16) |
                 ((0xFF & byteArray[1]) << 8) |
                 (0xFF & byteArray[2])
               );
            if ((result & 0x00800000) > 0)
            {
                result = (int)((uint)result | (uint)0xFF000000);
            }
            else
            {
                result = (int)((uint)result & (uint)0x00FFFFFF);
            }
            return result;
        }
    }
}
