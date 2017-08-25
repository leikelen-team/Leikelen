﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.ProcessingModule;
using cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels.View;

namespace cl.uv.leikelen.ProcessingModule.EEGEmotion2Channels
{
    public class TrainerEntryPoint : API.ProcessingModule.ProcessingModule, IEegProcessingModule
    {
        private readonly EEGReceiver _logic;

        public override Task FunctionAfterStop()
        {
            return new Task(_logic.Train);
        }

        public TrainerEntryPoint()
        {
            Windows = new List<Tuple<string, WindowBuilder>>();
            IsActiveBeforeRecording = false;
            Name = Properties.EEGEmotion2Channels.ClassificationModuleName;
            IsEnabled = true;
            var configWindow = new Tuple<string, WindowBuilder>(Properties.EEGEmotion2Channels.ConfigWindowTitle,
                new WindowBuilder(new ClassifierWindow()));
            Windows.Add(configWindow);
            Plurality = ProcessingPlurality.General;

            _logic = new EEGReceiver(ClassifierType.Trainer);
        }

        public EventHandler<EegFrameArrivedEventArgs> EegListener()
        {
            return _logic.DataReceiver;
        }
    }
}
