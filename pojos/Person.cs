using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos
{
    public class Person
    {
        public enum Gender { Masculino, Femenino};

        public int bodyIndex { get; set; }
        public string name { get; set; }
        public Gender gender { get; set; }
        public int age { get; set; }

        public List<Posture> postures { get; private set; }
        //public List<Interval> intervals { get; private set; }

        public Person(int bodyIndex, string name, Gender gender, int age)
        {
            this.bodyIndex = bodyIndex;
            this.name = name;
            this.gender = gender;
            this.age = age;
            this.postures = new List<Posture>();
        }

        public IReadOnlyDictionary<Emotion, float> calculateEmotionsAverage()
        {
            int totalCount = postures.Count;
            Dictionary<Emotion, float> dic = new Dictionary<Emotion, float>();
            
            Emotion[] emotions = (Emotion[])Enum.GetValues(typeof(Emotion));
            foreach(Emotion emotion in emotions)
            {
                int emotionCount = postures.Count(p => p.emotion == emotion);
                float avg = (float)emotionCount / (float)totalCount;
                dic.Add(emotion, avg);
            }

            return dic;
        }

        public IReadOnlyDictionary<string, string> calculateEmotionsAverageString()
        {
            int totalCount = postures.Count;
            if (totalCount == 0) return null;
            Dictionary<string, string> dic = new Dictionary<string, string>();

            Emotion[] emotions = (Emotion[])Enum.GetValues(typeof(Emotion));
            foreach (Emotion emotion in emotions)
            {
                int emotionCount = postures.Count(p => p.emotion == emotion);
                if (emotionCount == 0) continue;
                int avg = (int) (((float)emotionCount / (float)totalCount) * 100.0f);
                dic.Add(emotion.ToString("g")+" "+avg.ToString()+"%", avg.ToString());
            }

            return dic;
        }

        public void showPersonInfo()
        {
            IReadOnlyDictionary<Emotion, float> emotionsAvg = calculateEmotionsAverage();

            Console.WriteLine("Nombre: "+name);
            Console.WriteLine("Genero: "+gender.ToString("g"));
            Console.WriteLine("Edad: "+age);

            foreach (Emotion emotion in emotionsAvg.Keys)
            {
                Console.WriteLine(emotion.ToString("g") + ": " + emotionsAvg[emotion].ToString("0.00"));
            }

            ChartForm chartForm = new ChartForm(this);
            chartForm.Show();

        }

        
    }
}
