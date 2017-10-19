﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.API.Module
{
    public abstract class AbstractModule
    {
        public string Name { get; protected set; }
        public List<Tuple<string, WindowBuilder>> Windows { get; protected set; }
        public bool IsEnabled { get; private set; } = false;
        public List<ITab> Tabs { get; protected set; } = new List<ITab>();

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }
    }
}
