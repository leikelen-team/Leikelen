using System;
using System.Collections.Generic;

namespace cl.uv.leikelen.src.Module
{
    public static class TMPLoader
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
