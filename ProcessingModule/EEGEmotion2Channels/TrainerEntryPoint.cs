using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.ProcessingModule;
using cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels.View;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels
{
    public class TrainerEntryPoint : API.ProcessingModule.ProcessingModule
    {
        public static Dictionary<TagType, List<Scene>> ScenesAndTags;
        public TrainerEntryPoint()
        {
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = true;
            Name = Properties.EEGEmotion2Channels.ClassificationModuleName;
            var configWindow = new Tuple<string, WindowBuilder>(Properties.EEGEmotion2Channels.ConfigWindowTitle,
                new WindowBuilder(new ClassifierWindow()));
            Windows.Add(configWindow);
            Plurality = ProcessingPlurality.General;
        }

        public override Task FunctionAfterStop()
        {
            return null;
        }
    }
}
