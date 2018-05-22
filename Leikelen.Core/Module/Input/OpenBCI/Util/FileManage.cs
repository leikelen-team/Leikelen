using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cl.uv.leikelen.Module.Input.OpenBCI.Util
{
    /// <summary>
    /// Class to manage the file of the data of 
    /// a given OpenBCI input sensor of a person.
    /// </summary>
    public class FileManage
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }
        private StreamWriter _sw;
        private const string Separador = "\t";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileManage"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public FileManage(string fileName)
        {
            _sw = new StreamWriter(fileName);
            FileName = fileName;
        }

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="actualTime">The actual time.</param>
        /// <param name="data">The data of each channel.</param>
        public void WriteFile(TimeSpan actualTime, double[] data)
        {
            if (ReferenceEquals(null, _sw)) return;
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

        /// <summary>
        /// Closes the file.
        /// </summary>
        public void CloseFile()
        {
            _sw.Close();
            _sw = null;
        }
    }
}
