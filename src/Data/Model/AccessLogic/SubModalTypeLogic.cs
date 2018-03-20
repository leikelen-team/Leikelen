using System;

namespace cl.uv.leikelen.src.Data.Model.AccessLogic
{
    public static class SubModalTypeLogic
    {
        public static void addInterval(this SubModalType smt, TimeSpan start, TimeSpan end)
        {
            if (smt.DataType != DataType.IntervalGroup) throw new Exception("Different DataType");
            smt.IntervalGroup.Add(new IntervalData(smt, start, end));
        }

        public static void addIntervalString(this SubModalType smt, TimeSpan start, TimeSpan end, string stringData)
        {
            if (smt.DataType != DataType.StringIntervalGroup) throw new Exception("Different DataType");
            smt.IntervalGroup.Add(new IntervalData(smt, start, end, stringData));
        }

        public static void addIntervalNumber(this SubModalType smt, TimeSpan start, TimeSpan end, double numberData)
        {
            if (smt.DataType != DataType.NumberIntervalGroup) throw new Exception("Different DataType");
            smt.IntervalGroup.Add(new IntervalData(smt, start, end, numberData));
        }

        public static void addIntervalStringNumber(this SubModalType smt, TimeSpan start, TimeSpan end, double numberData, string stringData)
        {
            if (smt.DataType != DataType.StringNumberIntervalGroup) throw new Exception("Different DataType");
            smt.IntervalGroup.Add(new IntervalData(smt, start, end, stringData, numberData));
        }

        public static void addEvent(this SubModalType smt, TimeSpan eventTime)
        {
            if (smt.DataType != DataType.Event) throw new Exception("Different DataType");
            smt.EventGroup.Add(new EventData(smt, eventTime));
        }

        public static void addEventString(this SubModalType smt, TimeSpan eventTime, string stringData)
        {
            if (smt.DataType != DataType.StringEvent) throw new Exception("Different DataType");
            smt.EventGroup.Add(new EventData(smt, eventTime, stringData));
        }

        public static void addEventNumber(this SubModalType smt, TimeSpan eventTime, double numberData)
        {
            if (smt.DataType != DataType.NumberEvent) throw new Exception("Different DataType");
            smt.EventGroup.Add(new EventData(smt, eventTime, numberData));
        }

        public static void addEventStringNumber(this SubModalType smt, TimeSpan eventTime, double numberData, string stringData)
        {
            if (smt.DataType != DataType.StringNumberEvent) throw new Exception("Different DataType");
            smt.EventGroup.Add(new EventData(smt, eventTime, stringData, numberData));
        }
    }
}
