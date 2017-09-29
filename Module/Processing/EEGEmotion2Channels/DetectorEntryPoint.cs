using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.Module.Processing;
using cl.uv.leikelen.Module.Processing.EEGEmotion2Channels.View;

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels
{
    public sealed class DetectorEntryPoint : ProcessingModule, IEegProcessingModule
    {
        private readonly EEGReceiver _logic;

        public override Task FunctionAfterStop()
        {
            return null;
        }

        public DetectorEntryPoint()
        {
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            Name = Properties.EEGEmotion2Channels.DetectionModuleName;
            IsEnabled = true;
            var configWindow = new Tuple<string, WindowBuilder>(Properties.EEGEmotion2Channels.ConfigWindowTitle,
               new WindowBuilder(new ConfigurationWindow(Properties.EEGEmotion2Channels.ConfigWindowTitle)));
            Windows.Add(configWindow);
            
            _logic = new EEGReceiver();
        }

        public EventHandler<EegFrameArrivedEventArgs> EegListener()
        {
            return _logic.DataReceiver;
        }
    }
}
