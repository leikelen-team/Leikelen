using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;
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
            int totalCount = person.MicroPostures.Count;
            Dictionary<PostureType, float> dic = new Dictionary<PostureType, float>();

            ////Posture[] postures = (Posture[])Enum.GetValues(typeof(Posture));
            //foreach (PostureType postureType in PostureType.availablesPostureTypes)
            //{
            //    int postureCount = person.MicroPostures.Count(p => p.PostureType.Name == postureType.Name);
            //    float avg = (float)postureCount / (float)totalCount;
            //    dic.Add(postureType, avg);
            //}
            return dic;
        }

        public static IReadOnlyDictionary<string, string> calculateEmotionsAverageString(this Person person)
        {
            int totalCount = person.MicroPostures.Count;
            if (totalCount == 0) return null;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            
            //foreach (PostureType postureType in PostureType.availablesPostureTypes)
            //{
            //    int postureCount = person.MicroPostures.Count(p => p.PostureType.Name == postureType.Name);
            //    if (postureCount == 0) continue;
            //    int avg = (int)(((float)postureCount / (float)totalCount) * 100.0f);
            //    dic.Add(postureType.Name + " " + avg.ToString() + "%", avg.ToString());
            //}

            return dic;
        }

        
    }
}
