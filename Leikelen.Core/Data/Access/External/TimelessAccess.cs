using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Access.External
{
    public class TimelessAccess : ITimelessAccess
    {
        public List<Timeless> GetAll(Person person, string modalName, string subModalName)
        {
            var personInScene = Internal.SceneInUse.Instance.Scene?.PersonsInScene?.Find(pis => pis.Person.Equals(person));
            var subModalPersonInScene = personInScene?.SubModalType_PersonInScenes?.Find(smtPis => smtPis.SubModalType.SubModalTypeId.Equals(subModalName) && smtPis.SubModalType.ModalType.ModalTypeId.Equals(modalName));
            var timelessRepresent = subModalPersonInScene?.RepresentTypes?.FindAll(rt => ReferenceEquals(null, rt.IntervalData) && ReferenceEquals(null, rt.EventData) && rt.Index.HasValue);
            if(ReferenceEquals(null, timelessRepresent) || timelessRepresent.Count == 0)
            {
                return null;
            }
            else
            {
                List<Timeless> timelessList = new List<Timeless>();
                foreach (var timeless in timelessRepresent)
                {
                    timelessList.Add(new Timeless()
                    {
                        Value = timeless.Value,
                        Subtitle = timeless.Subtitle,
                        Index = timeless.Index
                    });
                }
                return timelessList.OrderBy(t => t.Index).ToList();
            }
        }

        #region public add methods
        public void Add(Person person, string modalName, string subModalName, int index, double value)
        {
            InternalAdd(person, modalName, subModalName, index, value, null);
        }

        public void Add(Person person, string modalName, string subModalName, int index, double value, string subtitle)
        {
            InternalAdd(person, modalName, subModalName, index, value, subtitle);
        }

        public void Add(Person person, string modalName, string subModalName, int index, string subtitle)
        {
            InternalAdd(person, modalName, subModalName, index, null, subtitle);
        }

        public void Add(Person person, string modalName, string subModalName, int index)
        {
            InternalAdd(person, modalName, subModalName, index, null, null);
        }
        #endregion

        private void InternalAdd(Person person, string modalName, string subModalName, int index, double? value, string subtitle)
        {
            var subModalPersonInScene = TypeValidation.GetSmtPis(person, modalName, subModalName);
            
            //create event and add to smtPis
            subModalPersonInScene.RepresentTypes.Add(new RepresentType()
            {
                Value = value,
                Subtitle = subtitle,
                Index = index,
                SubModalType_PersonInScene = subModalPersonInScene
            });
        }
    }
}
