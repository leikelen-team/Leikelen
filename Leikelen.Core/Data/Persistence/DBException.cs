using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Data.Persistence
{
    /// <summary>
    /// Class for the exception derived from database errors
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class DbException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbException"/> class.
        /// </summary>
        public DbException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbException"/> class with message.
        /// </summary>
        /// <param name="message">Message that describe the error.</param>
        public DbException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbException"/> class.
        /// </summary>
        /// <param name="message">Message that describe the error.</param>
        /// <param name="innerException">The exception that causes this exception or null (<see langword="Nothing" /> in Visual Basic).</param>
        public DbException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
