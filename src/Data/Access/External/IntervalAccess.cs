﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Access.External
{
    class IntervalAccess : IIntervalAccess
    {
        public List<Interval> GetAll(int personId, string modalName, string subModalName)
        {
            var personInScene = SceneInUse.Instance.Scene.PersonInScenes.Find(pis => pis.PersonId == personId);
            var subModalPersonInScene = personInScene.SubModalType_PersonInScenes.Find(smtPis => smtPis.SubModalType.Name == subModalName && smtPis.SubModalType.ModalType.Name == modalName);
            var intervalRepresent = subModalPersonInScene.RepresentType.FindAll(rt => rt.IntervalData != null && rt.EventData == null);
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
            return intervalList;
        }

        #region public add methods
        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value)
        {
            InternalAdd(personId, modalName, subModalName, startTime, endTime, value, null, null);
        }
        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value, string subtitle)
        {
            InternalAdd(personId, modalName, subModalName, startTime, endTime, value, subtitle, null);
        }
        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value, int index)
        {
            Add(personId, modalName, subModalName, startTime, endTime, value, null, index);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, string subtitle)
        {
            InternalAdd(personId, modalName, subModalName, startTime, endTime, null, subtitle, null);
        }
        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, int index)
        {
            InternalAdd(personId, modalName, subModalName, startTime, endTime, null, null, index);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, string subtitle, int index)
        {
            InternalAdd(personId, modalName, subModalName, startTime, endTime, null, subtitle, index);
        }

        public void Add(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double value, string subtitle, int index)
        {
            InternalAdd(personId, modalName, subModalName, startTime, endTime, value, subtitle, index);
        }
        #endregion

        private void InternalAdd(int personId, string modalName, string subModalName, TimeSpan startTime, TimeSpan endTime, double? value, string subtitle, int? index)
        {
            var personInScene = SceneInUse.Instance.Scene.PersonInScenes.Find(pis => pis.PersonId == personId);
            var subModalPersonInScene = personInScene.SubModalType_PersonInScenes.Find(smtPis => smtPis.SubModalType.Name == subModalName && smtPis.SubModalType.ModalType.Name == modalName);
            IntervalData intervalElement = new IntervalData()
            {
                StartTime = startTime,
                EndTime = endTime
            };
            subModalPersonInScene.RepresentType.Add(new RepresentType()
            {
                Value = value,
                Subtitle = subtitle,
                Index = index,
                IntervalData = intervalElement
            });
            var submodalModalKey = new Tuple<string, string>(modalName, subModalName);
            if (!TmpSubModalTypes.Instance.TemporalSubmodals.Contains(submodalModalKey))
            {
                TmpSubModalTypes.Instance.TemporalSubmodals.Add(submodalModalKey);
            }
        }
    }
}
