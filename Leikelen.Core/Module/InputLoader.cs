﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cl.uv.leikelen.API.Module.Input;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Module
{
    /// <summary>
    /// Gets the input modules/sensors (scene and person)
    /// </summary>
    public class InputLoader
    {
        /// <summary>
        /// Gets the scene input modules.
        /// </summary>
        /// <value>
        /// The scene input modules.
        /// </value>
        public List<InputModule> SceneInputModules { get; private set; }

        /// <summary>
        /// Gets the person input modules.
        /// </summary>
        /// <value>
        /// The person input modules.
        /// </value>
        public Dictionary<Person, List<InputModule>> PersonInputModules { get; private set; }

        /// <summary>
        /// Gets the video handler to see in the player.
        /// </summary>
        /// <value>
        /// The video handler to see in the player.
        /// </value>
        public IVideo VideoHandler { get; private set; }

        public static event EventHandler InputModulesHasReset;

        private static InputLoader _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static InputLoader Instance
        {
            get
            {
                if (ReferenceEquals(null, _instance)) _instance = new InputLoader();
                return _instance;
            }
        }

        /// <summary>
        /// Fills the person input modules.
        /// </summary>
        /// <param name="person">The person.</param>
        public void FillPersonInputModules(Person person)
        {
            var personInputModules = new List<InputModule>()
            {
                new Input.OpenBCI.OpenBCIInput(person),
                new Input.MindwaveTGC.MindwaveTGCInput(person)
            };

            PersonInputModules.Add(person, personInputModules);
        }

        private InputLoader()
        {
            SceneInputModules = new List<InputModule>();
            PersonInputModules = new Dictionary<Person, List<InputModule>>();

            FillSceneInputModules();
        }

        public static void Reset()
        {
            foreach(var scIn in _instance.SceneInputModules)
            {
                try
                {
                    scIn.Disable();
                    scIn.Monitor.Close();
                    scIn.Player.Close();
                }
                catch(Exception ex)
                {

                }
            }
            foreach(var ps in _instance.PersonInputModules.Keys)
            {
                foreach(var psIn in _instance.PersonInputModules[ps])
                {
                    try
                    {
                        psIn.Disable();
                        psIn.Monitor.Close();
                        psIn.Player.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            _instance = null;
            _instance = new InputLoader();
            InputModulesHasReset?.Invoke(_instance, new EventArgs());
        }
        private void FillSceneInputModules()
        {
            var kinectInput = new Input.Kinect.KinectInput();
            SceneInputModules.Add(kinectInput);
            VideoHandler = Input.Kinect.KinectInput.SkeletonColorVideoViewer;
        }

        
    }
}
