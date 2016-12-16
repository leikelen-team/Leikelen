using Microsoft.Kinect;
//using cl.uv.multimodalvisualizer.analytics;
using cl.uv.multimodalvisualizer.kinectmedia;
using cl.uv.multimodalvisualizer.db;
using cl.uv.multimodalvisualizer.views;
using cl.uv.multimodalvisualizer.windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.Kinect.VisualGestureBuilder;
using cl.uv.multimodalvisualizer.interfaces;

namespace cl.uv.multimodalvisualizer.models
{
    public class Person: IPerson
    {
        //private IHumanModal postureModal = new IPosture();

        public IHumanModal PostureModal {
            get
            {
                return null;
            }
            set
            {
                //PostureModal.RepresentType
            }
        }

        //[NotMapped]
        public int ListIndex { get; private set; }

        public int PersonId { get; set; }
        //public int BodyIndex { get; set; }
        //[Column(TypeName = "bigint")]
        public long TrackingId { get; set; }
        [NotMapped]
        private string name;
        public string Name {
            get
            {
                return name;
            }
            set
            {
                name = value;
                if (this.View != null) this.View.Label.Content = name;
            }
        }
        public GenderEnum Gender { get; set; }
        public int Age { get; set; }
        public List<PostureIntervalGroup> PostureIntervalGroups { get; set; }
        //[NotMapped]
        public List<MicroPosture> MicroPostures { get; set; }

        [NotMapped]
        public PostureIntervalGroup pigVoz = new PostureIntervalGroup(new PostureType("Voz Muchas", ""));

        //public int BodyDistanceId { get; set; }
        public List<Distance> Distances { get; set; }
        public Dictionary<Tuple<DistanceInferred, DistanceTypes>, double> DistancesSum { get; set; }
        public Dictionary<Tuple<DistanceInferred, DistanceTypes>, double> DistancesEntropy { get; set; }

        public Dictionary<TimeSpan, List<Distance>> IntervalDistances { get; set; }
        public Dictionary<Tuple<DistanceInferred, DistanceTypes>, Dictionary<TimeSpan, double>> IntervalDistancesSum { get; set; }
        public Dictionary<Tuple<DistanceInferred, DistanceTypes>, Dictionary<TimeSpan, double>> IntervalDistancesEntropy { get; set; }


        public int SceneId { get; set; }
        public IScene Scene { get; set; }

        [NotMapped]
        public Brush Color { get; private set; }

        [NotMapped]
        public bool HasBeenTracked
        {
            get {
                return
                    (PostureIntervalGroups != null && PostureIntervalGroups.Count != 0) ||
                    (MicroPostures!=null && MicroPostures.Count != 0);
            }
        }

        [NotMapped]
        public PersonView View { get; private set; }

        [NotMapped]
        private static Brush[] colors = {
            Brushes.Red,
            Brushes.Orange,
            Brushes.Green,
            Brushes.Blue,
            Brushes.Indigo,
            Brushes.Violet };

        [NotMapped]
        private static int currentColorIndex = 0;
       
        public Person() {
            this.Color = colors[currentColorIndex++];
            if (currentColorIndex == colors.Count()) currentColorIndex = 0;
        }

        //public enum GenderEnum { Male, Female };
        public Person(ulong trackingId, int listIndex/*int bodyIndex, string name, GenderEnum gender, int age*/)
        //public Person(int bodyIndex, string name)
        {
            //this.BodyIndex = bodyIndex;
            this.TrackingId = (long)trackingId;
            this.ListIndex = listIndex;
            this.Name = "Person "+ listIndex;
            //this.Gender = null;
            //this.Age = null;
            this.MicroPostures = new List<MicroPosture>();
            this.PostureIntervalGroups = new List<PostureIntervalGroup>();
            //this.BodyDistance = new BodyDistance();
            this.Distances = new List<Distance>();
            this.IntervalDistances = new Dictionary<TimeSpan, List<Distance>>();
            this.DistancesSum = new Dictionary<Tuple<DistanceInferred, DistanceTypes>, double>();
            this.DistancesEntropy = new Dictionary<Tuple<DistanceInferred, DistanceTypes>, double>();
            this.IntervalDistancesSum = new Dictionary<Tuple<DistanceInferred, DistanceTypes>, Dictionary<TimeSpan, double>>();
            this.IntervalDistancesEntropy = new Dictionary<Tuple<DistanceInferred, DistanceTypes>, Dictionary<TimeSpan, double>>();
            //this.PosturesView = null;
            this.Color = colors[currentColorIndex++];
            if (currentColorIndex == colors.Count()) currentColorIndex = 0;

        }

        public void generateView()
        {
            this.View = new PersonView(this);
        }

        
        public void generatePostureIntervals()
        {
            addVoice();
            if (this.MicroPostures.Count == 0) return;
            //Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (PostureType currentPostureType in PostureTypeContext.db.PostureType.ToList())
            {
                //System.Collections.IEnumerable microPostures = person.microPostures.Where(microPosture => microPosture.postureType.name == postureType.name);

                PostureIntervalGroup postureIntervalGroup = new PostureIntervalGroup(currentPostureType);
                MicroPosture initialMicroPosture = null, lastMicroPosture = null;
                TimeSpan threshold = TimeSpan.FromMilliseconds(Convert.ToDouble(Properties.Resources.PostureDurationDetectionThreshold));
                //bool moreThanOneInterval = false;
                //bool intervalOpen = false;

                foreach (MicroPosture microPosture in this.MicroPostures)
                {
                    if (microPosture.PostureType.Name != currentPostureType.Name) continue;
                    if (microPosture.GestureType == GestureType.Continuous) continue;

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

        public void addVoice()
        {
            PostureIntervalGroup pigVozAux = new PostureIntervalGroup(new PostureType("Voz Real", ""));

            pigVoz.Intervals.OrderBy(v => v.StartTime);
            int i = 0;
            TimeSpan initial = new TimeSpan();
            TimeSpan final = new TimeSpan();
            TimeSpan threshold = TimeSpan.FromMilliseconds(Convert.ToDouble(Properties.Resources.PostureDurationDetectionThreshold));
            bool intervalEnded = true;
            for(i = 0; i < pigVoz.Intervals.Count; i++)
            {
                Interval thisInterval = pigVoz.Intervals.ElementAt(i);
                if (i == 0 || intervalEnded == true) //Primero o aun no hay intervalo vigente
                {
                    initial = thisInterval.StartTime;
                    intervalEnded = false;
                }
                
                if(i+1 == pigVoz.Intervals.Count) //ultimo
                {
                    final = thisInterval.EndTime;
                }
                else //no es el ultimo
                {
                    Interval nextInterval = pigVoz.Intervals.ElementAt(i + 1);
                    if(nextInterval.StartTime.Subtract(thisInterval.EndTime) >= threshold)
                    {
                        final = thisInterval.EndTime;
                        intervalEnded = true;
                    }
                }

                if (intervalEnded == true)
                {
                    pigVozAux.addByStartAndEnd(initial, final);
                }
            }




            if (pigVozAux.Intervals.Count > 0)
            {
                this.PostureIntervalGroups.Add(pigVozAux);
            }
            this.PostureIntervalGroups.Add(this.pigVoz);
        }

        override
        public string ToString()
        {
            return Name;
        }

        public void generateDistanceSum()
        {
            foreach (Distance dist in this.Distances)
            {
                Tuple<DistanceInferred, DistanceTypes> tupleTMP = new Tuple<DistanceInferred, DistanceTypes>(dist.inferredType, dist.DistanceType);
                if (this.DistancesSum.ContainsKey(tupleTMP))
                {
                    this.DistancesSum[tupleTMP] += dist.distance;
                }
                else
                {
                    this.DistancesSum[tupleTMP] = dist.distance;
                }
            }
            generateDistanceEntropy();
        }
        private void generateDistanceEntropy()
        {
            foreach (Distance dist in this.Distances)
            {
                Tuple<DistanceInferred, DistanceTypes> tupleTMP = new Tuple<DistanceInferred, DistanceTypes>(dist.inferredType, dist.DistanceType);
                if (this.DistancesSum.ContainsKey(tupleTMP))
                {
                    double probabilityDistance = 0;
                    if(this.DistancesSum[tupleTMP] != 0)
                        probabilityDistance = dist.distance / this.DistancesSum[tupleTMP];
                    if (this.DistancesEntropy.ContainsKey(tupleTMP))
                    {
                        if(probabilityDistance != 0)
                            this.DistancesEntropy[tupleTMP] += probabilityDistance*Math.Log10(probabilityDistance);
                    }
                    else
                    {
                        if (probabilityDistance != 0)
                            this.DistancesEntropy[tupleTMP] = probabilityDistance * Math.Log10(probabilityDistance);
                        else
                            this.DistancesEntropy[tupleTMP] = 0;
                    }
                    
                }
            }

            List<Tuple<DistanceInferred, DistanceTypes>> distancesEntropyKeys = new List<Tuple<DistanceInferred, DistanceTypes>>(this.DistancesEntropy.Keys);
            foreach (Tuple<DistanceInferred, DistanceTypes> tupleKey in distancesEntropyKeys)
            {
                this.DistancesEntropy[tupleKey] = (this.DistancesEntropy[tupleKey] * - 1) / Math.Log10(25);
                Console.WriteLine("Entropia Distancia Total persona: " + this.Name + " con tipos: " + tupleKey.Item1.ToString()+" y "+tupleKey.Item2.ToString()+" es: "+this.DistancesEntropy[tupleKey].ToString()+" y Suma: "+this.DistancesSum[tupleKey].ToString());
            }
        }


        public void generateIntervalDistancesSum()
        {
            foreach (TimeSpan intervalTime in this.IntervalDistances.Keys)
            {
                foreach (Distance dist in this.IntervalDistances[intervalTime])
                {
                    Tuple<DistanceInferred, DistanceTypes> tupleTMP = new Tuple<DistanceInferred, DistanceTypes>(dist.inferredType, dist.DistanceType);
                    if (this.IntervalDistancesSum.ContainsKey(tupleTMP))
                    {
                        if (this.IntervalDistancesSum[tupleTMP].ContainsKey(intervalTime))
                        {
                            this.IntervalDistancesSum[tupleTMP][intervalTime] += dist.distance;
                        }
                        else
                        {
                            this.IntervalDistancesSum[tupleTMP][intervalTime] = dist.distance;
                        }
                    }
                    else
                    {
                        this.IntervalDistancesSum[tupleTMP] = new Dictionary<TimeSpan, double>();
                        this.IntervalDistancesSum[tupleTMP][intervalTime] = dist.distance;
                    }
                }
            }
            generateIntervalDistancesEntropy();
        }

        private void generateIntervalDistancesEntropy()
        {
            foreach (TimeSpan intervalTime in this.IntervalDistances.Keys)
            {
                foreach (Distance dist in this.IntervalDistances[intervalTime])
                {
                    Tuple<DistanceInferred, DistanceTypes> tupleTMP = new Tuple<DistanceInferred, DistanceTypes>(dist.inferredType, dist.DistanceType);
                    if (this.IntervalDistancesSum.ContainsKey(tupleTMP))
                    {
                        if (this.IntervalDistancesSum[tupleTMP].ContainsKey(intervalTime))
                        {
                            double probabilityDistance = 0;
                            if(this.IntervalDistancesSum[tupleTMP][intervalTime] != 0)
                                probabilityDistance = dist.distance / this.IntervalDistancesSum[tupleTMP][intervalTime];
                            if (this.IntervalDistancesEntropy.ContainsKey(tupleTMP))
                            {
                                if (this.IntervalDistancesEntropy[tupleTMP].ContainsKey(intervalTime))
                                {
                                    if(probabilityDistance != 0)
                                        this.IntervalDistancesEntropy[tupleTMP][intervalTime] += probabilityDistance * Math.Log10(probabilityDistance);
                                }
                                else
                                {
                                    if (probabilityDistance != 0)
                                        this.IntervalDistancesEntropy[tupleTMP][intervalTime] = probabilityDistance * Math.Log10(probabilityDistance);
                                    else
                                        this.IntervalDistancesEntropy[tupleTMP][intervalTime] = 0;
                                }
                            }
                            else
                            {
                                this.IntervalDistancesEntropy[tupleTMP] = new Dictionary<TimeSpan, double>();
                                if (probabilityDistance != 0)
                                    this.IntervalDistancesEntropy[tupleTMP][intervalTime] = probabilityDistance * Math.Log10(probabilityDistance);
                                else
                                    this.IntervalDistancesEntropy[tupleTMP][intervalTime] = 0;
                            }
                        }
                    }
                }
            }
            List<Tuple<DistanceInferred, DistanceTypes>> IntervalDistancesEntropyKeys = new List<Tuple<DistanceInferred, DistanceTypes>>(this.IntervalDistancesEntropy.Keys);
            foreach (Tuple<DistanceInferred, DistanceTypes> tupleKey in IntervalDistancesEntropyKeys)
            {
                List<TimeSpan> timeKeysCol = new List<TimeSpan>(this.IntervalDistancesEntropy[tupleKey].Keys);
                foreach(TimeSpan timeKey in timeKeysCol)
                {
                    this.IntervalDistancesEntropy[tupleKey][timeKey] = (this.IntervalDistancesEntropy[tupleKey][timeKey] * - 1) / Math.Log10(25);
                    Console.WriteLine("Entropia Intervalo persona: " + this.Name +" en tiempo: "+timeKey.ToString()+" con tipos: "+ tupleKey.Item1.ToString()+" y "+tupleKey.Item2.ToString()+" es: "+this.IntervalDistancesEntropy[tupleKey][timeKey].ToString()+" y Suma: "+this.IntervalDistancesSum[tupleKey][timeKey].ToString());
                }
            }
        }

    }
}
