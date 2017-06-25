using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.src.API.InputModule
{
    public interface IPlayer
    {
        void Unpause();
        void Play();
        void Pause();
        void Stop();
        void ChangeTime(TimeSpan newTime);
        bool IsPlaying();
        void OpenFile(int SceneId);
        void Close();
        TimeSpan? GetTotalDuration();
        TimeSpan? GetLocation();
        event EventHandler Finished;
        event EventHandler LocationChanged;
    }
}
