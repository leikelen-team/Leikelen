using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.multimodalvisualizer.models;
using cl.uv.multimodalvisualizer.db;
using cl.uv.multimodalvisualizer.utils;

namespace cl.uv.multimodalvisualizer.core
{
    public class GenerateGestures
    {
        public GenerateGestures()
        {

        }

        public static void generate_Gestures()
        {
            if (PostureTypeContext.db == null)
                return;
            List<myGesture> gestureList = PostureTypeContext.db.myGesture.ToList();
            foreach (myGesture gesture in gestureList)
            {
                foreach(Person person in Scene.Instance.Persons)
                {
                    PostureType contType = gesture.continuousPosture;
                    List<MicroPosture> mpostures = person.MicroPostures;
                    List<MicroPosture> contMPostures = mpostures.FindAll(mc => mc.GestureType == Microsoft.Kinect.VisualGestureBuilder.GestureType.Continuous && mc.PostureType == contType);
                    double[] time = new double[contMPostures.Count];
                    double[] progress = new double[contMPostures.Count];
                    contMPostures.Sort(delegate (MicroPosture x, MicroPosture y)
                    {
                        if (x.SceneLocationTime > y.SceneLocationTime) return 1;
                        else return -1;
                    });

                    for(int i=0; i< contMPostures.Count; i++)
                    {
                        time[i] = contMPostures[i].SceneLocationTime.TotalMilliseconds;
                        progress[i] = contMPostures[i].Progress;
                    }
                    List<Interval> startIntervals = person.PostureIntervalGroups.Find(pig => pig.PostureType == gesture.startPosture).Intervals;
                    List<Interval> endIntervals = person.PostureIntervalGroups.Find(pig => pig.PostureType == gesture.endPosture).Intervals;

                    for (int i = 0; i < startIntervals.Count; i++)
                    {
                        for(int j = 0; j <  endIntervals.Count; j++)
                        {
                            if (i < startIntervals.Count - 1 
                                && endIntervals[j].StartTime > startIntervals[i].EndTime 
                                && endIntervals[j].StartTime < startIntervals[i + 1].StartTime)
                            {
                                int startIndex = 0;
                                int endIndex = 0;
                                for (startIndex = 0; startIndex < time.Length; startIndex++)
                                {
                                    if(time[startIndex] > startIntervals[i].EndTime.TotalMilliseconds)
                                    {
                                        break;
                                    }
                                }

                                for (endIndex = 0; endIndex < time.Length; endIndex++)
                                {
                                    if (time[endIndex] > endIntervals[i].StartTime.TotalMilliseconds)
                                    {
                                        break;
                                    }
                                }

                                double rsquared;
                                double yintercept;
                                double slope;

                                LinearRegression.genLinearRegression(time, progress, startIndex, endIndex, out rsquared, out yintercept, out slope);

                                if(slope > 0)
                                {
                                    gesture.gestureIntervalGroup.addByStartAndEnd(startIntervals[i].EndTime, endIntervals[i].StartTime);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
