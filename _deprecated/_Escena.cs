using Microsoft.Kinect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Threading;
using System.ComponentModel;
using Microsoft.Samples.Kinect.VisualizadorMultimodal.utils;

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
    //class Frame
    //{
        
    //}

    class _Escena
    {
        private string id; // unique id, based on the startDate
        private string name;
        private string description;
        private DateTime startDate; // start date when begin to record
        private TimeSpan duration;

        private string mainPath;
        private List<_Frame> frames;
        private bool isRecordedOrImported;
        private bool isRecording;
        private bool isPlaying;

        private _Frame previousFrame;

        private BackgroundWorker playerWorker = new BackgroundWorker();


        public _Escena()
        {
            
            isRecordedOrImported = false;
            isRecording = false;
            previousFrame = null;

            playerWorker.WorkerReportsProgress = false;
            playerWorker.WorkerSupportsCancellation = true;
            playerWorker.DoWork += new DoWorkEventHandler(this.bw_DoWork_StartPlaying);
            //playerWorker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            //playerWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        //public Escena(string mainPath, string name)
        //{
        //    this.mainPath = mainPath;
        //    this.name = name;
        //}
        public bool IsPlaying
        {
            get { return isPlaying; }
            private set { isPlaying = value; }
        }
        public bool IsRecordedOrImported
        {
            get { return isRecordedOrImported; }
            private set { isRecordedOrImported = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public DateTime StartDate
        {
            get { return startDate; }
            private set { startDate = value; }
        }
        
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string MainPath
        {
            get { return mainPath; }
            set { mainPath = value; }
        }

        public bool IsRecording
        {
            get { return isRecording; }
            private set { isRecording = value; }
        }
        
        private string GetColorFramePath()
        {
            //return Properties.Paths.recordScenes + this.id + "/frames/color/";
            return null;
        }

        private string GetBodyFramePath()
        {
            //return Properties.Paths.recordScenes + this.id + "/frames/body/";
            return null;
        }

        public void StartRecording()
        {
            if (!isRecording)
            {
                this.frames = new List<_Frame>();
                this.startDate = DateTime.Now;
                this.id = this.startDate.GetStringTime("dd-MM-yy__HH-mm-ss") + " scene";
                Directory.CreateDirectory(this.GetColorFramePath());
                Directory.CreateDirectory(this.GetBodyFramePath());


                //// Configure save file dialog box
                //Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                //dialog.FileName = "Escena"; // Default file name
                //dialog.DefaultExt = ".vms"; // Default file extension
                //dialog.Filter = "Escena de Visualizador Multimodal (.vms)|*.vms"; // Filter files by extension

                //// Show save file dialog box
                //Nullable<bool> result = dialog.ShowDialog();

                //// Process save file dialog box results
                //if (result == true)
                //{
                //    // Save document
                //    this.Name = dialog.FileName;
                //    System.IO.FileStream fs = (System.IO.FileStream)dialog.OpenFile();
                //    //dialog.OpenFile.
                //}

                isRecording = true;
                isRecordedOrImported = false;
            }
        }
        public void StopRecording()
        {
            if (isRecording)
            {
                duration = DateTime.Now.Subtract(this.StartDate);
                isRecording = false;
                isRecordedOrImported = true;
            }
        }

        public void StartPlaying()
        {
            //playerWorker.RunWorkerAsync();


            Console.WriteLine("Playing...");
            this.isPlaying = true;

            _Frame bodyFrame = frames.First(x => x.Type == _Frame.FrameType.Body);
            _Frame colorFrame = frames.First(x => x.Type == _Frame.FrameType.Color);
            _VisualizerXamlView.Instance().SetBodyFrame(bodyFrame.Bitmap);
            _VisualizerXamlView.Instance().SetColorFrame(colorFrame.Bitmap);

            //VisualizerXamlView view = VisualizerXamlView.Instance();
            //foreach (Frame frame in frames)
            //{
            //    if (frame.Type == Frame.FrameType.Body)
            //    {
            //        view.SetBodyFrame(frame.Bitmap);
            //        //Utils.RunActionAfter(view.SetBodyFrame, frame.Bitmap, frame.timeWithPrevFrame);
            //    }
            //    else if (frame.Type == Frame.FrameType.Color)
            //    {
            //        view.SetColorFrame(frame.Bitmap);
            //        //Utils.RunActionAfter(view.SetBodyFrame, frame.Bitmap, frame.timeWithPrevFrame);
            //    }
            //    int millis = (int)frame.timeWithPrevFrame.TotalMilliseconds;
            //    Console.WriteLine("waiting " + millis);
            //    Thread.Sleep(millis);
            //}

        }
        public void StopPlaying()
        {
            //playerWorker.CancelAsync();
            this.isPlaying = false;
        }

        private void bw_DoWork_StartPlaying(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Playing...");
            this.isPlaying = true;

            //Frame bodyFrame = frames.First(x => x.Type == Frame.FrameType.Body);
            //Frame colorFrame = frames.First(x => x.Type == Frame.FrameType.Color);
            //VisualizerXamlView.Instance().SetBodyFrame(bodyFrame.Bitmap);
            //VisualizerXamlView.Instance().SetColorFrame(colorFrame.Bitmap);

            _VisualizerXamlView view = _VisualizerXamlView.Instance();
            foreach (_Frame frame in frames)
            {
                if (frame.Type == _Frame.FrameType.Body)
                {
                    view.SetBodyFrame(frame.Bitmap);
                    //Utils.RunActionAfter(view.SetBodyFrame, frame.Bitmap, frame.timeWithPrevFrame);
                }else if (frame.Type == _Frame.FrameType.Color)
                {
                    view.SetColorFrame(frame.Bitmap);
                    //Utils.RunActionAfter(view.SetBodyFrame, frame.Bitmap, frame.timeWithPrevFrame);
                }
                Thread.Sleep(frame.timeWithPrevFrame);
            }

            
        }
        

        



        public void AddColorFrame(BitmapSource bitmap)
        {
            if (this.isRecording)
            {
                _Frame frame;
                TimeSpan timeWithPreviousFrame;

                DateTime now = DateTime.Now;
                TimeSpan timeSpan = now.Subtract(this.StartDate);

                string path = this.GetColorFramePath() + now.GetStringTime() + ".jpg";
                if (this.previousFrame == null)
                    timeWithPreviousFrame = timeSpan;
                else
                    timeWithPreviousFrame = timeSpan.Subtract(this.previousFrame.Time);
                
                frame = new _Frame(bitmap.Clone(), path, timeSpan, timeWithPreviousFrame, _Frame.FrameType.Color);
                this.previousFrame = frame;
                frames.Add(frame);

                //bitmap.ToImageFile(path);
            }
        }

        internal void AddBodyFrame(DrawingImage drawingImage)
        {
            if (this.isRecording)
            {
                //DateTime now = DateTime.Now;
                //TimeSpan timeSpan = this.StartDate.Subtract(now);
                //string path = this.GetBodyFramePath() + now.GetStringTime() + ".png";
                //RenderTargetBitmap bitmap = drawingImage.ToBitmap();
                //frames.Add(new Frame(bitmap.Clone(), path, timeSpan, Frame.FrameType.Body));
                ////bitmap.ToImageFile(path);

                _Frame frame;
                TimeSpan timeWithPreviousFrame;

                DateTime now = DateTime.Now;
                TimeSpan timeSpan = now.Subtract(this.StartDate);

                string path = this.GetColorFramePath() + now.GetStringTime() + ".png";
                if (this.previousFrame == null)
                    timeWithPreviousFrame = timeSpan;
                else
                    timeWithPreviousFrame = timeSpan.Subtract(this.previousFrame.Time);

                RenderTargetBitmap bitmap = drawingImage.ToBitmap();
                frame = new _Frame(bitmap.Clone(), path, timeSpan, timeWithPreviousFrame, _Frame.FrameType.Body);
                this.previousFrame = frame;
                frames.Add(frame);

                //bitmap.ToImageFile(path);
            }
        }



        //public void AddFrames(ColorFrame colorFrame, BodyFrame bodyFrame)
        //{
        //    colorFrames.Add(colorFrame);
        //    bodyFrames.Add(bodyFrame);
        //}


        //public void AddColor(BitmapSource bmpSource)
        //{
        //    //yeah..
        //}

        //public void AddColorFrame(ColorFrame colorFrame)
        //{
        //    colorFrames.Add(colorFrame);
        //    long size = 0;
        //    using (Stream s = new MemoryStream())
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(s, colorFrame);
        //        size = s.Length;
        //        Console.WriteLine("ColorFrame Size: " + size);
        //    }
        //    //Console.WriteLine("colorFrame: {0:dd\\.hh\\:mm\\:ss\\:fff}", colorFrame.RelativeTime);
        //}
        //public void AddBodyFrame(BodyFrame bodyFrame)
        //{
        //    bodyFrames.Add(bodyFrame);
        //    //Console.WriteLine("bodyFrame: {0:dd\\.hh\\:mm\\:ss\\:fff}\n", bodyFrame.RelativeTime);




        //    int colorMillis = colorFrames.Last().RelativeTime.Milliseconds;
        //    int bodyMillis = bodyFrame.RelativeTime.Milliseconds;
        //    if (colorMillis < bodyMillis)
        //    {
        //        Console.WriteLine("!!!!!!!!!!!!!! ColorFrame Primero !!!!!!!!!!!!!!");
        //    }
        //    else
        //    {
        //        Console.WriteLine("bodyFrame primero");
        //    }
        //}


    }
}
