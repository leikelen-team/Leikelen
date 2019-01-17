using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.IO;

/// <summary>
/// Test for the application.
/// </summary>
namespace cl.uv.leikelen.Test
{
    public class UnitTest : IDisposable
    {
        private const string _exportFileNamePath = "test/toExportFile.leikelen";
        private const string _importFileNamePath = "test/toImportFile.leikelen";

        private const string _hahvSqlitePath = "test/data/SegYo-HAHV.db";
        private const string _halvSqlitePath = "test/data/SegYo-HALV.db";
        private const string _lahvSqlitePath = "test/data/SegYo-LAHV.db";
        private const string _lalvSqlitePath = "test/data/SegYo-LALV.db";

        public UnitTest()
        {
            if (File.Exists(_exportFileNamePath))
                File.Delete(_exportFileNamePath);
        }

        public void Dispose()
        {

        }

        #region File
        [Fact]
        public async Task UnitExportTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Data.Access.External.TestScene.LoadFakeScene("sceneTestExport");
                Controller.FileController.Export(false, _exportFileNamePath);
                Assert.True(File.Exists(_exportFileNamePath));
            });
        }

        [Fact]
        public async Task UnitImportTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Assert.NotNull(Controller.FileController.Import(_importFileNamePath));
            });
        }
        #endregion

        #region scene
        [Fact]
        public async Task UnitCreateSceneTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Data.Model.Scene sc = new Data.Model.Scene()
                {
                    Name = "Testing create scene",
                    Description = "create test",
                    NumberOfParticipants = 5,
                    Type = "some type",
                    Place = "some place",
                    Duration = new TimeSpan()
                };
                var res = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveNew(sc);
                Assert.NotNull(res);
            });
        }

        [Fact]
        public async Task UnitGetScenesTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Data.Access.External.TestScene.LoadTest("sceneTestGet");
                var scenes = Data.Access.DataAccessFacade.Instance.GetSceneAccess().GetAll();
                Assert.NotEmpty(scenes);
            });
        }

        [Fact]
        public async Task UnitUpdateSceneTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Data.Model.Scene sc = new Data.Model.Scene()
                {
                    Name = "Testing update scene",
                    Description = "update test",
                    NumberOfParticipants = 5,
                    Type = "some type",
                    Place = "some place",
                    Duration = new TimeSpan()
                };
                var res = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveNew(sc);
                int resId = res.SceneId;
                res.Name = "updated scene";
                var res2 = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(res);
                int res2Id = res2.SceneId;
                Assert.NotNull(res2);
                Assert.Equal("updated scene", res2.Name);
                Assert.Equal(resId, res2Id);
            });
        }

        [Fact]
        public async Task UnitDeleteSceneTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Data.Model.Scene sc = new Data.Model.Scene()
                {
                    Name = "Testing delete scene",
                    Description = "delete test",
                    NumberOfParticipants = 5,
                    Type = "some type",
                    Place = "some place",
                    Duration = new TimeSpan()
                };
                var res = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveNew(sc);
                int resId = res.SceneId;
                Data.Access.DataAccessFacade.Instance.GetSceneAccess().Delete(res);
                var scenes = Data.Access.DataAccessFacade.Instance.GetSceneAccess().GetAll();
                var sceneIds = new List<int>();
                foreach(var scene in scenes)
                {
                    sceneIds.Add(scene.SceneId);
                }
                Assert.DoesNotContain(resId, sceneIds);
            });
        }

        #endregion

        #region person
        [Fact]
        public async Task UnitCreatePersonTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null));
            });
        }

        [Fact]
        public async Task UnitGetPersonTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("get", null, new DateTime(1992, 5, 3), 0, null);
                var persons = Data.Access.DataAccessFacade.Instance.GetPersonAccess().GetAll();
                Assert.NotEmpty(persons);
            });
        }

        [Fact]
        public async Task UnitUpdatePersonTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                var p1 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("to update person", null, new DateTime(1992, 5, 3), 0, null);
                int id1 = p1.PersonId;
                p1.Name = "updated person";
                var p2 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Update(p1);
                int id2 = p2.PersonId;
                Assert.NotNull(p2);
                Assert.Equal("updated person", p2.Name);
                Assert.Equal(id1, id2);
            });
        }

        [Fact]
        public async Task UnitDeletePersonTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                var p1 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("to delete person", null, new DateTime(1992, 5, 3), 0, null);
                int id1 = p1.PersonId;
                Data.Access.DataAccessFacade.Instance.GetPersonAccess().Delete(p1);
                var allPersons = Data.Access.DataAccessFacade.Instance.GetPersonAccess().GetAll();
                List<int> ids = new List<int>();
                foreach(var p in allPersons)
                {
                    ids.Add(p.PersonId);
                }
                Assert.DoesNotContain(id1, ids);
            });
        }

        #endregion

        #region player
        [Fact]
        public async Task UnitStartPlayTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Controller.FileController.Import("test/toImportFile.leikelen");
                var pc = new Controller.PlayerController();
                pc.Play();
                bool rec = false;
                foreach (var input in Module.InputLoader.Instance.SceneInputModules)
                {
                    if (input.Player.IsPlaying())
                    {
                        rec = true;
                        break;
                    }
                }
                Assert.True(rec);
            });
        }

        [Fact]
        public async Task UnitStopPlayTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Controller.FileController.Import("test/toImportFile.leikelen");
                var pc = new Controller.PlayerController();
                pc.Play();
                pc.Stop();
                bool rec = false;
                foreach (var input in Module.InputLoader.Instance.SceneInputModules)
                {
                    if (input.Player.IsPlaying())
                    {
                        rec = true;
                        break;
                    }
                }
                Assert.False(rec);
            });
        }

        #endregion

        #region recorder
        [Fact]
        public async Task UnitStartRecordTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
            {
                Controller.FileController.Import("test/toImportFile.leikelen");
                var pc = new Controller.RecorderController();
                await pc.Record();
                bool rec = false;
                foreach (var input in Module.InputLoader.Instance.SceneInputModules)
                {
                    if (input.Monitor.IsRecording())
                    {
                        rec = true;
                        break;
                    }
                }
                Assert.True(rec);
            });
        }

        [Fact]
        public async Task UnitStopRecordTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
            {
                Controller.FileController.Import("test/toImportFile.leikelen");
                var pc = new Controller.RecorderController();
                await pc.Record();
                bool rec = false;
                foreach (var input in Module.InputLoader.Instance.SceneInputModules)
                {
                    if (input.Monitor.IsRecording())
                    {
                        rec = true;
                        break;
                    }
                }
                Assert.True(rec);
                await pc.Stop();
                rec = false;
                foreach (var input in Module.InputLoader.Instance.SceneInputModules)
                {
                    if (input.Monitor.IsRecording())
                    {
                        rec = true;
                        break;
                    }
                }
                Assert.False(rec);
            });
        }

        #endregion

        #region emotion
        [Fact]
        public async Task UnitTrainEmotionTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                API.Module.General.GeneralModule trainmodule = null;
                foreach(var gm in Module.GeneralLoader.Instance.GeneralModules)
                {
                    if (gm.Name.Equals(Module.Processing.EEGEmotion2Channels.Properties.EEGEmotion2Channels.ClassificationModuleName))
                    {
                        trainmodule = gm;
                        break;
                    }
                }
                Assert.NotNull(trainmodule);
                Module.Processing.EEGEmotion2Channels.View.TrainerFileSelector filesel =
                new Module.Processing.EEGEmotion2Channels.View.TrainerFileSelector();
                Assert.NotNull(filesel);

                var dict = new Dictionary<Module.Processing.EEGEmotion2Channels.TagType, List<List<double[]>>>
                {
                    {
                        Module.Processing.EEGEmotion2Channels.TagType.HAHV,
                        filesel.ReadSqlite(_hahvSqlitePath)
                    },
                    {
                        Module.Processing.EEGEmotion2Channels.TagType.HALV,
                        filesel.ReadSqlite(_halvSqlitePath)
                    },
                    { Module.Processing.EEGEmotion2Channels.TagType.LAHV,
                        filesel.ReadSqlite(_lahvSqlitePath)
                    },
                    { Module.Processing.EEGEmotion2Channels.TagType.LALV,
                        filesel.ReadSqlite(_lalvSqlitePath)
                    }
                };
                Module.Processing.EEGEmotion2Channels.LearningModel.Train(dict);
            });
        }

        [Fact]
        public async Task UnitClassifyEmotionTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                //var emotionModelPath = @"data\modal\Emotion\emotionmodel.svm";
                //Assert.True(System.IO.File.Exists(emotionModelPath));

                Module.Processing.EEGEmotion2Channels.View.TrainerFileSelector filesel =
                new Module.Processing.EEGEmotion2Channels.View.TrainerFileSelector();
                Assert.NotNull(filesel);

                var lalvData = filesel.ReadSqlite(_lalvSqlitePath);
                var tag = Module.Processing.EEGEmotion2Channels.LearningModel.Classify(lalvData[0]);
                Assert.Equal(
                    Module.Processing.EEGEmotion2Channels.TagType.LALV,
                    tag);
            });
        }
        #endregion
    }
}
