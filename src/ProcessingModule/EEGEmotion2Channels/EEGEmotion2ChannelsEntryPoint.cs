using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API.FrameProvider.EEG;

namespace cl.uv.leikelen.src.ProcessingModule.EEGEmotion2Channels
{
    public class EEGEmotion2ChannelsEntryPoint : IEEG
    {
        private List<double[]> ChannelValues;

        public EEGEmotion2ChannelsEntryPoint()
        {

        }

        public EventHandler<EEGFrameArrivedEventArgs> EEGListener()
        {
            return EEGReceiver;
        }

        private void EEGReceiver(object sender, EEGFrameArrivedEventArgs e)
        {
            foreach(var channel in e.Channels)
            {
                if(channel.PositionSystem=="10/20")
                {
                    double[] values = new double[2];
                    if(channel.Position == "F3")
                    {
                        values[0] = channel.Value;
                    }
                    if (channel.Position == "C4")
                    {
                        values[1] = channel.Value;
                    }
                    ChannelValues.Add(values);
                }
            }
        }
    }
}
