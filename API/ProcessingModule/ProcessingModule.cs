using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cl.uv.leikelen.Helper;

namespace cl.uv.leikelen.API.ProcessingModule
{
    public abstract class ProcessingModule
    {
        public string Name { get; protected set; }
        public List<Tuple<string, WindowBuilder>> Windows { get; protected set; }
        public bool IsActiveBeforeRecording { get; protected set; }
        public bool IsEnabled { get; set; }

        public abstract Task FunctionAfterStop();
    }
}
