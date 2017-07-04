using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.InputModule;

namespace cl.uv.leikelen.src.Controller
{
    public class MediaController
    {
        public void SetFromNone()
        {
            foreach (var input in InputLoader.Instance.InputModules)
            {
                input.Player.Close();
                input.Monitor.Close();
            }
        }

        public void SetFromSensor()
        {
            foreach (var input in InputLoader.Instance.InputModules)
            {
                input.Player.Close();
                input.Monitor.Open();
            }
        }

        public void SetFromFile(int SceneId)
        {
            foreach (var input in InputLoader.Instance.InputModules)
            {
                input.Monitor.Close();
                input.Player.OpenFile(SceneId);
            }
        }
    }
}
