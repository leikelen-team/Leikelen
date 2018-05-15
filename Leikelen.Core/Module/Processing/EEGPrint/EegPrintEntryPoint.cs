using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.API.Module.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Module.Processing.EEGPrint
{
    public class EegPrintEntryPoint : ProcessingModule, IEegProcessingModule
    {
        public EegPrintEntryPoint()
        {
            Name = "print eeg";
        }

        public override Action FunctionAfterStop()
        {
            return null;
        }

        public EventHandler<EegFrameArrivedEventArgs> EegListener()
        {
            return this.myListener;
        }

        public void myListener(object sender, EegFrameArrivedEventArgs e)
        {
            Console.WriteLine(e);
            //Console.WriteLine(e.Quality);

        }
    }
}
