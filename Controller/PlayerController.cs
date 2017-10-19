using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.Module;

namespace cl.uv.leikelen.Controller
{
    public class PlayerController
    {
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

        public event EventHandler Finished;
    }
}
