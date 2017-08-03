using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.EEG;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels
{
    public class TrainerEntryPoint : API.ProcessingModule.ProcessingModule, IEeg
    {
        private readonly EEGReceiver _logic;

        public override Task FunctionAfterStop()
        {
            return new Task(_logic.Train);
        }

        public TrainerEntryPoint()
        {
            Windows = new List<Tuple<string, Window>>();
            IsActiveBeforeRecording = false;
            Name = Properties.EEGEmotion2Channels.ModuleName;
            IsEnabled = true;
            var configWindow = new Tuple<string, Window>(Properties.EEGEmotion2Channels.ConfigWindowTitle,
                new EEGEmo2ChannelWindow());
            Windows.Add(configWindow);
            
            _logic = new EEGReceiver(ClassifierType.Trainer);
        }

        public EventHandler<EegFrameArrivedEventArgs> EegListener()
        {
            return _logic.DataReceiver;
        }
    }
}
