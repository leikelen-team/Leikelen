using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class Scene
    {
        private static Scene instance;

        public string name { get; private set; }
        public string description { get; set; }
        public DateTime startDate { get; private set; } // start date when begin to record
        public TimeSpan duration { get; private set; }

        public List<Person> persons { get; private set; }
        //public List<_Interval> intervals { get; private set; }

        private Scene(string name, DateTime startDate, TimeSpan duration)
        {
            this.name = name;
            this.startDate = startDate;
            this.duration = duration;
        }

        public static Scene Create(string name, DateTime startDate, TimeSpan duration)
        {
            instance = new Scene(name, startDate, duration);
            instance.persons = new List<Person>(6);
            for (int i = 1; i <= 6; i++)
            {
                instance.persons.Add(new Person(i-1, "Sujeto "+i, Person.Gender.Masculino, 0));
            }
            return instance;
        }
        public static Scene Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
