using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.InputModule;

namespace cl.uv.leikelen.InputModule.OpenBCI
{
    public class Player : IPlayer
    {
        public event EventHandler Finished;
        public event EventHandler LocationChanged;

        public void ChangeTime(TimeSpan newTime)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            return;
        }

        public TimeSpan? GetLocation()
        {
            return null;
        }

        public TimeSpan? GetTotalDuration()
        {
            return null;
        }

        public bool IsPlaying()
        {
            return false;
        }

        public void OpenFile(int sceneId)
        {
            return;
        }

        public void Pause()
        {
            return;
        }

        public void Play()
        {
            return;
        }

        public void Stop()
        {
            return;
        }

        public void Unpause()
        {
            return;
        }
    }
}
