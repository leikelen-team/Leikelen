using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cl.uv.leikelen.src.InputModule.OpenBCI
{
    public class FileManage
    {
        private StreamWriter sw;
        public string fileName;
        private const string SEPARADOR = "\t";

        public FileManage(string fileName)
        {
            sw = new StreamWriter(fileName);
        }

        public void WriteFile(double[] data)
        {
            if (sw == null) return;
            double multiplier = (4.5 / 24 / (Math.Pow(2, 23) - 1)) * (Math.Pow(10, 6));

            for (int i = 0; i < 8; i++)
            {
                data[i + 1] = data[i + 1] * multiplier;
            }

            for(int i = 0; i < 11; i++)
            {
                sw.Write("{0}"+SEPARADOR, data[i]);
            }
            sw.WriteLine("{0}", data[11]);
        }

        public void CloseFile()
        {
            sw.Close();
            sw = null;
        }
    }
}
