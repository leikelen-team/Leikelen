using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.models;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.views
{
    public class ChartView : INotifyPropertyChanged
    {
        private String myClass;
        public String Class
        {
            get { return myClass; }
            set {
                myClass = value;
                RaisePropertyChangeEvent("Class");
            }
        }

        private double totalMinutesDuration;
        public double TotalMinutesDuration
        {
            get { return totalMinutesDuration; }
            set
            {
                totalMinutesDuration = value;
                RaisePropertyChangeEvent("TotalMinutesDuration");
            }
        }

        //private double fund;

        //public double Fund
        //{
        //    get { return fund; }
        //    set {
        //        fund = value;
        //        RaisePropertyChangeEvent("Fund");
        //    }
        //}

        //private double total;

        //public double Total
        //{
        //    get { return total; }
        //    set {
        //        total = value;
        //        RaisePropertyChangeEvent("Total");
        //    }
        //}

        //private double benchmark;

        //public double Benchmark
        //{
        //    get { return benchmark; }
        //    set {
        //        benchmark = value;
        //        RaisePropertyChangeEvent("Benchmark");
        //    }
        //}

        public static List<ChartView> GenerateFromPerson(Person person)
        {
            List<ChartView> chartClasses = new List<ChartView>();
            foreach(var pig in person.PostureIntervalGroups)
            {
                TimeSpan duration = pig.CalculateTotalDuration();
                string className = pig.PostureType.Name;
                chartClasses.Add(new ChartView() { Class = className, TotalMinutesDuration = duration.TotalMinutes });
            }
            return chartClasses;
        }

        //public static List<ChartView> ConstructTestData()
        //{
        //    List<ChartView> assetClasses = new List<ChartView>();

        //    assetClasses.Add(new ChartView() { Class = "PostureTypeName1", TotalDuration = TimeSpan.FromMinutes(13) });
        //    assetClasses.Add(new ChartView() { Class = "PostureTypeName2", TotalDuration = TimeSpan.FromMinutes(13) });

        //    return assetClasses;
        //}

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChangeEvent(String propertyName)
        {
            if (PropertyChanged!=null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            
        }

        #endregion
    }
}
