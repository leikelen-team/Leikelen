using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API.InputModule;

namespace cl.uv.leikelen.src.InputModule.OpenBCI
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
            throw new NotImplementedException();
        }

        public TimeSpan? GetLocation()
        {
            throw new NotImplementedException();
        }

        public TimeSpan? GetTotalDuration()
        {
            throw new NotImplementedException();
        }

        public bool IsPlaying()
        {
            throw new NotImplementedException();
        }

        public void OpenFile(int SceneId)
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Play()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Unpause()
        {
            throw new NotImplementedException();
        }
    }
}
