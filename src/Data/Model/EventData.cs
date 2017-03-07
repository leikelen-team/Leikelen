using System;

namespace cl.uv.leikelen.src.Data.Model
{
    public class EventData
    {
        public int EventDataId { get; set; }
        public TimeSpan EventTime { get; set; }

        public int SubModalTypeId { get; set; }
        public SubModalType SubModalType { get; set; }

        public EventData() { }

        public EventData(SubModalType smt, TimeSpan eventTime)
        {
            this.SubModalType = smt;
            this.EventTime = eventTime;
        }
    }
}
