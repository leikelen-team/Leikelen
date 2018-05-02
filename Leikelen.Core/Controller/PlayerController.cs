using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.Module;

namespace cl.uv.leikelen.Controller
{
    /// <summary>
    /// Controller for play a recorded scene
    /// </summary>
    public class PlayerController
    {
        /// <summary>
        /// Plays the scene in use.
        /// </summary>
        public void Play()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Player.Play();
                input.Player.Finished += Player_Finished;
            }
            foreach (var person in InputLoader.Instance.PersonInputModules.Keys)
            {
                foreach (var personInput in InputLoader.Instance.PersonInputModules[person])
                {
                    personInput.Player.Play();
                    personInput.Player.Finished += Player_Finished;
                }
            }
            SceneInUse.Instance.PlayStartTime = DateTime.Now;
        }

        /// <summary>
        /// Pauses the scene in use.
        /// </summary>
        public void Pause()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Player.Pause();
            }
            foreach (var person in InputLoader.Instance.PersonInputModules.Keys)
            {
                foreach (var personInput in InputLoader.Instance.PersonInputModules[person])
                {
                    personInput.Player.Pause();
                }
            }
            SceneInUse.Instance.PlayPausedTime = DateTime.Now;
        }

        /// <summary>
        /// Resume the scene in use.
        /// </summary>
        public void UnPause()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Player.Unpause();
            }
            foreach (var person in InputLoader.Instance.PersonInputModules.Keys)
            {
                foreach (var personInput in InputLoader.Instance.PersonInputModules[person])
                {
                    personInput.Player.Unpause();
                }
            }
            if (SceneInUse.Instance.PlayPausedTime.HasValue)
                SceneInUse.Instance.PlayStartTime = SceneInUse.Instance.PlayStartTime.Value
                    .Add(
                    DateTime.Now
                    .Subtract(SceneInUse.Instance.PlayPausedTime.Value));
        }

        /// <summary>
        /// Stops the scene in use.
        /// </summary>
        public void Stop()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Player.Stop();
            }
            foreach (var person in InputLoader.Instance.PersonInputModules.Keys)
            {
                foreach (var personInput in InputLoader.Instance.PersonInputModules[person])
                {
                    personInput.Player.Stop();
                }
            }
            SceneInUse.Instance.PlayStartTime = null;
        }

        /// <summary>
        /// Closes the scene in use and all sensor players.
        /// </summary>
        public void Close()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Player.Close();
            }
            foreach (var person in InputLoader.Instance.PersonInputModules.Keys)
            {
                foreach (var personInput in InputLoader.Instance.PersonInputModules[person])
                {
                    personInput.Player.Close();
                }
            }
            SceneInUse.Instance.PlayStartTime = null;
        }

        /// <summary>
        /// Moves the player controllers of the scene in use to the specified time.
        /// </summary>
        /// <param name="time">The time to move the scene in use.</param>
        public void MoveTo(TimeSpan time)
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Player.ChangeTime(time);
            }
            foreach (var person in InputLoader.Instance.PersonInputModules.Keys)
            {
                foreach (var personInput in InputLoader.Instance.PersonInputModules[person])
                {
                    personInput.Player.ChangeTime(time);
                }
            }
        }

        private void Player_Finished(object sender, EventArgs e)
        {
            Finished?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when the playing is [finished].
        /// </summary>
        public event EventHandler Finished;
    }
}
