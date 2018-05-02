using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.DataAccess
{
    /// <summary>
    /// Interface to insert and get the events data
    /// </summary>
    public interface IEventAccess
    {
        /// <summary>
        /// Gets a list of all events of a given person, modal name and submodal name
        /// </summary>
        /// <param name="person">person object</param>
        /// <param name="modalName">Name of the modal type</param>
        /// <param name="subModalName">Name of a submodal type inside a modal type</param>
        /// <returns>List of events</returns>
        List<Event> GetAll(Person person, string modalName, string subModalName);

        /// <summary>
        /// Adds an event to a given person, modal and submodal type
        /// </summary>
        /// <param name="person">person object</param>
        /// <param name="modalName">Name of the modal type</param>
        /// <param name="subModalName">Name of a submodal type inside a modal type</param>
        /// <param name="eventTime">time in what event occurred</param>
        /// <param name="value">value number</param>
        /// <param name="toInterval">interval type to add</param>
        void Add(Person person, string modalName, string subModalName, TimeSpan eventTime, double value, int toInterval);

        /// <summary>
        /// Adds an event to a given person, modal and submodal type
        /// </summary>
        /// <param name="person">person object</param>
        /// <param name="modalName">Name of the modal type</param>
        /// <param name="subModalName">Name of a submodal type inside a modal type</param>
        /// <param name="eventTime">time in what event occurred</param>
        /// <param name="value">value number</param>
        /// <param name="subtitle">text content in the event</param>
        /// <param name="toInterval">interval type to add</param>
        void Add(Person person, string modalName, string subModalName, TimeSpan eventTime, double value, string subtitle, int toInterval);

        /// <summary>
        /// Adds an event to a given person, modal and submodal type
        /// </summary>
        /// <param name="person">person object</param>
        /// <param name="modalName">Name of the modal type</param>
        /// <param name="subModalName">Name of a submodal type inside a modal type</param>
        /// <param name="eventTime">time in what event occurred</param>
        /// <param name="subtitle">text content in the event</param>
        /// <param name="toInterval">interval type to add</param>
        void Add(Person person, string modalName, string subModalName, TimeSpan eventTime, string subtitle, int toInterval);

        /// <summary>
        /// Adds an event to a given person, modal and submodal type
        /// </summary>
        /// <param name="person">person object</param>
        /// <param name="modalName">Name of the modal type</param>
        /// <param name="subModalName">Name of a submodal type inside a modal type</param>
        /// <param name="eventTime">time in what event occurred</param>
        /// <param name="toInterval">interval type to add</param>
        void Add(Person person, string modalName, string subModalName, TimeSpan eventTime, int toInterval);
    }
}
