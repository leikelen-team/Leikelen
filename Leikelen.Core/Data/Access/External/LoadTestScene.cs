using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Access.Internal;
using cl.uv.leikelen.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.Data.Access.External
{
    public static class TestScene
    {
        public static void LoadTest(string sceneName)
        {
            var rnd = new Random();
            var scenes = DataAccessFacade.Instance.GetSceneAccess().GetAll();
            if (!ReferenceEquals(null, scenes) || scenes.Count == 0)
            {
                var testScene = scenes.Find(s => s.Name.Equals(sceneName));
                if (!ReferenceEquals(null, testScene))
                {
                    testScene = DataAccessFacade.Instance.GetSceneAccess().Get(testScene.SceneId);
                    SceneInUse.Instance.Set(testScene);
                    return;
                }
            }
            var scene = new Scene()
            {
                Name = sceneName,
                NumberOfParticipants = 2,
                Type = "Test scene",
                Description = "This is a test scene\n only for purposes of development",
                Place = "Programmed",
                RecordRealDateTime = new DateTime(2017, 8, 25, 16, 2, 0),
                Duration = new TimeSpan(0, 10, 30)
            };
            SceneInUse.Instance.Set(scene);
            var person1 = DataAccessFacade.Instance.GetPersonAccess().Add("Erick", null, new DateTime(1992, 4,3), 0, null);
            var person2 = DataAccessFacade.Instance.GetPersonAccess().Add("Dorotea", null, null, 1, null);
            DataAccessFacade.Instance.GetPersonAccess().AddToScene(person1, scene);
            DataAccessFacade.Instance.GetPersonAccess().AddToScene(person2, scene);

            if (!DataAccessFacade.Instance.GetModalAccess().Exists("Voice"))
                DataAccessFacade.Instance.GetModalAccess().Add("Voice", "Hablo o no de kinect");
            if (!DataAccessFacade.Instance.GetSubModalAccess().Exists("Voice", "Talked"))
                DataAccessFacade.Instance.GetSubModalAccess().Add("Voice", "Talked", "Talked or not", null);

            if (!DataAccessFacade.Instance.GetModalAccess().Exists("Discrete Posture"))
                DataAccessFacade.Instance.GetModalAccess().Add("Discrete Posture", "Posturas de kinect");
            if (!DataAccessFacade.Instance.GetSubModalAccess().Exists("Discrete Posture", "Seated"))
                DataAccessFacade.Instance.GetSubModalAccess().Add("Discrete Posture", "Seated", "Is Seated", null);
            if (!DataAccessFacade.Instance.GetSubModalAccess().Exists("Discrete Posture", "Hand On Wrist"))
                DataAccessFacade.Instance.GetSubModalAccess().Add("Discrete Posture", "Hand On Wrist", "o<>===<", null);
            if (!DataAccessFacade.Instance.GetSubModalAccess().Exists("Discrete Posture", "Asking Help"))
                DataAccessFacade.Instance.GetSubModalAccess().Add("Discrete Posture", "Asking Help", "I have a question teacher plis", null);
            if (!DataAccessFacade.Instance.GetSubModalAccess().Exists("Discrete Posture", "CrossedArms"))
                DataAccessFacade.Instance.GetSubModalAccess().Add("Discrete Posture", "CrossedArms", "Is crossing his/her arms", null);


            foreach (var pis in DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene)
            {
                foreach (var m in DataAccessFacade.Instance.GetModalAccess().GetAll())
                {
                    Console.WriteLine($"{m.ModalTypeId} tiene {m.SubModalTypes.Count} submodaltypes");
                    foreach (var s in m.SubModalTypes)
                    {
                        int useOrNot = rnd.Next(1, 3);
                        Console.WriteLine($"use or not: {useOrNot}");
                        if (useOrNot == 1) break;
                        var which = rnd.Next(1, 3);
                        Console.WriteLine($"Which: {which}");
                        var data = rnd.Next(1, 3);
                        Console.WriteLine($"Data: {data}");
                        int representTypeQuantity = rnd.Next(5, 20);
                        int lastTickGenerated = 0;
                        for (int j = 1; j <= representTypeQuantity; j++)
                        {
                            double doubleData = rnd.NextDouble();
                            string subtitleData = "hola este es un string";
                            int sceneTicks = (int)DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().Duration.Ticks;
                            switch (which)
                            {
                                case 1:
                                    switch (data)
                                    {
                                        case 1:
                                            DataAccessFacade.Instance.GetEventAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, new TimeSpan(rnd.Next(0, sceneTicks)), doubleData, -1);
                                            break;
                                        case 2:
                                            DataAccessFacade.Instance.GetEventAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, new TimeSpan(rnd.Next(0, sceneTicks)), subtitleData, -1);
                                            break;
                                        case 3:
                                            DataAccessFacade.Instance.GetEventAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, new TimeSpan(rnd.Next(0, sceneTicks)), -1);
                                            break;
                                    }
                                    break;
                                case 2:
                                    int minTicks = lastTickGenerated + 1;
                                    int maxTicks = sceneTicks / representTypeQuantity * j;
                                    lastTickGenerated = maxTicks;
                                    TimeSpan min = new TimeSpan(minTicks);
                                    TimeSpan max = new TimeSpan(rnd.Next(minTicks, maxTicks));
                                    switch (data)
                                    {
                                        case 1:
                                            DataAccessFacade.Instance.GetIntervalAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, min, max, doubleData);
                                            break;
                                        case 2:
                                            DataAccessFacade.Instance.GetIntervalAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, min, max, subtitleData);
                                            break;
                                        case 3:
                                            DataAccessFacade.Instance.GetIntervalAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, min, max);
                                            break;
                                    }
                                    break;
                                case 3:
                                    switch (data)
                                    {
                                        case 1:
                                            DataAccessFacade.Instance.GetTimelessAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, rnd.Next(0, 24), doubleData);
                                            break;
                                        case 2:
                                            DataAccessFacade.Instance.GetTimelessAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, rnd.Next(0, 24), subtitleData);
                                            break;
                                        case 3:
                                            DataAccessFacade.Instance.GetTimelessAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, rnd.Next(0, 24));
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }


        public static void LoadFakeScene(string sceneName)
        {
            var scenes = DataAccessFacade.Instance.GetSceneAccess().GetAll();
            if (!ReferenceEquals(null, scenes) || scenes.Count == 0)
            {
                var testScene = scenes.Find(s => s.Name.Equals(sceneName));
                if (!ReferenceEquals(null, testScene))
                {
                    testScene = DataAccessFacade.Instance.GetSceneAccess().Get(testScene.SceneId);
                    SceneInUse.Instance.Set(testScene);
                    return;
                }
            }
            var scene = new Scene()
            {
                Name = sceneName,
                NumberOfParticipants = 2,
                Type = "Test scene",
                Description = "This is a test scene\n only for purposes of development",
                Place = "Programmed",
                RecordRealDateTime = new DateTime(2017, 8, 25, 16, 2, 0),
                Duration = new TimeSpan(0, 10, 30)
            };
            SceneInUse.Instance.Set(scene);
            var person1 = DataAccessFacade.Instance.GetPersonAccess().Add("Erick", null, new DateTime(1992, 4, 3), 0, null);
            var person2 = DataAccessFacade.Instance.GetPersonAccess().Add("Dorotea", null, null, 1, null);
            DataAccessFacade.Instance.GetPersonAccess().AddToScene(person1, scene);
            DataAccessFacade.Instance.GetPersonAccess().AddToScene(person2, scene);

            if (!DataAccessFacade.Instance.GetModalAccess().Exists("Voice"))
                DataAccessFacade.Instance.GetModalAccess().Add("Voice", "Hablo o no de kinect");
            if (!DataAccessFacade.Instance.GetSubModalAccess().Exists("Voice", "Talked"))
                DataAccessFacade.Instance.GetSubModalAccess().Add("Voice", "Talked", "Talked or not", null);

            if (!DataAccessFacade.Instance.GetModalAccess().Exists("Discrete Posture"))
                DataAccessFacade.Instance.GetModalAccess().Add("Discrete Posture", "Posturas de kinect");
            if (!DataAccessFacade.Instance.GetSubModalAccess().Exists("Discrete Posture", "Seated"))
                DataAccessFacade.Instance.GetSubModalAccess().Add("Discrete Posture", "Seated", "Is Seated", null);
            if (!DataAccessFacade.Instance.GetSubModalAccess().Exists("Discrete Posture", "Hand On Wrist"))
                DataAccessFacade.Instance.GetSubModalAccess().Add("Discrete Posture", "Hand On Wrist", "o<>===<", null);
            if (!DataAccessFacade.Instance.GetSubModalAccess().Exists("Discrete Posture", "Asking Help"))
                DataAccessFacade.Instance.GetSubModalAccess().Add("Discrete Posture", "Asking Help", "I have a question teacher plis", null);
            if (!DataAccessFacade.Instance.GetSubModalAccess().Exists("Discrete Posture", "CrossedArms"))
                DataAccessFacade.Instance.GetSubModalAccess().Add("Discrete Posture", "CrossedArms", "Is crossing his/her arms", null);

            Dictionary<string, int> whiches = new Dictionary<string, int>();
            whiches.Add("Talked", 1);
            whiches.Add("Seated", 2);
            whiches.Add("Hand On Wrist", 3);
            whiches.Add("Asking Help", 1);
            whiches.Add("CrossedArms", 2);

            foreach (var pis in DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene)
            {
                foreach (var m in DataAccessFacade.Instance.GetModalAccess().GetAll())
                {
                    Console.WriteLine($"{m.ModalTypeId} tiene {m.SubModalTypes.Count} submodaltypes");
                    foreach (var s in m.SubModalTypes)
                    {
                        var which = whiches[s.SubModalTypeId];
                        Console.WriteLine($"Which: {which}");
                        int representTypeQuantity = 15;
                        int lastTickGenerated = 0;
                        for (int j = 1; j <= representTypeQuantity; j++)
                        {
                            double doubleData = 2.5;
                            int sceneTicks = (int)DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().Duration.Ticks;
                            int maxTicks = sceneTicks / representTypeQuantity * j;
                            switch (which)
                            {
                                case 1:
                                    DataAccessFacade.Instance.GetEventAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, new TimeSpan(maxTicks), doubleData, -1);
                                    
                                    break;
                                case 2:
                                    int minTicks = lastTickGenerated + 1;
                                    lastTickGenerated = maxTicks;
                                    TimeSpan min = new TimeSpan(minTicks);
                                    TimeSpan max = new TimeSpan(maxTicks);
                                    DataAccessFacade.Instance.GetIntervalAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, min, max, doubleData);
                                    
                                    break;
                                case 3:
                                    DataAccessFacade.Instance.GetTimelessAccess().Add(pis.Person.PersonId, m.ModalTypeId, s.SubModalTypeId, j, doubleData);
                                    
                                    break;
                            }
                        }
                    }
                }
            }

            DataAccessFacade.Instance.GetSceneAccess().SaveOrUpdate(scene);
        }
    }
}
