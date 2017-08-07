﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.InputModule;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.InputModule
{
    public class InputLoader
    {
        public List<API.InputModule.InputModule> SceneInputModules { get; private set; }
        public Dictionary<Person, List<API.InputModule.InputModule>> PersonInputModules { get; private set; }
        public IVideo VideoHandler { get; private set; }
        private static InputLoader _instance;

        public static InputLoader Instance
        {
            get
            {
                if (_instance == null) _instance = new InputLoader();
                return _instance;
            }
        }

        public void FillPersonInputModules(Person person)
        {
            var personInputModules = new List<API.InputModule.InputModule>();
            personInputModules.Add(new OpenBCI.OpenBCISensor());

            PersonInputModules.Add(person, personInputModules);

        }

        private InputLoader()
        {
            SceneInputModules = new List<API.InputModule.InputModule>();
            PersonInputModules = new Dictionary<Person, List<API.InputModule.InputModule>>();

            FillSceneInputModules();
        }

        private void FillSceneInputModules()
        {
            //TODO: esto es temporal
            SceneInputModules.Add(new OpenBCI.OpenBCISensor());
        }

        
    }
}
