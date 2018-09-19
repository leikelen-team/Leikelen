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

        void API.Module.Input.IPlayer.ChangeTime(TimeSpan newTime)
        {
            return;
        }

        void API.Module.Input.IPlayer.Close()
        {
            return;
        }

        TimeSpan? API.Module.Input.IPlayer.GetLocation()
        {
            return null;
        }

        TimeSpan? API.Module.Input.IPlayer.GetTotalDuration()
        {
            return null;
        }

        bool API.Module.Input.IPlayer.IsPlaying()
        {
            return false;
        }

        void API.Module.Input.IPlayer.Pause()
        {
            return;
        }

        void API.Module.Input.IPlayer.Play()
        {
            return;
        }

        void API.Module.Input.IPlayer.Stop()
        {
            return;
        }

        void API.Module.Input.IPlayer.Unpause()
        {
            return;
        }
    }
}
