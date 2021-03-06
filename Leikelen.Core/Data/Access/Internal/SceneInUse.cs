﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Data.Access.Internal
{
    public class SceneInUse
    {
        private static SceneInUse _instance;
        public Scene Scene { get; private set; }
        public bool IsRecording = false;
        public DateTime? PlayStartTime { get; set; }
        public DateTime? PlayPausedTime { get; set; }

        public static SceneInUse Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new SceneInUse();
                return _instance;
            }
        }

        private SceneInUse()
        {

        }

        public void Set(Scene scene)
        {
            Scene = scene;
        }
    }
}
