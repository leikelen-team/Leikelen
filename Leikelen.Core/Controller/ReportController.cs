using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;
using cl.uv.leikelen.API.DataAccess;

namespace cl.uv.leikelen.Controller
{
    /// <summary>
    /// Controller to create the reports in PDF
    /// </summary>
    public class ReportController
    {
        /// <summary>
        /// Generates the scene report and saves to the output file path.
        /// </summary>
        /// <param name="outputFilePath">The output file path.</param>
        /// <returns>Asynchronous Task associated</returns>
        public async Task GenerateSceneReport(string outputFilePath)
        {
            //get data
            var datas = View.Widget.HomeTab.TabScene.GetEventsAndIntervals();

            //get all events and intervals
            var intervalList = new Dictionary<string, Dictionary<string, Dictionary<string, List<Interval>>>>();
            var eventList = new Dictionary<string, Dictionary<string, Dictionary<string, List<Event>>>>();
            List<Person> personList = new List<Person>();

            foreach (var pis in DataAccessFacade.Instance.GetSceneInUseAccess().GetScene().PersonsInScene)
            {
                personList.Add(new Person
                {
                    Name = pis.Person.Name,
                    TrackingId = pis.Person.TrackingId,
                    PersonId = pis.Person.PersonId,
                    Birthday = pis.Person.Birthday,
                    Sex = pis.Person.Sex,
                    Photo = pis.Person.Photo,
                    PersonInScenes = null
                });
                if (!intervalList.ContainsKey(pis.Person.Name))
                    intervalList[pis.Person.Name] = new Dictionary<string, Dictionary<string, List<Interval>>>();
                if(!eventList.ContainsKey(pis.Person.Name))
                    eventList[pis.Person.Name] = new Dictionary<string, Dictionary<string, List<Event>>>();
                foreach (var subModal in pis.SubModalType_PersonInScenes)
                {
                    var intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(pis.Person, subModal.ModalTypeId, subModal.SubModalTypeId);
                    if (!intervalList[pis.Person.Name].ContainsKey(subModal.ModalTypeId))
                        intervalList[pis.Person.Name][subModal.ModalTypeId] = new Dictionary<string, List<Interval>>();
                    if (!intervalList[pis.Person.Name][subModal.ModalTypeId].ContainsKey(subModal.SubModalTypeId))
                        intervalList[pis.Person.Name][subModal.ModalTypeId][subModal.SubModalTypeId] = intervals;

                    var events = DataAccessFacade.Instance.GetEventAccess().GetAll(pis.Person, subModal.ModalTypeId, subModal.SubModalTypeId);
                    if (!eventList[pis.Person.Name].ContainsKey(subModal.ModalTypeId))
                        eventList[pis.Person.Name][subModal.ModalTypeId] = new Dictionary<string, List<Event>>();
                    if (!eventList[pis.Person.Name][subModal.ModalTypeId].ContainsKey(subModal.SubModalTypeId))
                        eventList[pis.Person.Name][subModal.ModalTypeId][subModal.SubModalTypeId] = events;
                }
            }

            //path definitions
            string assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string executableName = "wkhtmltopdf.exe";
            string jsonFile = @"report_templates\scene\dataJson.json";
            string htmlFile = @"report_templates\scene\content.html";

            var scene = DataAccessFacade.Instance.GetSceneInUseAccess().GetScene();
            
            //create the json file with the data
            using (var fileStream2 = File.CreateText(jsonFile))
            {
                var d = new
                {
                    persons = personList,
                    scene = new {
                        scene.Duration,
                        scene.Name,
                        scene.Place,
                        scene.Type,
                        scene.Description,
                        scene.RecordRealDateTime,
                        scene.RecordStartedDateTime,
                        scene.NumberOfParticipants
                    },
                    intervals = datas.Item1,
                    events = datas.Item2,
                    intervalList,
                    eventList
                };
                fileStream2.WriteLine(d.ToJsonString());
            }

            //Make and start process
            ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(assemblyPath, executableName))
            {
                Arguments = Path.Combine(assemblyPath, htmlFile) + " --javascript-delay "+ (GeneralSettings.Instance.SecsWaitPdfReport*1000) + " " + outputFilePath,
                UseShellExecute = false
            };
            var p = Process.Start(startInfo);
            p.Exited += (sender, e) =>
            {
                Console.WriteLine("Exited with code: "+p.ExitCode);
                //File.Delete(jsonFile);
                if (p.ExitCode != 0) //Exit code error
                {
                    throw new Exception(p.StandardError.ReadToEnd());
                }
            };
        }

        /// <summary>
        /// Generates the person report.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <param name="outputFilePath">The output file path.</param>
        /// <returns>Asynchronous Task associated</returns>
        public async Task GeneratePersonReport(Data.Model.Person person, string outputFilePath)
        {
            //get data
            var ps = new List<Person>
            {
                person
            };
            var datas = View.Widget.HomeTab.TabScene.GetEventsAndIntervals(persons: ps);

            //get all events and intervals
            var intervalList = new Dictionary<string, Dictionary<string, List<Interval>>>();
            var eventList = new Dictionary<string, Dictionary<string, List<Event>>>();
            foreach (var pis in person.PersonInScenes)
            {
                if(ReferenceEquals(pis.Scene, DataAccessFacade.Instance.GetSceneInUseAccess().GetScene()))
                {
                    foreach(var subModal in pis.SubModalType_PersonInScenes)
                    {
                        var intervals = DataAccessFacade.Instance.GetIntervalAccess().GetAll(pis.Person, subModal.ModalTypeId, subModal.SubModalTypeId);
                        if (!intervalList.ContainsKey(subModal.ModalTypeId))
                            intervalList[subModal.ModalTypeId] = new Dictionary<string, List<Interval>>();
                        if (!intervalList[subModal.ModalTypeId].ContainsKey(subModal.SubModalTypeId))
                            intervalList[subModal.ModalTypeId][subModal.SubModalTypeId] = intervals;
                        
                        var events = DataAccessFacade.Instance.GetEventAccess().GetAll(pis.Person, subModal.ModalTypeId, subModal.SubModalTypeId);
                        if (!eventList.ContainsKey(subModal.ModalTypeId))
                            eventList[subModal.ModalTypeId] = new Dictionary<string, List<Event>>();
                        if (!eventList[subModal.ModalTypeId].ContainsKey(subModal.SubModalTypeId))
                            eventList[subModal.ModalTypeId][subModal.SubModalTypeId] = events;
                        
                    }
                }
            }


            //path definitions
            string assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string executableName = "wkhtmltopdf.exe";
            string jsonFile = @"report_templates\person\dataJson.json";
            string htmlFile = @"report_templates\person\content.html";

            var scene = DataAccessFacade.Instance.GetSceneInUseAccess().GetScene();

            //create the json file with the data
            using (var fileStream2 = File.CreateText(jsonFile))
            {
                var d = new
                {
                    person = new {
                        person.Name,
                        person.TrackingId,
                        person.PersonId,
                        person.Birthday,
                        person.Sex,
                        person.Photo
                    },
                    scene = new
                    {
                        scene.Duration,
                        scene.Name,
                        scene.Place,
                        scene.Type,
                        scene.Description,
                        scene.RecordRealDateTime,
                        scene.RecordStartedDateTime,
                        scene.NumberOfParticipants
                    },
                    intervals = datas.Item1,
                    events = datas.Item2,
                    intervalList,
                    eventList
                };
                fileStream2.WriteLine(d.ToJsonString());
            }

            //Make and start process
            ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(assemblyPath, executableName))
            {
                Arguments = Path.Combine(assemblyPath, htmlFile) + "  --debug-javascript --no-stop-slow-scripts --javascript-delay " + (GeneralSettings.Instance.SecsWaitPdfReport * 1000) + " " + outputFilePath,
                UseShellExecute = false
            };
            var p = Process.Start(startInfo);
            p.Exited += (sender, e) =>
            {
                Console.WriteLine("Exited with code: " + p.ExitCode);
                //File.Delete(jsonFile);
                if (p.ExitCode != 0) //Exit code error
                {
                    throw new Exception(p.StandardError.ReadToEnd());
                }
            };
        }

    }
}
