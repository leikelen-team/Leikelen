﻿using Microsoft.Samples.Kinect.VisualizadorMultimodal.analytics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.pojos
{
    public class Person : INotifyPropertyChanged
    {
        public enum Gender { Masculino, Femenino};

        public int bodyIndex { get; set; }
        public string name { get; set; }
        public Gender gender { get; set; }
        public int age { get; set; }

        public Dictionary<TimeSpan, Posture> postures { get; private set; }
        //public List<Interval> intervals { get; private set; }

        public Person(int bodyIndex, string name, Gender gender, int age)
        {
            this.bodyIndex = bodyIndex;
            this.name = name;
            this.gender = gender;
            this.age = age;
            this.postures = new Dictionary<TimeSpan, Posture>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        

        public void showPersonInfo()
        {
            IReadOnlyDictionary<Posture, float> posturesAvg = this.calculatePosturesAverage();

            Console.WriteLine("Nombre: "+name);
            Console.WriteLine("Genero: "+gender.ToString("g"));
            Console.WriteLine("Edad: "+age);

            foreach (Posture posture in posturesAvg.Keys)
            {
                Console.WriteLine(posture.name + ": " + posturesAvg[posture].ToString("0.00"));
            }

            ChartForm chartForm = new ChartForm(this);
            chartForm.Show();

        }

        
    }
}