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
    public class EventInsert : IEventInsert
    {
        public void AddEvent(int personId, string modalTypeName, string subModalTypeName, TimeSpan time, bool create)
        {
            PersonInScene pis = StaticScene.Instance.getPersonInScene(personId);
            if (pis == null) throw new Exception("No such person");
            if (!hasSubModalType(personId, modalTypeName, subModalTypeName))
            {
                if (create)
                {
                    pis.addModalType(modalTypeName, null).addSubModalType(subModalTypeName, null, null, DataType.Event);
                }
                else
                {
                    throw new Exception("No such modaltype or submodaltype");
                }
            }
            pis.getModalType(modalTypeName).getSubModalType(subModalTypeName).addEvent(time);
        }

        public void AddNumberEvent(int personId, string modalTypeName, string subModalTypeName, TimeSpan time, double numberData, bool create)
        {
            PersonInScene pis = StaticScene.Instance.getPersonInScene(personId);
            if (pis == null) throw new Exception("No such person");
            if (!hasSubModalType(personId, modalTypeName, subModalTypeName))
            {
                if (create)
                {
                    pis.addModalType(modalTypeName, null).addSubModalType(subModalTypeName, null, null, DataType.NumberEvent);
                }
                else
                {
                    throw new Exception("No such modaltype or submodaltype");
                }
            }
            pis.getModalType(modalTypeName).getSubModalType(subModalTypeName).addEventNumber(time, numberData);
        }

        public void AddStringEvent(int personId, string modalTypeName, string subModalTypeName, TimeSpan time, string stringData, bool create)
        {
            PersonInScene pis = StaticScene.Instance.getPersonInScene(personId);
            if (pis == null) throw new Exception("No such person");
            if (!hasSubModalType(personId, modalTypeName, subModalTypeName))
            {
                if (create)
                {
                    pis.addModalType(modalTypeName, null).addSubModalType(subModalTypeName, null, null, DataType.StringEvent);
                }
                else
                {
                    throw new Exception("No such modaltype or submodaltype");
                }
            }
            pis.getModalType(modalTypeName).getSubModalType(subModalTypeName).addEventString(time, stringData);
        }

        public void AddStringNumberEvent(int personId, string modalTypeName, string subModalTypeName, TimeSpan time, string stringData, double numberData, bool create)
        {
            PersonInScene pis = StaticScene.Instance.getPersonInScene(personId);
            if (pis == null) throw new Exception("No such person");
            if (!hasSubModalType(personId, modalTypeName, subModalTypeName))
            {
                if (create)
                {
                    pis.addModalType(modalTypeName, null).addSubModalType(subModalTypeName, null, null, DataType.StringNumberEvent);
                }
                else
                {
                    throw new Exception("No such modaltype or submodaltype");
                }
            }
            pis.getModalType(modalTypeName).getSubModalType(subModalTypeName).addEventStringNumber(time, numberData, stringData);
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

    }
}
