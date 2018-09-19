using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Model;

/// <summary>
/// Access classes that are visible to the modules.
/// </summary>
namespace cl.uv.leikelen.Data.Access.External
{
    /// <summary>
    /// Interface to insert and get the events data
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.DataAccess.IEventAccess" />
    public class EventAccess : IEventAccess
    {
        /// <summary>
        /// Gets a list of all events of a given person, modal name and submodal name
        /// </summary>
        /// <param name="person">person object</param>
        /// <param name="modalName">Name of the modal type</param>
        /// <param name="subModalName">Name of a submodal type inside a modal type</param>
        /// <returns>List of events</returns>
        public List<API.DataAccess.Event> GetAll(Data.Model.Person person, string modalName, string subModalName)
        {
            var personInScene = Internal.SceneInUse.Instance.Scene?.PersonsInScene?.Find(pis => pis.Person.Equals(person));
            var subModalPersonInScene = personInScene?.SubModalType_PersonInScenes?.Find(smtPis => smtPis.SubModalType.SubModalTypeId.Equals(subModalName) && smtPis.SubModalType.ModalType.ModalTypeId.Equals(modalName));
            var eventRepresent = subModalPersonInScene?.RepresentTypes?.FindAll(rt => ReferenceEquals(null, rt.IntervalData) && !rt.Index.HasValue && !ReferenceEquals(null,  rt.EventData));
            if(eventRepresent is null || eventRepresent.Count == 0)
            {
                return null;
            }
            else
            {
                List<Event> eventList = new List<Event>();
                foreach (var eventElement in eventRepresent)
                {
                    eventList.Add(new Event()
                    {
                        Value = eventElement.Value,
                        Subtitle = eventElement.Subtitle,
                        Index = eventElement.Index,
                        EventTime = eventElement.EventData.EventTime,
                        toInterval = eventElement.EventData.ToInterval
                    });
                }
                return eventList.OrderBy(ed => ed.EventTime).ToList();
            }
        }

        #region public add methods
        /// <summary>
        /// Adds an event to a given person, modal and submodal type
        /// </summary>
        /// <param name="person">person object</param>
        /// <param name="modalName">Name of the modal type</param>
        /// <param name="subModalName">Name of a submodal type inside a modal type</param>
        /// <param name="eventTime">time in what event occurred</param>
        /// <param name="value">value number</param>
        /// <param name="toInterval">interval type to add</param>
        public void Add(Data.Model.Person person, string modalName, string subModalName, TimeSpan eventTime, double value, int toInterval)
        {
            InternalAdd(person, modalName, subModalName, eventTime, value, null, toInterval);
        }

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
        public void Add(Data.Model.Person person, string modalName, string subModalName, TimeSpan eventTime, double value, string subtitle, int toInterval)
        {
            InternalAdd(person, modalName, subModalName, eventTime, value, subtitle, toInterval);
        }

        /// <summary>
        /// Adds an event to a given person, modal and submodal type
        /// </summary>
        /// <param name="person">person object</param>
        /// <param name="modalName">Name of the modal type</param>
        /// <param name="subModalName">Name of a submodal type inside a modal type</param>
        /// <param name="eventTime">time in what event occurred</param>
        /// <param name="subtitle">text content in the event</param>
        /// <param name="toInterval">interval type to add</param>
        public void Add(Data.Model.Person person, string modalName, string subModalName, TimeSpan eventTime, string subtitle, int toInterval)
        {
            InternalAdd(person, modalName, subModalName, eventTime, null, subtitle, toInterval);
        }

        /// <summary>
        /// Adds an event to a given person, modal and submodal type
        /// </summary>
        /// <param name="person">person object</param>
        /// <param name="modalName">Name of the modal type</param>
        /// <param name="subModalName">Name of a submodal type inside a modal type</param>
        /// <param name="eventTime">time in what event occurred</param>
        /// <param name="toInterval">interval type to add</param>
        public void Add(Data.Model.Person person, string modalName, string subModalName, TimeSpan eventTime, int toInterval)
        {
            InternalAdd(person, modalName, subModalName, eventTime, null, null, toInterval);
        }
        #endregion

        private void InternalAdd(Data.Model.Person person, string modalName, string subModalName, TimeSpan eventTime, double? value, string subtitle, int toInterval)
        {
            try
            {
                var subModalPersonInScene = TypeValidation.GetSmtPis(person, modalName, subModalName);

                //create event and add to smtPis
                EventData eventElement = new EventData()
                {
                    EventTime = eventTime,
                    ToInterval = toInterval
                };
                subModalPersonInScene.RepresentTypes.Add(new RepresentType()
                {
                    Value = value,
                    Subtitle = subtitle,
                    Index = null,
                    EventData = eventElement,
                    SubModalType_PersonInScene = subModalPersonInScene
                });
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
