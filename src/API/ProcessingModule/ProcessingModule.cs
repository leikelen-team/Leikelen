using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cl.uv.leikelen.API.ProcessingModule
{
    public abstract class ProcessingModule
    {
        public string Name { get; protected set; }
        public List<Tuple<string, Window>> Windows { get; protected set; }
        public bool IsActiveBeforeRecording { get; protected set; }
        public bool IsEnabled { get; set; }

        public abstract Task FunctionAfterStop();
    }
}
