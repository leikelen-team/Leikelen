﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.multimodalvisualizer.models
{
    public class PostureIntervalGroup
    {
        public int PostureIntervalGroupId { get; set; }
        public List<Interval> Intervals { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
        public int PostureTypeId { get; private set; }
        public PostureType PostureType { get; private set; }

        public PostureIntervalGroup() { }

        public PostureIntervalGroup(PostureType postureType)
        {
            this.PostureType = postureType;
            this.Intervals = new List<Interval>();
        }

        public TimeSpan CalculateTotalDuration()
        {
            TimeSpan? durationSum = null;
            foreach (var interval in Intervals)
            {
                if (durationSum != null) durationSum = interval.Duration.Add(durationSum.Value);
                else durationSum = interval.Duration;
            }
            if (!durationSum.HasValue) return TimeSpan.FromSeconds(0);
            return durationSum.Value;
        }
        
        public void addInterval(MicroPosture initialMicroPosture, MicroPosture finalMicroPosture)
        {
            if(initialMicroPosture.PostureType != this.PostureType || finalMicroPosture.PostureType != this.PostureType)
            {
                //throw new Exception("You are adding an interval with diferent postureType to this IntervalPosture object");
                Console.WriteLine("ERROR!! : You are adding an interval with diferent postureType to this IntervalPosture object");
                return;
            }
            if(initialMicroPosture.SceneLocationTime >= finalMicroPosture.SceneLocationTime)
            {
                //throw new Exception("initialMicroPosture must be lower than finalMicroPosture");
                Console.WriteLine("ERROR!! : initialMicroPosture must be lower than finalMicroPosture");
                return;

            }
            bool exists = this.Intervals.Exists(
                interval =>
                    //tuple.Item1.sceneLocationTime >= initialMicroPosture.sceneLocationTime ||
                    interval.EndTime >= initialMicroPosture.SceneLocationTime
                );
            if (exists)
            {
                //throw new Exception("The initialMicroPosture must be greater than an existent finalMicroPosture interval");
                Console.WriteLine("ERROR!! : The initialMicroPosture must be greater than an existent finalMicroPosture interval");
            }
            int threshold = Convert.ToInt32(Properties.Resources.MinPostureIntervalDuration);
            if (finalMicroPosture.SceneLocationTime.Subtract(initialMicroPosture.SceneLocationTime).TotalMilliseconds >= threshold)
                Intervals.Add(new Interval(initialMicroPosture, finalMicroPosture));
            else
            {
                Console.WriteLine("### !! Interval SKIPPED!!: {0} - {1}", initialMicroPosture.SceneLocationTime, finalMicroPosture.SceneLocationTime);
            }
        }

        public void addAudioBeamInterval(TimeSpan startTime, TimeSpan duration)
        {
            Interval IntervalTMP = new Interval();
            IntervalTMP.StartTime = startTime;
            IntervalTMP.EndTime = startTime.Add(duration);
            IntervalTMP.Duration = duration;
            Intervals.Add(IntervalTMP);
        }

        //public IReadOnlyList<Tuple<MicroPosture, MicroPosture>> Intervals {
        //    get
        //    {
        //        return this.intervals;
        //    }
        //}
    }
}
