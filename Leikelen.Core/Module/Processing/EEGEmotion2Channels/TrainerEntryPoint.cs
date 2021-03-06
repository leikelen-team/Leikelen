﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.API.Helper;
using cl.uv.leikelen.API.Module.General;
using cl.uv.leikelen.Module.Processing.EEGEmotion2Channels.View;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.API.DataAccess;
using cl.uv.leikelen.Data.Access;

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels
{
    public class TrainerEntryPoint : GeneralModule
    {
        public static Dictionary<TagType, List<Scene>> ScenesAndTags = new Dictionary<TagType, List<Scene>>();
        private IDataAccessFacade _dataAccessFacade = new DataAccessFacade();

        public TrainerEntryPoint()
        {
            _dataAccessFacade.GetModalAccess().AddIfNotExists("Emotion", "Affects or feels of a person");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Emotion", "LALV", "Low arousal Low Valence", "emotionmodel.svm");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Emotion", "LAHV", "Low arousal High Valence", "emotionmodel.svm");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Emotion", "HALV", "High arousal Low Valence", "emotionmodel.svm");
            _dataAccessFacade.GetSubModalAccess().AddIfNotExists("Emotion", "HAHV", "High arousal High Valence", "emotionmodel.svm");


            Windows = new List<Tuple<string, WindowBuilder>>();
            Name = Properties.EEGEmotion2Channels.ClassificationModuleName;
            var configWindow = new Tuple<string, WindowBuilder>(Properties.EEGEmotion2Channels.ConfigWindowTitle,
                new WindowBuilder(new ConfigurationWindow(Properties.EEGEmotion2Channels.ConfigWindowTitle)));
            //var sceneWindow = new Tuple<string, WindowBuilder>(Properties.EEGEmotion2Channels.TrainerSceneSelectorTitle,
            //    new WindowBuilder(new TrainerSceneSelector()));
            var fileWindow = new Tuple<string, WindowBuilder>(Properties.EEGEmotion2Channels.TrainerFileSelectorTitle,
                new WindowBuilder(new TrainerFileSelector()));
            Windows.Add(configWindow);
            //Windows.Add(sceneWindow);
            Windows.Add(fileWindow);
        }
    }
}
