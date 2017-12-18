using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Access.External
{
    public class EventAccess : IEventAccess
    {
        

        public List<Event> GetAll(int personId, string modalName, string subModalName)
        {
            var personInScene = Internal.SceneInUse.Instance.Scene?.PersonsInScene?.Find(pis => pis.PersonId == personId);
            var subModalPersonInScene = personInScene?.SubModalType_PersonInScenes?.Find(smtPis => smtPis.SubModalType.SubModalTypeId.Equals(subModalName) && smtPis.SubModalType.ModalType.ModalTypeId.Equals(modalName));
            var eventRepresent = subModalPersonInScene?.RepresentTypes?.FindAll(rt => ReferenceEquals(null, rt.IntervalData) && !rt.Index.HasValue && !ReferenceEquals(null,  rt.EventData));
            if(ReferenceEquals(null, eventRepresent) || eventRepresent.Count == 0)
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
        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, bool toInterval)
        {
            InternalAdd(personId, modalName, subModalName, eventTime, value, null, toInterval);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, string subtitle, bool toInterval)
        {
            InternalAdd(personId, modalName, subModalName, eventTime, value, subtitle, toInterval);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, string subtitle, bool toInterval)
        {
            InternalAdd(personId, modalName, subModalName, eventTime, null, subtitle, toInterval);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, bool toInterval)
        {
            InternalAdd(personId, modalName, subModalName, eventTime, null, null, toInterval);
        }
        #endregion

        private void InternalAdd(int personId, string modalName, string subModalName, TimeSpan eventTime, double? value, string subtitle, bool toInterval)
        {
            var subModalPersonInScene = TypeValidation.GetSmtPis(personId, modalName, subModalName);

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
    }
}
