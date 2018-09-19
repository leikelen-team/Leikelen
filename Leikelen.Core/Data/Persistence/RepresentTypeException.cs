using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Data.Persistence
{
    /// <summary>
    /// Class for the exception derived from represent type errors (event, interval or timeless)
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class RepresentTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Persistence.RepresentTypeException"/> class.
        /// </summary>
        public RepresentTypeException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Persistence.RepresentTypeException"/> class.
        /// </summary>
        /// <param name="message">Message that describe the error.</param>
        public RepresentTypeException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Data.Persistence.RepresentTypeException"/> class.
        /// </summary>
        /// <param name="message">Message that describe the error.</param>
        /// <param name="innerException">The exception that causes this exception or null (<see langword="Nothing" /> in Visual Basic).</param>
        public RepresentTypeException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
