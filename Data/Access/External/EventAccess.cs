using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Access.External
{
    public class EventAccess : IEventAccess
    {
        public List<Event> GetAll(int personId, string modalName, string subModalName)
        {
            var personInScene = Internal.SceneInUse.Instance.Scene.PersonsInScene.Find(pis => pis.PersonId == personId);
            var subModalPersonInScene = personInScene.SubModalType_PersonInScenes.Find(smtPis => smtPis.SubModalType.SubModalTypeId == subModalName && smtPis.SubModalType.ModalType.ModalTypeId == modalName);
            var eventRepresent = subModalPersonInScene.RepresentTypes.FindAll(rt => rt.IntervalData == null && rt.EventData != null);
            List<Event> eventList = new List<Event>();
            foreach (var eventElement in eventRepresent)
            {
                eventList.Add(new Event()
                {
                    Value = eventElement.Value,
                    Subtitle = eventElement.Subtitle,
                    Index = eventElement.Index,
                    EventTime = eventElement.EventData.EventTime
                });
            }
            return eventList;
        }

        #region public add methods
        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value)
        {
            InternalAdd(personId, modalName, subModalName, eventTime, value, null, null);
        }
        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, string subtitle)
        {
            InternalAdd(personId, modalName, subModalName, eventTime, value, subtitle, null);
        }
        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, int index)
        {
            Add(personId, modalName, subModalName, eventTime, value, null, index);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, string subtitle)
        {
            InternalAdd(personId, modalName, subModalName, eventTime, null, subtitle, null);
        }
        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, int index)
        {
            InternalAdd(personId, modalName, subModalName, eventTime, null, null, index);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, string subtitle, int index)
        {
            InternalAdd(personId, modalName, subModalName, eventTime, null, subtitle, index);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, string subtitle, int index)
        {
            InternalAdd(personId, modalName, subModalName, eventTime, value, subtitle, index);
        }
        #endregion

        private void InternalAdd(int personId, string modalName, string subModalName, TimeSpan eventTime, double? value, string subtitle, int? index)
        {
            var personInScene = Internal.SceneInUse.Instance.Scene.PersonsInScene.Find(pis => pis.PersonId == personId);
            var subModalPersonInScene = personInScene.SubModalType_PersonInScenes.Find(smtPis => smtPis.SubModalType.SubModalTypeId == subModalName && smtPis.SubModalType.ModalType.ModalTypeId == modalName);
            EventData eventElement = new EventData()
            {
                EventTime = eventTime
            };
            subModalPersonInScene.RepresentTypes.Add(new RepresentType()
            {
                Value = value,
                Subtitle = subtitle,
                Index = index,
                EventData = eventElement
            });
            var submodalModalKey = new Tuple<string, string>(modalName, subModalName);
            if (!TmpSubModalTypes.Instance.TemporalSubmodals.Contains(submodalModalKey))
            {
                TmpSubModalTypes.Instance.TemporalSubmodals.Add(submodalModalKey);
            }
        }
    }
}
