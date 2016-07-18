using Microsoft.Samples.Kinect.VisualizadorMultimodal.analytics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos
{
    public class Person : INotifyPropertyChanged
    {
        public enum Gender { Masculino, Femenino };
        private static Brush[] colors = {Brushes.Red, Brushes.Orange, Brushes.Green, Brushes.Blue, Brushes.Indigo, Brushes.Violet };

        public int bodyIndex { get; set; }
        public string name { get; set; }
        public Gender gender { get; set; }
        public int age { get; set; }

        public List<MicroPosture> microPostures { get; private set; }
        private List<PostureIntervalGroup> postureIntervalGroups = null;

        public List<PostureIntervalGroup> PostureIntervalGroups
        {
            get {
                if (postureIntervalGroups == null)
                {
                    postureIntervalGroups = new List<PostureIntervalGroup>();
                    this.generatePostureIntervals();
                }
                return postureIntervalGroups;
            }
        }
        
        public Brush Color {
            get
            {
                return colors[bodyIndex];
            }
        }

        public Person(int bodyIndex, string name, Gender gender, int age)
        {
            this.bodyIndex = bodyIndex;
            this.name = name;
            this.gender = gender;
            this.age = age;
            //this.postures = new Dictionary<TimeSpan, PostureType>();
            this.microPostures = new List<MicroPosture>();
            this.postureIntervalGroups = null;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        public bool HasBeenTracked
        {
            get { return microPostures.Count != 0; }
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

        
    }
}
