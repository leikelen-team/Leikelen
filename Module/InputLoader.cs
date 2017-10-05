using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.Input;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Module
{
    public class InputLoader
    {
        public List<InputModule> SceneInputModules { get; private set; }
        public Dictionary<Person, List<InputModule>> PersonInputModules { get; private set; }
        public IVideo VideoHandler { get; private set; }
        private static InputLoader _instance;

        public static InputLoader Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new InputLoader();
                return _instance;
            }
        }

        public void FillPersonInputModules(Person person)
        {
            var personInputModules = new List<InputModule>()
            {
                new Input.OpenBCI.OpenBCIInput(person)
            };

            PersonInputModules.Add(person, personInputModules);

        }

        private InputLoader()
        {
            SceneInputModules = new List<InputModule>();
            PersonInputModules = new Dictionary<Person, List<InputModule>>();

            FillSceneInputModules();
        }

        private void FillSceneInputModules()
        {
            var kinectInput = new Input.Kinect.KinectInput();
            SceneInputModules.Add(kinectInput);
            VideoHandler = Input.Kinect.KinectInput.SkeletonColorVideoViewer;

            //TODO: esto es temporal por motivos de prueba
            //SceneInputModules.Add(new Input.OpenBCI.OpenBCIInput());
        }

        
    }
}
