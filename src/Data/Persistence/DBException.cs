using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.Data.Persistence
{
    class DBException : Exception
    {
        public DBException() : base()
        {

        }

        public DBException(string message) : base(message)
        {

        }

        public DBException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }
}
