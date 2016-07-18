using Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.analytics
{
    static class PersonAnalytic
    {
        public static IReadOnlyDictionary<PostureType, float> calculatePosturesAverage(this Person person)
        {
            int totalCount = person.microPostures.Count;
            Dictionary<PostureType, float> dic = new Dictionary<PostureType, float>();

            //Posture[] postures = (Posture[])Enum.GetValues(typeof(Posture));
            foreach (PostureType postureType in PostureType.availablesPostureTypes)
            {
                int postureCount = person.microPostures.Count(p => p.postureType.name == postureType.name);
                float avg = (float)postureCount / (float)totalCount;
                dic.Add(postureType, avg);
            }
            return dic;
        }

        public static IReadOnlyDictionary<string, string> calculateEmotionsAverageString(this Person person)
        {
            int totalCount = person.microPostures.Count;
            if (totalCount == 0) return null;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            
            foreach (PostureType postureType in PostureType.availablesPostureTypes)
            {
                int postureCount = person.microPostures.Count(p => p.postureType.name == postureType.name);
                if (postureCount == 0) continue;
                int avg = (int)(((float)postureCount / (float)totalCount) * 100.0f);
                dic.Add(postureType.name + " " + avg.ToString() + "%", avg.ToString());
            }

            return dic;
        }

        public static void generatePostureIntervals(this Person person)
        {
            if (person.microPostures.Count == 0) return;
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            
            foreach (PostureType currentPostureType in PostureType.availablesPostureTypes)
            {
                //System.Collections.IEnumerable microPostures = person.microPostures.Where(microPosture => microPosture.postureType.name == postureType.name);

                PostureIntervalGroup postureIntervalGroup = new PostureIntervalGroup(currentPostureType);
                MicroPosture initialMicroPosture = null, lastMicroPosture = null;
                TimeSpan threshold = TimeSpan.FromMilliseconds( Convert.ToDouble(Properties.Resources.PostureDurationDetectionThreshold) );
                //bool moreThanOneInterval = false;
                //bool intervalOpen = false;

                foreach (MicroPosture microPosture in person.microPostures)
                {
                    if (microPosture.postureType.name != currentPostureType.name) continue;
                    
                    if(lastMicroPosture == null)
                    {
                        initialMicroPosture = microPosture;
                        //intervalOpen = true;
                    }
                    else if( microPosture.sceneLocationTime.Subtract(lastMicroPosture.sceneLocationTime) >= threshold )
                    {
                        postureIntervalGroup.addInterval(initialMicroPosture, lastMicroPosture);
                        initialMicroPosture = microPosture;
                        //intervalOpen = true;
                    }
                    lastMicroPosture = microPosture;

                }
                if ( initialMicroPosture!=null)
                {
                    postureIntervalGroup.addInterval(initialMicroPosture, lastMicroPosture);
                }
                if (postureIntervalGroup.Intervals.Count>0) person.PostureIntervalGroups.Add(postureIntervalGroup);
            }
        }
    }
}
