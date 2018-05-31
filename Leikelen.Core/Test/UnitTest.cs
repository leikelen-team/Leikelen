using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.IO;

namespace cl.uv.leikelen.Test
{
    public class UnitTest : IDisposable
    {
        private const string _exportFileNamePath = "test/toExportFile.leikelen";
        private const string _importFileNamePath = "test/toImportFile.leikelen";

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
                Data.Access.External.TestScene.LoadTest("sceneTestExport");
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
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Assert.True(false);
            });
        }

        [Fact]
        public async Task UnitStopRecordTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Assert.True(false);
            });
        }

        #endregion

        #region emotion
        [Fact]
        public async Task UnitTrainEmotionTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Assert.True(false);
            });
        }

        [Fact]
        public async Task UnitClassifyEmotionTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Assert.True(false);
            });
        }
        #endregion
    }
}
