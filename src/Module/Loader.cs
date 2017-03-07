using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.kinectmedia;
using Microsoft.Kinect;

namespace cl.uv.leikelen.src.Module
{
    public static class Loader
    {
        public static List<IModule> Modules = new List<IModule>();

        public static void addModules()
        {
            var voiceListener = new Voice.Voice();
            var distance = new Distance.Distance();
            Modules.Add(voiceListener);
            Modules.Add(distance);
        }
    }
}
