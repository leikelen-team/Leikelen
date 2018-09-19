using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    /// <summary>
    /// Interface to get or add intervals to a person
    /// </summary>
    public interface IIntervalAccess
    {
        /// <summary>
        /// Gets all intervals of a person, modal and submodal type.
        /// </summary>
        /// <param name="person">The person object.</param>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="subModalName">Name of the sub modal type.</param>
        /// <returns>List of Intervals</returns>
        List<API.DataAccess.Interval> GetAll(Data.Model.Person person, string modalName, string subModalName);

        /// <summary>
        /// Adds an interval to the specified person.
        /// </summary>
        /// <param name="person">The person object.</param>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="subModalName">Name of the sub modal type.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="value">The value.</param>
        void Add(Data.Model.Person person, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value);

        /// <summary>
        /// Adds an interval to the specified person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="modalName">Name of the modal.</param>
        /// <param name="subModalName">Name of the sub modal.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="value">The value.</param>
        /// <param name="subtitle">The text content in the interval.</param>
        void Add(Data.Model.Person person, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value, string subtitle);

        /// <summary>
        /// Adds an interval to the specified person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="modalName">Name of the modal.</param>
        /// <param name="subModalName">Name of the sub modal.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="subtitle">The text content in the interval.</param>
        void Add(Data.Model.Person person, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, string subtitle);

        /// <summary>
        /// Adds an interval to the specified person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="modalName">Name of the modal.</param>
        /// <param name="subModalName">Name of the sub modal.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        void Add(Data.Model.Person person, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime);

        /// <summary>
        /// Create intervals From a list of events.
        /// </summary>
        /// <param name="person">The person object.</param>
        /// <param name="modalName">Name of the modal type.</param>
        /// <param name="subModalName">Name of the sub modal type.</param>
        /// <param name="millisecondsThreshold">The milliseconds threshold
        /// When an event and the next are separated by this threshold or more, a new interval is created.</param>
        /// <param name="which">The parameter "ToInterval" of the event, which list of events to take in account.</param>
        /// <param name="intervalName">Name of the interval (new submodal type in the given modal type).</param>
        void FromEvent(Data.Model.Person person, string modalName, string subModalName, int millisecondsThreshold, int which, string intervalName);
    }
}
