using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal
{
    class _VisualizerXamlView
    {
        private _VisualizerXamlView() {}

        private static _VisualizerXamlView instance = new _VisualizerXamlView();
        public System.Windows.Controls.Image colorFrame { get; private set; }
        public System.Windows.Controls.Image bodyFrame { get; private set; }
        
        public static _VisualizerXamlView Instance()
        {
            //if( instance == null)
            //{
            //    instance = new VisualizerXamlView;
            //}
            return instance;
        }

        public void InitColorFrame(ref System.Windows.Controls.Image colorFrame)
        {
            this.colorFrame = colorFrame;
        }

        public void InitBodyFrame(ref System.Windows.Controls.Image bodyFrame)
        {
            this.bodyFrame = bodyFrame;
        }

        public void SetColorFrame(BitmapSource bitmap)
        {
            this.colorFrame.Source = bitmap;
        }

        public void SetBodyFrame(BitmapSource bitmap)
        {
            this.bodyFrame.Source = bitmap;
        }

    }
}
