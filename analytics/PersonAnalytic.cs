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
        public static IReadOnlyDictionary<Posture, float> calculatePosturesAverage(this Person person)
        {
            int totalCount = person.postures.Count;
            Dictionary<Posture, float> dic = new Dictionary<Posture, float>();

            //Posture[] postures = (Posture[])Enum.GetValues(typeof(Posture));
            foreach (Posture posture in Posture.posturesAvailables)
            {
                int postureCount = person.postures.Count(p => p.Value.name == posture.name);
                float avg = (float)postureCount / (float)totalCount;
                dic.Add(posture, avg);
            }
            return dic;
        }

        public static IReadOnlyDictionary<string, string> calculateEmotionsAverageString(this Person person)
        {
            int totalCount = person.postures.Count;
            if (totalCount == 0) return null;
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //Posture.Name[] postures = (Posture.Name[])Enum.GetValues(typeof(Posture.Name));
            foreach (Posture currentPosture in Posture.posturesAvailables)
            {
                int postureCount = person.postures.Count(p => p.Value.name == currentPosture.name);
                if (postureCount == 0) continue;
                int avg = (int)(((float)postureCount / (float)totalCount) * 100.0f);
                dic.Add(currentPosture.name + " " + avg.ToString() + "%", avg.ToString());
            }

            return dic;
        }
    }
}
