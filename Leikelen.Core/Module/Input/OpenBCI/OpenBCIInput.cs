﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cl.uv.leikelen.API.Module.Input;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Module.Input.OpenBCI
{
    public sealed class OpenBCIInput : InputModule
    {
        public OpenBCIInput(Person person)
        {
            Plurality = InputPlurality.Person;
            var myMonitor = new Monitor(person);
            Tabs.Add(myMonitor.GraphTab);
            Monitor = myMonitor as IMonitor;
            Name = Properties.OpenBCI.SensorName;
            Player = new Player();
            Windows = new List<Tuple<string, WindowBuilder>>();
            Windows.Add(new Tuple<string, WindowBuilder>(Properties.OpenBCI.ConfigWindowTitle, 
                new WindowBuilder(new View.OpenBCIWindow(Monitor))));

            
        }
    }
}
