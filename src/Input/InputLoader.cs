using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.src.API.Input;

namespace cl.uv.leikelen.src.Input
{
    public class InputLoader
    {
        public List<InputType> Inputs;
        public IVideoViewer videoViewer;
        private static InputLoader _instance;

        public static InputLoader Instance
        {
            get
            {
                if (_instance == null) _instance = new InputLoader();
                return _instance;
            }
        }

        private InputLoader()
        {
            Inputs = new List<InputType>();
            addInputs();
        }

        public void addInputs()
        {
            var kinectInput = new InputType();
            var monitor = new Kinect.Monitor();
            kinectInput.Monitor = monitor;
            kinectInput.Player = new Kinect.Player();
            Inputs.Add(kinectInput);

            videoViewer = monitor.videoViewer;
        }
    }
}
