using cl.uv.leikelen.API.Module.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Module.Input.MindwaveTGC
{
    /// <summary>
    /// Player for the Mindwave sensor
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Input.IPlayer" />
    public class Player : IPlayer
    {
        /// <summary>
        /// Occurs when playback [finished].
        /// </summary>
        public event EventHandler Finished;
        /// <summary>
        /// Occurs when playback's [location changed].
        /// </summary>
        public event EventHandler LocationChanged;

        void IPlayer.ChangeTime(TimeSpan newTime)
        {
            return;
        }

        void IPlayer.Close()
        {
            return;
        }

        TimeSpan? IPlayer.GetLocation()
        {
            return null;
        }

        TimeSpan? IPlayer.GetTotalDuration()
        {
            return null;
        }

        bool IPlayer.IsPlaying()
        {
            return false;
        }

        void IPlayer.Pause()
        {
            return;
        }

        void IPlayer.Play()
        {
            return;
        }

        void IPlayer.Stop()
        {
            return;
        }

        void IPlayer.Unpause()
        {
            return;
        }
    }
}
