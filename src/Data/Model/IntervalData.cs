using System;

namespace cl.uv.leikelen.src.Data.Model
{
    public class IntervalData
    {
        public int IntervalId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string StringData { get; set; }
        public double? NumberData { get; set; }

        public int SubModalTypeId { get; set; }
        public SubModalType SubModalType { get; set; }

        public IntervalData() { }

        public IntervalData(SubModalType smt, TimeSpan start, TimeSpan end)
        {
            this.SubModalType = smt;
            this.StartTime = start;
            this.EndTime = end;
            this.StringData = null;
            this.NumberData = null;
        }

        public IntervalData(SubModalType smt, TimeSpan start, TimeSpan end, string stringData)
        {
            this.SubModalType = smt;
            this.StartTime = start;
            this.EndTime = end;
            this.StringData = stringData;
            this.NumberData = null;
        }

        public IntervalData(SubModalType smt, TimeSpan start, TimeSpan end, double numberData)
        {
            this.SubModalType = smt;
            this.StartTime = start;
            this.EndTime = end;
            this.StringData = null;
            this.NumberData = numberData;
        }

        public IntervalData(SubModalType smt, TimeSpan start, TimeSpan end, string stringData, double numberData)
        {
            this.SubModalType = smt;
            this.StartTime = start;
            this.EndTime = end;
            this.StringData = stringData;
            this.NumberData = numberData;
        }
    }
}
