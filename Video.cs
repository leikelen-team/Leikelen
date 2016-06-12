using Microsoft.Kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal
{
    //class RecFrame
    //{
    //    private ColorFrame colorFrame;
    //    private BodyFrame bodyFrame;
    //    //private TimeSpan time;
        
    //    public RecFrame(ColorFrame colorFrame, BodyFrame bodyFrame)
    //    {
    //        this.colorFrame = colorFrame;
    //        this.bodyFrame = bodyFrame;
    //        //Console.WriteLine("colorFrame: " + colorFrame.RelativeTime.Seconds + ":" +
    //        //    colorFrame.RelativeTime.Minutes);
    //        //Console.WriteLine("bodyFrame: " + bodyFrame.RelativeTime.Seconds + ":" +
    //        //    bodyFrame.RelativeTime.Minutes);
            //Console.WriteLine("colorFrame: {0:dd\\.hh\\:mm\\:ss\\:fff}", colorFrame.RelativeTime);
            //Console.WriteLine("bodyFrame: {0:dd\\.hh\\:mm\\:ss\\:fff}", bodyFrame.RelativeTime);
    //        //if (colorFrame.RelativeTime.Equals(bodyFrame.RelativeTime))
    //        //{
    //        //    Console.Error.WriteLine("Frames con distinto relative time");
    //        //    throw new Exception("Frames con distinto RelativeTime");
    //        //}
    //        //this.time = time;
    //    }
    //    public ColorFrame ColorFrame { get; private set; }
    //    public BodyFrame BodyFrame
    //    {
    //        get {
    //            return this.bodyFrame;
    //        }

    //        private set
    //        {
    //            this.bodyFrame = value;
    //        }
    //    }
    //    public TimeSpan Time { get { return colorFrame.RelativeTime; } }
    //}
    class Video
    {
        private List<ColorFrame> colorFrames;
        private List<BodyFrame> bodyFrames;

        public Video()
        {
            colorFrames = new List<ColorFrame>();
            bodyFrames = new List<BodyFrame>();
        }

        //public void AddFrames(ColorFrame colorFrame, BodyFrame bodyFrame)
        //{
        //    colorFrames.Add(colorFrame);
        //    bodyFrames.Add(bodyFrame);
        //}
        
        public void AddColorFrame(ColorFrame colorFrame)
        {
            colorFrames.Add(colorFrame);
            long size = 0;
            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, colorFrame);
                size = s.Length;
                Console.WriteLine("ColorFrame Size: " + size);
            }
            //Console.WriteLine("colorFrame: {0:dd\\.hh\\:mm\\:ss\\:fff}", colorFrame.RelativeTime);
        }
        public void AddBodyFrame(BodyFrame bodyFrame)
        {
            bodyFrames.Add(bodyFrame);
            //Console.WriteLine("bodyFrame: {0:dd\\.hh\\:mm\\:ss\\:fff}\n", bodyFrame.RelativeTime);
            
            
            

            int colorMillis = colorFrames.Last().RelativeTime.Milliseconds;
            int bodyMillis = bodyFrame.RelativeTime.Milliseconds;
            if (colorMillis < bodyMillis)
            {
                Console.WriteLine("!!!!!!!!!!!!!! ColorFrame Primero !!!!!!!!!!!!!!");
            }
            else
            {
                Console.WriteLine("bodyFrame primero");
            }
        }


    }
}
