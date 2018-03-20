using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Data.Persistence
{
    public class RepresentTypeException : Exception
    {
        public RepresentTypeException() : base()
        {

        }

        public RepresentTypeException(string message) : base(message)
        {

        }

        public RepresentTypeException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
