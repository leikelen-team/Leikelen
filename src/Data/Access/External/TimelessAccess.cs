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
    public class TimelessAccess : ITimelessAccess
    {
        public List<Timeless> GetAll(int personId, string modalName, string subModalName)
        {
            var personInScene = SceneInUse.Instance.Scene.PersonInScenes.Find(pis => pis.PersonId == personId);
            var subModal_PersonInScene = personInScene.SubModalType_PersonInScenes.Find(smt_pis => smt_pis.SubModalType.Name == subModalName && smt_pis.SubModalType.ModalType.Name == modalName);
            var timelessRepresent = subModal_PersonInScene.RepresentType.FindAll(rt => rt.IntervalData == null && rt.EventData == null);
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
            return timelessList;
        }

        #region public add methods
        public void Add(int personId, string modalName, string subModalName, double value)
        {
            Add(personId, modalName, subModalName, value, null, null);
        }
        public void Add(int personId, string modalName, string subModalName, double value, string subtitle)
        {
            Add(personId, modalName, subModalName, value, subtitle, null);
        }
        public void Add(int personId, string modalName, string subModalName, double value, int index)
        {
            Add(personId, modalName, subModalName, value, null, index);
        }

        public void Add(int personId, string modalName, string subModalName, string subtitle)
        {
            Add(personId, modalName, subModalName, null, subtitle, null);
        }
        public void Add(int personId, string modalName, string subModalName, int index)
        {
            Add(personId, modalName, subModalName, null, null, index);
        }

        public void Add(int personId, string modalName, string subModalName, string subtitle, int index)
        {
            Add(personId, modalName, subModalName, null, subtitle, index);
        }

        public void Add(int personId, string modalName, string subModalName, double value, string subtitle, int index)
        {
            Add(personId, modalName, subModalName, value, subtitle, index);
        }
        #endregion

        private void Add(int personId, string modalName, string subModalName, double? value, string subtitle, int? index)
        {
            var personInScene = SceneInUse.Instance.Scene.PersonInScenes.Find(pis => pis.PersonId == personId);
            var subModal_PersonInScene = personInScene.SubModalType_PersonInScenes.Find(smt_pis => smt_pis.SubModalType.Name == subModalName && smt_pis.SubModalType.ModalType.Name == modalName);
            subModal_PersonInScene.RepresentType.Add(new RepresentType()
            {
                Value = value,
                Subtitle = subtitle,
                Index = index
            });
            var submodalModalKey = new Tuple<string, string>(modalName, subModalName);
            if (!TMPSubModalTypes.Instance.TemporalSubmodals.Contains(submodalModalKey))
            {
                TMPSubModalTypes.Instance.TemporalSubmodals.Add(submodalModalKey);
            }
        }
    }
}
