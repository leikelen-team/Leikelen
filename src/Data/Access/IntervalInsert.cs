using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API;
using cl.uv.leikelen.src.Data.Model;
using cl.uv.leikelen.src.Data.Model.AccessLogic;

namespace cl.uv.leikelen.src.Data.Access
{
    public class IntervalInsert : IIntervalInsert
    {
        public void AddInterval(int personId, string modalTypeName, string subModalTypeName, TimeSpan start, TimeSpan end)
        {
            if (!hasSubModalType(personId, modalTypeName, subModalTypeName)) throw new Exception("No such person, modaltype or submodaltype");
            PersonInScene pis = StaticScene.Instance.getPersonInScene(personId);
            pis.getModalType(modalTypeName).getSubModalType(subModalTypeName).addInterval(start, end);
        }

        public void AddNumberInterval(int personId, string modalTypeName, string subModalTypeName, TimeSpan start, TimeSpan end, double numberData)
        {
            if (!hasSubModalType(personId, modalTypeName, subModalTypeName)) throw new Exception("No such person, modaltype or submodaltype");
            PersonInScene pis = StaticScene.Instance.getPersonInScene(personId);
            pis.getModalType(modalTypeName).getSubModalType(subModalTypeName).addIntervalNumber(start, end, numberData);
        }

        public void AddStringInterval(int personId, string modalTypeName, string subModalTypeName, TimeSpan start, TimeSpan end, string stringData)
        {
            if (!hasSubModalType(personId, modalTypeName, subModalTypeName)) throw new Exception("No such person, modaltype or submodaltype");
            PersonInScene pis = StaticScene.Instance.getPersonInScene(personId);
            pis.getModalType(modalTypeName).getSubModalType(subModalTypeName).addIntervalString(start, end, stringData);
        }

        public void AddStringNumberInterval(int personId, string modalTypeName, string subModalTypeName, TimeSpan start, TimeSpan end, string stringData, double numberData)
        {
            if (!hasSubModalType(personId, modalTypeName, subModalTypeName)) throw new Exception("No such person, modaltype or submodaltype");
            PersonInScene pis = StaticScene.Instance.getPersonInScene(personId);
            pis.getModalType(modalTypeName).getSubModalType(subModalTypeName).addIntervalStringNumber(start, end, numberData, stringData);
        }

        private bool hasSubModalType(int personId, string modalTypeName, string subModalTypeName)
        {
            PersonInScene pis;
            if (!StaticScene.Instance.isPersonInScene(personId)) return false;
            pis = StaticScene.Instance.getPersonInScene(personId);
            if (pis.hasModalType(modalTypeName))
            {
                if (pis.getModalType(modalTypeName).hasSubModalType(subModalTypeName))
                {
                    return true;
                }
            }
            return false;
        }

        public bool createAndAddInterval(List<IntervalData> intervalGroup, TimeSpan start, TimeSpan end, int millisecondsThreshold)
        {
            if (start >= end)
            {
                return false;

            }
            bool exists = intervalGroup.Exists(
                interval =>
                    interval.EndTime >= start
                );
            if (exists)
            {
                return false;

            }
            if (end.Subtract(start).TotalMilliseconds < millisecondsThreshold)
            {
                return false;
            }
            else
            {
                //TODO: insertar intervalo
                return true;
            }
        }

        public bool generateFromEvent(int personId, string eventModalTypeName, string eventSubModalTypeName, string intervalModalTypeName, string intervalSubModalTypeName, int millisecondsThreshold)
        {
            if (!hasSubModalType(personId, eventModalTypeName, eventSubModalTypeName)) return false;
            PersonInScene pis = StaticScene.Instance.getPersonInScene(personId);
            //TODO: comprobar tipo que sea evento
            List<EventData> events = pis.getModalType(eventModalTypeName).getSubModalType(eventSubModalTypeName).EventGroup;

            SubModalType intervalSubModal = pis.addModalType(intervalModalTypeName, null).addSubModalType(intervalSubModalTypeName, null, null, DataType.IntervalGroup);
            TimeSpan? start = null, end = null;
            int threshold = millisecondsThreshold;

            foreach (var timeEvent in events)
            {
                if (end == null)
                {
                    start = timeEvent.EventTime;
                }
                else if (end.Value.Subtract(start.Value).TotalMilliseconds >= millisecondsThreshold)
                {
                    intervalSubModal.addInterval(start.Value, end.Value);
                    start = timeEvent.EventTime;
                }
                end = timeEvent.EventTime;

            }
            if (start != null)
            {
                intervalSubModal.addInterval(start.Value, end.Value);
            }
            return true;
        }
    }
}
