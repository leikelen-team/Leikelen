using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.InputModule
{
    public interface IMonitor
    {
        event EventHandler StatusChanged;
        bool IsRecording();
        InputStatus GetStatus();
        Task Open();
        Task Close();
        Task StartRecording();
        Task StopRecording();
        Task OpenPort(string portName);
    }
}
