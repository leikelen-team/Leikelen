﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.Controller;
using cl.uv.leikelen.Data.Access.External;
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.Module;
using cl.uv.leikelen.Module.Processing.EEGEmotion2Channels;

namespace cl.uv.leikelen.test.Unit
{
    [TestClass]
    public class UnitTest1
    {
        [ClassInitialize()]
        public static void UnitInitialize(TestContext testContext)
        {
            //DataAccessFacade.Instance.GetSceneAccess().ClearDb();
            TestScene.LoadFakeScene("sceneTest1");
            TestScene.LoadFakeScene("sceneTest2");
            TestScene.LoadFakeScene("sceneTest3");
            
            DataAccessFacade.Instance.GetModalAccess().AddIfNotExists("Emotion", "Affects or feels of a person");
            DataAccessFacade.Instance.GetSubModalAccess().AddIfNotExists("Emotion", "LALV", "Low arousal Low Valence", "emotionmodel.svm");
            DataAccessFacade.Instance.GetSubModalAccess().AddIfNotExists("Emotion", "LAHV", "Low arousal High Valence", "emotionmodel.svm");
            DataAccessFacade.Instance.GetSubModalAccess().AddIfNotExists("Emotion", "HALV", "High arousal Low Valence", "emotionmodel.svm");
            DataAccessFacade.Instance.GetSubModalAccess().AddIfNotExists("Emotion", "HAHV", "High arousal High Valence", "emotionmodel.svm");
        }

        [ClassCleanup()]
        public static void Cleanup()
        {
            if (System.IO.File.Exists("test/toExportFile.leikelen"))
            {
                System.IO.File.Delete("test/toExportFile.leikelen");
            }
            if (System.IO.File.Exists("data/modal/Emotion/emotionmodel.svm"))
            {
                System.IO.File.Delete("data/modal/Emotion/emotionmodel.svm");
            }
            DataAccessFacade.Instance.DeleteDatabase();
        }

        [TestMethod]
        public void UnitExportTest()
        {
            var scenes = DataAccessFacade.Instance.GetSceneAccess().GetAll();
            Scene sc = scenes[0];
            FileController.Export(false, "test/toExportFile.leikelen");
            Assert.IsTrue(System.IO.File.Exists("test/toExportFile.leikelen"));
        }

        [TestMethod]
        public void UnitImportTest()
        {
            Assert.IsNotNull(FileController.Import("test/toImportFile.leikelen"));
        }

        #region Scene
        [TestMethod]
        public void UnitCreateSceneTest()
        {
            Scene sc = new Scene()
            {
                Name = "Prooobando",
                Description = "create test",
                NumberOfParticipants = 5,
                Type = "tiiipo",
                Place = "luuuugar",
                Duration = new TimeSpan()
            };
            var res = DataAccessFacade.Instance.GetSceneAccess().SaveNew(sc);
            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void UnitGetScenesTest()
        {
            var scenes = DataAccessFacade.Instance.GetSceneAccess().GetAll();
            Assert.AreNotEqual(scenes.Count, 0);
        }

        [TestMethod]
        public void UnitDeleteSceneTest()
        {
            Scene sc = new Scene()
            {
                Name = "ToDelete",
                Description = "create test",
                NumberOfParticipants = 5,
                Type = "tiiipo",
                Place = "luuuugar",
                Duration = new TimeSpan()
            };
            var res = DataAccessFacade.Instance.GetSceneAccess().SaveNew(sc);
            DataAccessFacade.Instance.GetSceneAccess().Delete(res);
        }

        [TestMethod]
        public void UnitUpdateSceneTest()
        {
            Scene sc = new Scene()
            {
                Name = "ToUpdate",
                Description = "create test",
                NumberOfParticipants = 5,
                Type = "tiiipo",
                Place = "luuuugar",
                Duration = new TimeSpan()
            };
            var res = DataAccessFacade.Instance.GetSceneAccess().SaveNew(sc);
            sc.Name = "updated";
            Assert.IsNotNull(DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(sc));
        }
        #endregion

        #region Emotion
        [TestMethod]
        public void UnitTrainEmoTest()
        {
            Assert.IsNotNull(LearningModel.Train(FalseTrain()));
        }

        [TestMethod]
        public void UnitClassifyEmoTest()
        {
            if (!System.IO.File.Exists("data/modal/Emotion/emotionmodel.svm"))
            {
                LearningModel.Train(FalseTrain());
            }
            Assert.AreEqual(TagType.HAHV, LearningModel.Classify(FalseTrain()[TagType.HAHV][0]));
        }

        private Dictionary<TagType, List<List<double[]>>> FalseTrain()
        {
            Dictionary<TagType, List<List<double[]>>> dataToTrain = new Dictionary<TagType, List<List<double[]>>>();

            return dataToTrain;
        }

        #endregion

        #region Person
        [TestMethod]
        public void UnitCreatePersonTest()
        {
            Assert.IsNotNull(DataAccessFacade.Instance.GetPersonAccess().Add("Doroteo", null, new DateTime(1992, 5, 3), 0));
        }

        [TestMethod]
        public void UnitGetPersonsTest()
        {
            var persons = DataAccessFacade.Instance.GetPersonAccess().GetAll();
            Assert.AreNotEqual(0, persons.Count);
        }

        [TestMethod]
        public void UnitDeletePersonTest()
        {
            var person = DataAccessFacade.Instance.GetPersonAccess().Add("Domitilo", null, new DateTime(1992, 5, 8), 0);
            DataAccessFacade.Instance.GetPersonAccess().Delete(person);
        }

        [TestMethod]
        public void UnitUpdatePersonTest()
        {
            var person = DataAccessFacade.Instance.GetPersonAccess().Add("Domitilo", null, new DateTime(1992, 5, 8), 0);
            person.Name = "Eduvigis";
            person.Sex = 1;
            Assert.IsNotNull(DataAccessFacade.Instance.GetPersonAccess().Update(person));
        }
        #endregion

        #region Player
        [TestMethod]
        public void UnitStartPlayTest()
        {
            FileController.Import("test/toImportFile.leikelen");
            var pc = new PlayerController();
            pc.Play();
            bool rec = false;
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                if (input.Player.IsPlaying())
                {
                    rec = true;
                    break;
                }
            }
            Assert.IsTrue(rec);
        }

        [TestMethod]
        public void UnitStopPlayTest()
        {
            FileController.Import("test/toImportFile.leikelen");
            var pc = new PlayerController();
            pc.Play();
            pc.Stop();
            bool rec = false;
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                if (input.Player.IsPlaying())
                {
                    rec = true;
                    break;
                }
            }
            Assert.IsFalse(rec);
        }
        #endregion

        #region Recorder
        [TestMethod]
        public async Task UnitStartRecordTestAsync()
        {
            SceneInUse.Instance.Set(FileController.Import("test/toImportFile.leikelen"));
            var rc = new RecorderController();
            await rc.Record();
            bool rec = false;
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                if (input.Monitor.IsRecording())
                {
                    rec = true;
                    break;
                }
            }
            Assert.IsTrue(rec);
        }

        [TestMethod]
        public async Task UnitStopRecordTestAsync()
        {
            SceneInUse.Instance.Set(FileController.Import("test/toImportFile.leikelen"));
            var rc = new RecorderController();
            await rc.Record();
            await rc.Stop();
            bool rec = false;
            foreach (var input in InputLoader.Instance.SceneInputModules)
            {
                if (input.Monitor.IsRecording())
                {
                    rec = true;
                    break;
                }
            }
            Assert.IsFalse(rec);
        }
        #endregion
    }
}
