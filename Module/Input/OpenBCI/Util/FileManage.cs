using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cl.uv.leikelen.Module.Input.OpenBCI.Util
{
    public class FileManage
    {
        private StreamWriter _sw;
        public string FileName { get; private set; }
        private const string Separador = "\t";

        public FileManage(string fileName)
        {
            _sw = new StreamWriter(fileName);
            FileName = fileName;
        }

        public void WriteFile(TimeSpan actualTime, double[] data)
        {
            if (_sw == null) return;
            double multiplier = (4.5 / 24 / (Math.Pow(2, 23) - 1)) * (Math.Pow(10, 6));

            for (int i = 0; i < 8; i++)
            {
                data[i + 1] = data[i + 1] * multiplier;
            }

            _sw.Write(actualTime.Hours+":"+actualTime.Minutes+":"+actualTime.Seconds+ Separador);
            for (int i = 0; i < 11; i++)
            {
                _sw.Write("{0}"+Separador, data[i]);
            }
            _sw.WriteLine("{0}", data[11]);
        }

        public void CloseFile()
        {
            _sw.Close();
            _sw = null;
        }
    }
}
