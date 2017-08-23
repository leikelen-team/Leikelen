using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.InputModule;

namespace cl.uv.leikelen.InputModule.Kinect
{
    public class Player : IPlayer
    {
        public void Unpause()
        {
            return;
        }

        public void Play()
        {
            return;
        }

        public void Pause()
        {
            return;
        }

        public void Stop()
        {
            return;
        }

        public void ChangeTime(TimeSpan newTime)
        {
            return;
        }

        public bool IsPlaying()
        {
            return false;
        }

        public void OpenFile(int sceneId)
        {
            return;
        }

        public void Close()
        {
            return;
        }

        public TimeSpan? GetTotalDuration()
        {
            throw new NotImplementedException();
        }

        public TimeSpan? GetLocation()
        {
            return null;
        }

        public event EventHandler Finished;
        public event EventHandler LocationChanged;
    }
}
