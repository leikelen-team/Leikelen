using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API.DataAccess;
using cl.uv.leikelen.src.Data.Access.Internal;
using cl.uv.leikelen.src.Data.Model;

namespace cl.uv.leikelen.src.Data.Access.External
{
    public class EventAccess : IEventAccess
    {
        public List<Event> GetAll(int personId, string modalName, string subModalName)
        {
            var personInScene = SceneInUse.Instance.Scene.PersonInScenes.Find(pis => pis.PersonId == personId);
            var subModal_PersonInScene = personInScene.SubModalType_PersonInScenes.Find(smt_pis => smt_pis.SubModalType.Name == subModalName && smt_pis.SubModalType.ModalType.Name == modalName);
            var eventRepresent = subModal_PersonInScene.RepresentType.FindAll(rt => rt.IntervalData == null && rt.EventData != null);
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
            Add(personId, modalName, subModalName, eventTime, value, null, null);
        }
        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, string subtitle)
        {
            Add(personId, modalName, subModalName, eventTime, value, subtitle, null);
        }
        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, int index)
        {
            Add(personId, modalName, subModalName, eventTime, value, null, index);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, string subtitle)
        {
            Add(personId, modalName, subModalName, eventTime, null, subtitle, null);
        }
        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, int index)
        {
            Add(personId, modalName, subModalName, eventTime, null, null, index);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, string subtitle, int index)
        {
            Add(personId, modalName, subModalName, eventTime, null, subtitle, index);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double value, string subtitle, int index)
        {
            Add(personId, modalName, subModalName, eventTime, value, subtitle, index);
        }
        #endregion

        private void Add(int personId, string modalName, string subModalName, TimeSpan eventTime, double? value, string subtitle, int? index)
        {
            var personInScene = SceneInUse.Instance.Scene.PersonInScenes.Find(pis => pis.PersonId == personId);
            var subModal_PersonInScene = personInScene.SubModalType_PersonInScenes.Find(smt_pis => smt_pis.SubModalType.Name == subModalName && smt_pis.SubModalType.ModalType.Name == modalName);
            EventData eventElement = new EventData()
            {
                EventTime = eventTime
            };
            subModal_PersonInScene.RepresentType.Add(new RepresentType()
            {
                Value = value,
                Subtitle = subtitle,
                Index = index,
                EventData = eventElement
            });
            var submodalModalKey = new Tuple<string, string>(modalName, subModalName);
            if (!TMPSubModalTypes.Instance.TemporalSubmodals.Contains(submodalModalKey))
            {
                TMPSubModalTypes.Instance.TemporalSubmodals.Add(submodalModalKey);
            }
        }
    }
}
