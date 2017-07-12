using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cl.uv.leikelen.src.API.ProcessingModule
{
    public interface IProcessingModule
    {
        string GetName();
        List<Tuple<string, Window>> GetWindows();
        bool IsActiveBeforeRecording();
        Action FunctionAfterStop();
    }
}
