using Microsoft.Samples.Kinect.VisualizadorMultimodal.analytics;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.views;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class Person
    {
        
        
        public int PersonId { get; set; }
        public int bodyIndex { get; set; }
        public string name { get; set; }
        public Gender gender { get; set; }
        public int age { get; set; }
        public List<PostureIntervalGroup> PostureIntervalGroups { get; set; }

        public int SceneId { get; set; }
        public Scene Scene { get; set; }

        //[NotMapped]
        public List<MicroPosture> MicroPostures { get; private set; }

        [NotMapped]
        public PersonView view { get; set; }

        [NotMapped]
        private static Brush[] colors = { Brushes.Red, Brushes.Orange, Brushes.Green, Brushes.Blue, Brushes.Indigo, Brushes.Violet };

        [NotMapped]
        public Brush Color {
            get
            {
                return colors[bodyIndex];
            }
        }

        [NotMapped]
        public bool HasBeenTracked
        {
            get { return MicroPostures.Count != 0; }
        }

        public enum Gender { Masculino, Femenino };
        public Person(int bodyIndex, string name, Gender gender, int age)
        //public Person(int bodyIndex, string name)
        {
            this.bodyIndex = bodyIndex;
            this.name = name;
            this.gender = gender;
            this.age = age;
            this.MicroPostures = new List<MicroPosture>();
            this.PostureIntervalGroups = new List<PostureIntervalGroup>();
            this.view = null;

        }

        

        public void showPersonInfo()
        {
            IReadOnlyDictionary<PostureType, float> posturesAvg = this.calculatePosturesAverage();

            Console.WriteLine("Nombre: "+name);
            Console.WriteLine("Genero: "+gender.ToString("g"));
            Console.WriteLine("Edad: "+age);

            foreach (PostureType posture in posturesAvg.Keys)
            {
                Console.WriteLine(posture.name + ": " + posturesAvg[posture].ToString("0.00"));
            }

            ChartForm chartForm = new ChartForm(this);
            chartForm.Show();

        }

        public void generatePostureIntervals()
        {
            if (this.MicroPostures.Count == 0) return;
            //Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (PostureType currentPostureType in PostureType.availablesPostureTypes)
            {
                //System.Collections.IEnumerable microPostures = person.microPostures.Where(microPosture => microPosture.postureType.name == postureType.name);

                PostureIntervalGroup postureIntervalGroup = new PostureIntervalGroup(currentPostureType);
                MicroPosture initialMicroPosture = null, lastMicroPosture = null;
                TimeSpan threshold = TimeSpan.FromMilliseconds(Convert.ToDouble(Properties.Resources.PostureDurationDetectionThreshold));
                //bool moreThanOneInterval = false;
                //bool intervalOpen = false;

                foreach (MicroPosture microPosture in this.MicroPostures)
                {
                    if (microPosture.PostureType.name != currentPostureType.name) continue;

                    if (lastMicroPosture == null)
                    {
                        initialMicroPosture = microPosture;
                        //intervalOpen = true;
                    }
                    else if (microPosture.SceneLocationTime.Subtract(lastMicroPosture.SceneLocationTime) >= threshold)
                    {
                        postureIntervalGroup.addInterval(initialMicroPosture, lastMicroPosture);
                        initialMicroPosture = microPosture;
                        //intervalOpen = true;
                    }
                    lastMicroPosture = microPosture;

                }
                if (initialMicroPosture != null)
                {
                    postureIntervalGroup.addInterval(initialMicroPosture, lastMicroPosture);
                }
                if (postureIntervalGroup.Intervals.Count > 0) this.PostureIntervalGroups.Add(postureIntervalGroup);
            }
        }


    }
}
