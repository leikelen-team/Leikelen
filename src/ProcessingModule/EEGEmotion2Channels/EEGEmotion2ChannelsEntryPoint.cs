using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using cl.uv.leikelen.API.ProcessingModule;
using cl.uv.leikelen.API.FrameProvider.EEG;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels
{
    public sealed class EEGEmotion2ChannelsEntryPoint : API.ProcessingModule.ProcessingModule, IEeg
    {
        private Classifier.EEGReceiver logic;

        public override Task FunctionAfterStop()
        {
            return null;
        }

        public EEGEmotion2ChannelsEntryPoint()
        {
            Windows = new List<Tuple<string, Window>>();
            IsActiveBeforeRecording = false;
            Name = Properties.EEGEmotion2Channels.Name;
            IsEnabled = true;
            Windows.Add(new Tuple<string, Window>(Properties.EEGEmotion2Channels.ConfigWindowTitle, new EEGEmo2ChannelWindow()));
            

            logic = new Classifier.EEGReceiver();
        }

        public EventHandler<EegFrameArrivedEventArgs> EegListener()
        {
            return logic.DataReceiver;
        }
    }
}
