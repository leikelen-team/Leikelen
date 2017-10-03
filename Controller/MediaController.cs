﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Module;

namespace cl.uv.leikelen.Controller
{
    public class MediaController
    {
        public void SetFromNone()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Player.Close();
                input.Monitor.Close();
            }
        }

        public async void SetFromSensor()
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Player.Close();
                await input.Monitor.Open();
            }
        }

        public void SetFromFile(int sceneId)
        {
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                input.Monitor.Close();
                input.Player.Play();
            }
        }
    }
}
