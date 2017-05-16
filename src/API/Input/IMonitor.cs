using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.API.Input
{
    public interface IMonitor
    {
        bool IsRecording();
        InputStatus getStatus();
        Task Open();
        Task Close();
        Task StartRecording();
        Task StopRecording();
        event EventHandler StatusChanged;
    }
}
