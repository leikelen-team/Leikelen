using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.IO;

namespace cl.uv.leikelen.Test
{
    public class IntegrationTest : IDisposable
    {
        private const string _exportFileNamePath = "test/toExportFile.leikelen";
        private const string _importFileNamePath = "test/toImportFile.leikelen";

        private const string _hahvSqlitePath = "test/data/SegYo-HAHV.db";
        private const string _halvSqlitePath = "test/data/SegYo-HALV.db";
        private const string _lahvSqlitePath = "test/data/SegYo-LAHV.db";
        private const string _lalvSqlitePath = "test/data/SegYo-LALV.db";

        public IntegrationTest()
        {
            if (File.Exists(_exportFileNamePath))
                File.Delete(_exportFileNamePath);
        }

        public void Dispose()
        {

        }

        #region File
        [Fact]
        public async Task IntegrationExportTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p1 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p1, res));


                int id1 = p1.PersonId;
                p1.Name = "updated person";
                var p2 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Update(p1);
                int id2 = p2.PersonId;
                Assert.NotNull(p2);
                Assert.Equal("updated person", p2.Name);
                Assert.Equal(id1, id2);
                //-------------------

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
                //----------
                id1 = p1.PersonId;
                Data.Access.DataAccessFacade.Instance.GetPersonAccess().Delete(p1);
                var allPersons = Data.Access.DataAccessFacade.Instance.GetPersonAccess().GetAll();
                List<int> ids = new List<int>();
                foreach (var p in allPersons)
                {
                    ids.Add(p.PersonId);
                }
                Assert.DoesNotContain(id1, ids);
                //----------

                Controller.FileController.Export(false, _exportFileNamePath);
                Assert.True(File.Exists(_exportFileNamePath));
            });
        }

        [Fact]
        public async Task IntegrationImportTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p1 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p1, res));


                int id1 = p1.PersonId;
                p1.Name = "updated person";
                var p2 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Update(p1);
                int id2 = p2.PersonId;
                Assert.NotNull(p2);
                Assert.Equal("updated person", p2.Name);
                Assert.Equal(id1, id2);
                //-------------------

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
                //----------
                id1 = p1.PersonId;
                Data.Access.DataAccessFacade.Instance.GetPersonAccess().Delete(p1);
                var allPersons = Data.Access.DataAccessFacade.Instance.GetPersonAccess().GetAll();
                List<int> ids = new List<int>();
                foreach (var p in allPersons)
                {
                    ids.Add(p.PersonId);
                }
                Assert.DoesNotContain(id1, ids);
                //----------
                if (File.Exists(_exportFileNamePath))
                {
                    File.Delete(_exportFileNamePath);
                }
                Controller.FileController.Export(false, _exportFileNamePath);
                Assert.True(File.Exists(_exportFileNamePath));

                Assert.NotNull(Controller.FileController.Import(_exportFileNamePath));
            });
        }
        #endregion

        #region scene
        [Fact]
        public async Task IntegrationCreateSceneTest()
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
        public async Task IntegrationGetScenesTest()
        {
            await Util.ThreadsUtil.StartSTATask(() =>
            {
                Data.Model.Scene sc1 = new Data.Model.Scene()
                {
                    Name = "Testing create scene 1",
                    Description = "create test 1",
                    NumberOfParticipants = 5,
                    Type = "some type",
                    Place = "Chile",
                    Duration = new TimeSpan()
                };
                var res1 = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveNew(sc1);
                Assert.NotNull(res1);

                Data.Model.Scene sc2 = new Data.Model.Scene()
                {
                    Name = "Testing create scene2",
                    Description = "create test2",
                    NumberOfParticipants = 5,
                    Type = "some type",
                    Place = "Chile",
                    Duration = new TimeSpan()
                };
                var res2 = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveNew(sc2);
                Assert.NotNull(res2);

                Data.Model.Scene sc3 = new Data.Model.Scene()
                {
                    Name = "Testing create scene3",
                    Description = "create test3",
                    NumberOfParticipants = 5,
                    Type = "some other type",
                    Place = "some other place",
                    Duration = new TimeSpan()
                };
                var res3 = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveNew(sc3);
                Assert.NotNull(res3);
                
                var scenes = Data.Access.DataAccessFacade.Instance.GetSceneAccess().GetAll();
                Assert.NotEmpty(scenes);
            });
        }

        [Fact]
        public async Task IntegrationUpdateSceneTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p1 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p1, res));


                int id1 = p1.PersonId;
                p1.Name = "updated person";
                var p2 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Update(p1);
                int id2 = p2.PersonId;
                Assert.NotNull(p2);
                Assert.Equal("updated person", p2.Name);
                Assert.Equal(id1, id2);
                //-------------------

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
                //----------
                id1 = p1.PersonId;
                Data.Access.DataAccessFacade.Instance.GetPersonAccess().Delete(p1);
                var allPersons = Data.Access.DataAccessFacade.Instance.GetPersonAccess().GetAll();
                List<int> ids = new List<int>();
                foreach (var p in allPersons)
                {
                    ids.Add(p.PersonId);
                }
                Assert.DoesNotContain(id1, ids);
                //----------
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
        public async Task IntegrationDeleteSceneTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p1 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p1, res));


                int id1 = p1.PersonId;
                p1.Name = "updated person";
                var p2 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Update(p1);
                int id2 = p2.PersonId;
                Assert.NotNull(p2);
                Assert.Equal("updated person", p2.Name);
                Assert.Equal(id1, id2);
                //-------------------

                var rc = new Controller.RecorderController();
                await rc.Record();
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

                await rc.Stop();
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
                //----------
                id1 = p1.PersonId;
                Data.Access.DataAccessFacade.Instance.GetPersonAccess().Delete(p1);
                var allPersons = Data.Access.DataAccessFacade.Instance.GetPersonAccess().GetAll();
                List<int> ids = new List<int>();
                foreach (var p in allPersons)
                {
                    ids.Add(p.PersonId);
                }
                Assert.DoesNotContain(id1, ids);
                //----------
                int resId = res.SceneId;
                res.Name = "updated scene";
                var res2 = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(res);
                int res2Id = res2.SceneId;
                Assert.NotNull(res2);
                Assert.Equal("updated scene", res2.Name);
                Assert.Equal(resId, res2Id);

                var pc = new Controller.PlayerController();
                pc.Play();
                bool play = false;
                foreach (var input in Module.InputLoader.Instance.SceneInputModules)
                {
                    if (input.Player.IsPlaying())
                    {
                        play = true;
                        break;
                    }
                }
                Assert.True(play);
                //-----------
                pc.Stop();
                play = false;
                foreach (var input in Module.InputLoader.Instance.SceneInputModules)
                {
                    if (input.Player.IsPlaying())
                    {
                        play = true;
                        break;
                    }
                }
                Assert.False(play);

                //--------
                if (File.Exists(_exportFileNamePath))
                {
                    File.Delete(_exportFileNamePath);
                }
                Controller.FileController.Export(false, _exportFileNamePath);
                Assert.True(File.Exists(_exportFileNamePath));

                Assert.NotNull(Controller.FileController.Import(_exportFileNamePath));
                //---------------
                res = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveNew(sc);
                 resId = res.SceneId;
                res.Name = "updated scene";
                 res2 = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(res);
                 res2Id = res2.SceneId;
                Assert.NotNull(res2);
                Assert.Equal("updated scene", res2.Name);
                Assert.Equal(resId, res2Id);
                //---------
                Data.Access.DataAccessFacade.Instance.GetSceneAccess().Delete(res);
                var scenes = Data.Access.DataAccessFacade.Instance.GetSceneAccess().GetAll();
                var sceneIds = new List<int>();
                foreach (var scene in scenes)
                {
                    sceneIds.Add(scene.SceneId);
                }
                Assert.DoesNotContain(resId, sceneIds);
            });
        }

        #endregion

        #region person
        [Fact]
        public async Task IntegrationCreatePersonTest()
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p, res));
            });
        }

        [Fact]
        public async Task IntegrationGetPersonTest()
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p, res));

                var persons = Data.Access.DataAccessFacade.Instance.GetPersonAccess().GetAll();
                Assert.NotEmpty(persons);
            });
        }

        [Fact]
        public async Task IntegrationUpdatePersonTest()
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p1 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p1, res));


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
        public async Task IntegrationDeletePersonTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p1 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p1, res));


                int id1 = p1.PersonId;
                p1.Name = "updated person";
                var p2 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Update(p1);
                int id2 = p2.PersonId;
                Assert.NotNull(p2);
                Assert.Equal("updated person", p2.Name);
                Assert.Equal(id1, id2);
                //-------------------

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
                //----------
                id1 = p1.PersonId;
                Data.Access.DataAccessFacade.Instance.GetPersonAccess().Delete(p1);
                var allPersons = Data.Access.DataAccessFacade.Instance.GetPersonAccess().GetAll();
                List<int> ids = new List<int>();
                foreach (var p in allPersons)
                {
                    ids.Add(p.PersonId);
                }
                Assert.DoesNotContain(id1, ids);
                //----------
            });
        }

        #endregion

        #region player
        [Fact]
        public async Task IntegrationStartPlayTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p1 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p1, res));


                int id1 = p1.PersonId;
                p1.Name = "updated person";
                var p2 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Update(p1);
                int id2 = p2.PersonId;
                Assert.NotNull(p2);
                Assert.Equal("updated person", p2.Name);
                Assert.Equal(id1, id2);
                //-------------------

                var rc = new Controller.RecorderController();
                await rc.Record();
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

                await rc.Stop();
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
                //----------
                id1 = p1.PersonId;
                Data.Access.DataAccessFacade.Instance.GetPersonAccess().Delete(p1);
                var allPersons = Data.Access.DataAccessFacade.Instance.GetPersonAccess().GetAll();
                List<int> ids = new List<int>();
                foreach (var p in allPersons)
                {
                    ids.Add(p.PersonId);
                }
                Assert.DoesNotContain(id1, ids);
                //----------
                int resId = res.SceneId;
                res.Name = "updated scene";
                var res2 = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(res);
                int res2Id = res2.SceneId;
                Assert.NotNull(res2);
                Assert.Equal("updated scene", res2.Name);
                Assert.Equal(resId, res2Id);

                var pc = new Controller.PlayerController();
                pc.Play();
                bool play = false;
                foreach (var input in Module.InputLoader.Instance.SceneInputModules)
                {
                    if (input.Player.IsPlaying())
                    {
                        play = true;
                        break;
                    }
                }
                Assert.True(play);
            });
        }

        [Fact]
        public async Task IntegrationStopPlayTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p1 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p1, res));


                int id1 = p1.PersonId;
                p1.Name = "updated person";
                var p2 = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Update(p1);
                int id2 = p2.PersonId;
                Assert.NotNull(p2);
                Assert.Equal("updated person", p2.Name);
                Assert.Equal(id1, id2);
                //-------------------

                var rc = new Controller.RecorderController();
                await rc.Record();
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

                await rc.Stop();
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
                //----------
                id1 = p1.PersonId;
                Data.Access.DataAccessFacade.Instance.GetPersonAccess().Delete(p1);
                var allPersons = Data.Access.DataAccessFacade.Instance.GetPersonAccess().GetAll();
                List<int> ids = new List<int>();
                foreach (var p in allPersons)
                {
                    ids.Add(p.PersonId);
                }
                Assert.DoesNotContain(id1, ids);
                //----------
                int resId = res.SceneId;
                res.Name = "updated scene";
                var res2 = Data.Access.DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(res);
                int res2Id = res2.SceneId;
                Assert.NotNull(res2);
                Assert.Equal("updated scene", res2.Name);
                Assert.Equal(resId, res2Id);

                var pc = new Controller.PlayerController();
                pc.Play();
                bool play = false;
                foreach (var input in Module.InputLoader.Instance.SceneInputModules)
                {
                    if (input.Player.IsPlaying())
                    {
                        play = true;
                        break;
                    }
                }
                Assert.True(play);
                //-----------
                pc.Stop();
                play = false;
                foreach (var input in Module.InputLoader.Instance.SceneInputModules)
                {
                    if (input.Player.IsPlaying())
                    {
                        play = true;
                        break;
                    }
                }
                Assert.False(play);
            });
        }

        #endregion

        #region recorder
        [Fact]
        public async Task IntegrationStartRecordTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p, res));

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
        public async Task IntegrationStopRecordTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p, res));

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
        public async Task IntegrationTrainEmotionTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p, res));

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
                //---------------------
                API.Module.General.GeneralModule trainmodule = null;
                foreach (var gm in Module.GeneralLoader.Instance.GeneralModules)
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
        public async Task IntegrationClassifyEmotionTest()
        {
            await Util.ThreadsUtil.StartSTATask(async () =>
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
                Data.Access.Internal.SceneInUse.Instance.Set(res);

                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetSceneInUseAccess().GetScene());

                var p = Data.Access.DataAccessFacade.Instance.GetPersonAccess().Add("invented person", null, new DateTime(1992, 5, 3), 0, null);
                Assert.NotNull(Data.Access.DataAccessFacade.Instance.GetPersonAccess().AddToScene(p, res));

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
                //---------------------
                API.Module.General.GeneralModule trainmodule = null;
                foreach (var gm in Module.GeneralLoader.Instance.GeneralModules)
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
                //var emotionModelPath = @"data\modal\Emotion\emotionmodel.svm";
                //Assert.True(System.IO.File.Exists(emotionModelPath));
                

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
