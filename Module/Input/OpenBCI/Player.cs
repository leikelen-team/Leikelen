using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.Input;

namespace cl.uv.leikelen.Module.Input.OpenBCI
{
    public class Player : IPlayer
    {
        public event EventHandler Finished;
        public event EventHandler LocationChanged;

        public void ChangeTime(TimeSpan newTime)
        {
            return;
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
