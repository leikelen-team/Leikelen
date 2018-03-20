using System;
using System.Collections.Generic;

namespace cl.uv.leikelen.src.Module
{
    public class ModuleLoader
    {
        public List<IModule> Modules;
        private static ModuleLoader _instance;

        public static ModuleLoader Instance
        {
            get
            {
                if (_instance == null) _instance = new ModuleLoader();
                return _instance;
            }
        }

        private ModuleLoader()
        {
            Modules = new List<IModule>();
            addModules();
        }

        public void addModules()
        {
            var voiceListener = new Voice.VoiceEntryPoint();
            var distance = new Distance.DistanceEntryPoint();
            var addPerson = new AddPerson.AddPersonEntryPoint();
            Modules.Add(voiceListener);
            Modules.Add(distance);
            Modules.Add(addPerson);
        }
    }
}
