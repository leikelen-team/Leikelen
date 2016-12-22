using System;
using System.Collections.Generic;
using System.ComponentModel;
using cl.uv.leikelen.src.model;

namespace cl.uv.leikelen.src.controller
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
