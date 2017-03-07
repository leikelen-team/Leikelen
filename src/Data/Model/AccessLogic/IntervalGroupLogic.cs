using System;

namespace cl.uv.leikelen.src.Data.Model.AccessLogic
{
    public static class IntervalGroupLogic
    {
        public static void addInterval(this IntervalGroup intervalGroup, Interval interval)
        {
            interval.IntervalGroup = intervalGroup;
            intervalGroup.Intervals.Add(interval);
        }

        public static void createAndAddInterval(this IntervalGroup intervalGroup, TimeSpan start, TimeSpan end, int millisecondsThreshold)
        {
            if (start >= end)
            {
                Console.WriteLine("ERROR!! : start time must be lower than end time");
                return;

            }
            bool exists = intervalGroup.Intervals.Exists(
                interval =>
                    interval.EndTime >= start
                );
            if (exists)
            {
                Console.WriteLine("ERROR!! : The start time must be greater than an existent end time interval");
            }
            if (end.Subtract(start).TotalMilliseconds >= millisecondsThreshold)
            {
                intervalGroup.Intervals.Add(new Interval(start, end, intervalGroup));
            }
            else
            {
                Console.WriteLine("### !! Interval SKIPPED!!: {0} - {1}", start, end);
            }
        }
    }
}
