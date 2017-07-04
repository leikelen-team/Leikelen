using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.InputModule.OpenBCI
{
    public class InterpretStream
    {
        double[] ConvertedData = new double[12];
        private int localByteCounter = 0;
        private int localChannelCounter = 0;
        private int PACKET_readstate = 0;
        private byte[] localAdsByteBuffer = { 0, 0, 0 };
        private byte[] localAccelByteBuffer = { 0, 0 };

        public double[] interpretBinaryStream(byte actbyte)
        {
            bool flag_copyRawDataToFullData = false;

            switch (PACKET_readstate)
            {
                case 0:
                    if (actbyte == 0xC0)
                    {          // Search for the beginning of the package
                        PACKET_readstate++;
                    }
                    break;
                case 1:
                    if (actbyte == 0xA0)
                    {          // Search for the beginning of the package
                        PACKET_readstate++;
                    }
                    else
                    {
                        PACKET_readstate = 0;
                    }
                    break;
                case 2:
                    localByteCounter = 0;
                    localChannelCounter = 0;
                    ConvertedData[localChannelCounter] = actbyte;
                    localChannelCounter++;
                    PACKET_readstate++;
                    break;
                case 3:
                    localAdsByteBuffer[localByteCounter] = actbyte;
                    localByteCounter++;
                    if (localByteCounter == 3)
                    {
                        ConvertedData[localChannelCounter] = Convert.Bit24ToInt32(localAdsByteBuffer);
                        localChannelCounter++;
                        if (localChannelCounter == 9)
                        {
                            PACKET_readstate++;
                            localByteCounter = 0;
                        }
                        else
                        {
                            localByteCounter = 0;
                        }
                    }
                    break;
                case 4:
                    localAccelByteBuffer[localByteCounter] = actbyte;
                    localByteCounter++;
                    if (localByteCounter == 2)
                    {
                        ConvertedData[localChannelCounter] = Convert.Bit16ToInt32(localAccelByteBuffer);
                        localChannelCounter++;
                        if (localChannelCounter == 12)
                        {
                            PACKET_readstate++;
                            localByteCounter = 0;
                        }
                        else
                        {
                            localByteCounter = 0;
                        }
                    }
                    break;
                case 5:
                    if (actbyte == 0xC0)
                    {
                        flag_copyRawDataToFullData = true;
                        PACKET_readstate = 1;
                    }
                    else
                    {
                        PACKET_readstate = 0;
                    }

                    break;
                default:
                    PACKET_readstate = 0;
                    break;
            }

            if (flag_copyRawDataToFullData)
            {
                flag_copyRawDataToFullData = false;
                return ConvertedData;
            }
            else
            {
                return null;
            }
        }
    }
}
