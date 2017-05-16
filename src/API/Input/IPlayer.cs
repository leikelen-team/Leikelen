using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.API.Input
{
    public interface IPlayer
    {
        void Unpause();
        void Play();
        void Pause();
        void Stop();
        void ChangeTime(TimeSpan newTime);
        bool IsPlaying();
        void OpenFile(string fullPath);
        void Close();
        TimeSpan? getTotalDuration();
        TimeSpan? getLocation();
        event EventHandler Finished;
        event EventHandler LocationChanged;
    }
}
