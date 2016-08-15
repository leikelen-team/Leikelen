﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal.models
{
    public class Scene
    {
        [NotMapped]
        private static Scene instance;

        public int SceneId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime startDate { get; private set; } // start date when begin to record
        public TimeSpan duration { get; private set; }

        public List<Person> Persons { get; private set; }

        [NotMapped]
        public static Scene Instance
        {
            get
            {
                return instance;
            }
        }

        private Scene(string name, DateTime startDate, TimeSpan duration)
        {
            this.name = name;
            this.startDate = startDate;
            this.duration = duration;
        }

        public static Scene Create(string name, DateTime startDate, TimeSpan duration)
        {
            instance = new Scene(name, startDate, duration);
            instance.Persons = new List<Person>();
            //for (int i = 1; i <= 6; i++)
            //{
            //    instance.Persons.Add(new Person(i-1, "Sujeto "+i, Person.GenderEnum.Masculino, 0));
            //}
            return instance;
        }
        
    }
}
