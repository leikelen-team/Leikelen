using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using cl.uv.leikelen.API.Helper;

namespace cl.uv.leikelen.API.Module.Processing
{
    public abstract class ProcessingModule : AbstractModule
    {
        public bool IsActiveBeforeRecording { get; protected set; } = false;

        public abstract Task FunctionAfterStop();
    }
}
