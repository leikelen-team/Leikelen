using System;

namespace cl.uv.leikelen.src.Data.Model.AccessLogic
{
    public static class SubModalTypeLogic
    {
        public static IntervalGroup getIG(this SubModalType smt)
        {
            if(smt.IntervalGroup != null)
            {
                return smt.IntervalGroup;
            }
            else
            {
                return smt.IntervalGroup = new IntervalGroup(smt);
            }
        }

        public static void addEventData(this SubModalType smt, TimeSpan eventTime)
        {
            smt.EventDatas.Add(new EventData(smt, eventTime));
        }
    }
}
