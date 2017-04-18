using System;

namespace cl.uv.leikelen.src.Data.Model
{
    public class EventData
    {
        public int EventDataId { get; set; }
        public TimeSpan EventTime { get; set; }
        public string StringData { get; set; }
        public double? NumberData { get; set; }

        public int SubModalTypeId { get; set; }
        public SubModalType SubModalType { get; set; }

        public EventData() { }

        public EventData(SubModalType smt, TimeSpan eventTime)
        {
            this.SubModalType = smt;
            this.EventTime = eventTime;
            this.StringData = null;
            this.NumberData = null;
        }

        public EventData(SubModalType smt, TimeSpan eventTime, string stringData)
        {
            this.SubModalType = smt;
            this.EventTime = eventTime;
            this.StringData = stringData;
            this.NumberData = null;
        }

        public EventData(SubModalType smt, TimeSpan eventTime, double numberData)
        {
            this.SubModalType = smt;
            this.EventTime = eventTime;
            this.StringData = null;
            this.NumberData = numberData;
        }

        public EventData(SubModalType smt, TimeSpan eventTime, string stringData, double numberData)
        {
            this.SubModalType = smt;
            this.EventTime = eventTime;
            this.StringData = stringData;
            this.NumberData = numberData;
        }
    }
}
