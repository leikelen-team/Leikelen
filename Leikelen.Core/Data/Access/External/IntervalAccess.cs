using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Access.External
{
    class IntervalAccess : IIntervalAccess
    {
        public List<Interval> GetAll(int personId, string modalName, string subModalName)
        {
            var personInScene = Internal.SceneInUse.Instance.Scene?.PersonsInScene?.Find(pis => pis.PersonId == personId);
            var subModalPersonInScene = personInScene?.SubModalType_PersonInScenes?.Find(smtPis => smtPis.SubModalType.SubModalTypeId.Equals(subModalName) && smtPis.SubModalType.ModalType.ModalTypeId.Equals(modalName));
            var intervalRepresent = subModalPersonInScene?.RepresentTypes?.FindAll(rt => !ReferenceEquals(null,  rt.IntervalData) && ReferenceEquals(null,  rt.EventData) && !rt.Index.HasValue);
            if(ReferenceEquals(null,  intervalRepresent) || intervalRepresent.Count == 0)
            {
                return null;
            }
            else
            {
                List<Interval> intervalList = new List<Interval>();
                foreach (var interval in intervalRepresent)
                {
                    intervalList.Add(new Interval()
                    {
                        Value = interval.Value,
                        Subtitle = interval.Subtitle,
                        Index = interval.Index,
                        StartTime = interval.IntervalData.StartTime,
                        EndTime = interval.IntervalData.EndTime
                    });
                }
                return intervalList.OrderBy(id => id.StartTime).ToList();
            }
        }

        public void FromEvent(int personId, string modalName, string subModalName, int millisecondsThreshold)
        {
            var eventAccess = new EventAccess();
            var events = eventAccess.GetAll(personId, modalName, subModalName);
            if (ReferenceEquals(null, events))
                throw new Exception("There are no events in ModalType:" + modalName + " and subModal:" + subModalName);
            TimeSpan? start = null, end = null;

            foreach (var timeEvent in events)
            {
                if (!start.HasValue && !end.HasValue)
                {
                    start = timeEvent.EventTime;
                    end = timeEvent.EventTime;
                }
                else if(end.HasValue && timeEvent.EventTime.Subtract(end.Value).TotalMilliseconds >= millisecondsThreshold)
                {
                    Add(personId, modalName, subModalName, start.Value, end.Value);
                    start = timeEvent.EventTime;
                }
                end = timeEvent.EventTime;

            }
            if (start.HasValue && end.HasValue)
            {
                Add(personId, modalName, subModalName, start.Value, end.Value);
            }
        }

        #region public add methods
        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value)
        {
            InternalAdd(personId, modalName, subModalName, startTime, endTime, value, null);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value, string subtitle)
        {
            InternalAdd(personId, modalName, subModalName, startTime, endTime, value, subtitle);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, string subtitle)
        {
            InternalAdd(personId, modalName, subModalName, startTime, endTime, null, subtitle);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime)
        {
            InternalAdd(personId, modalName, subModalName, startTime, endTime, null, null);
        }
        #endregion

        private void InternalAdd(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double? value, string subtitle)
        {
            var subModalPersonInScene = TypeValidation.GetSmtPis(personId, modalName, subModalName); 

            //create interval and add to smtPis
            IntervalData intervalElement = new IntervalData()
            {
                StartTime = startTime,
                EndTime = endTime
            };
            subModalPersonInScene.RepresentTypes.Add(new RepresentType()
            {
                Value = value,
                Subtitle = subtitle,
                Index = null,
                IntervalData = intervalElement,
                SubModalType_PersonInScene = subModalPersonInScene
            });

            
        }
    }
}
