using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using cl.uv.leikelen.Data.Access;
using cl.uv.leikelen.Data.Model;

namespace cl.uv.leikelen.Controller
{
    public class ReportController
    {
        public ReportController()
        {

        }

        public async Task GenerateSceneReport(string outputFilePath)
        {
            //get data
            var datas = View.Widget.HomeTab.TabScene.GetEventsAndIntervals(makeEvents: false);

            //path definitions
            string assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string executableName = "wkhtmltopdf.exe";
            string jsonFile = @"report_templates\scene\dataJson.json";
            string htmlFile = @"report_templates\scene\content.html";

            var scene = DataAccessFacade.Instance.GetSceneInUseAccess().GetScene();
            Scene sc = new Scene
            {
                Name = scene.Name,
                Duration = scene.Duration,
                Place = scene.Place,
                RecordRealDateTime = scene.RecordRealDateTime,
                RecordStartedDateTime = scene.RecordStartedDateTime,
                Description = scene.Description,
                Type = scene.Type,
                NumberOfParticipants = scene.NumberOfParticipants
            };

            //create the json file with the data
            using (var fileStream2 = File.CreateText(jsonFile))
            {
                var d = new
                {
                    scene = new {
                        Duration = sc.Duration.ToString(@"hh\:mm\:ss")
                    },
                    intervals = datas.Item1,
                    events = datas.Item2
                };
                fileStream2.WriteLine(d.ToJsonString());
            }

            //Make and start process
            ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(assemblyPath, executableName))
            {
                Arguments = Path.Combine(assemblyPath, htmlFile) + " " + outputFilePath,
                UseShellExecute = false
            };
            var p = Process.Start(startInfo);
            p.Exited += (sender, e) =>
            {
                Console.WriteLine("Exited with code: "+p.ExitCode);
                File.Delete(jsonFile);
                if (p.ExitCode != 0) //Exit code error
                {
                    throw new Exception(p.StandardError.ReadToEnd());
                }
            };
        }

        public async Task GeneratePersonReport(Data.Model.Person person, string outputFilePath)
        {
            //get data
            var ps = new List<Data.Model.Person>();
            ps.Add(person);
            var datas = View.Widget.HomeTab.TabScene.GetEventsAndIntervals(persons: ps);

            //path definitions
            string assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string executableName = "wkhtmltopdf.exe";
            string jsonFile = @"report_templates\person\dataJson.json";
            string htmlFile = @"report_templates\person\content.html";

            var scene = DataAccessFacade.Instance.GetSceneInUseAccess().GetScene();
            Scene sc = new Scene
            {
                Name = scene.Name,
                Duration = scene.Duration,
                Place = scene.Place,
                RecordRealDateTime = scene.RecordRealDateTime,
                RecordStartedDateTime = scene.RecordStartedDateTime,
                Description = scene.Description,
                Type = scene.Type,
                NumberOfParticipants = scene.NumberOfParticipants
            };

            //create the json file with the data
            using (var fileStream2 = File.CreateText(jsonFile))
            {
                var d = new
                {
                    scene = new
                    {
                        Duration = sc.Duration.ToString(@"hh\:mm\:ss")
                    },
                    intervals = datas.Item1,
                    events = datas.Item2
                };
                fileStream2.WriteLine(d.ToJsonString());
            }

            //Make and start process
            ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(assemblyPath, executableName))
            {
                Arguments = Path.Combine(assemblyPath, htmlFile) + " " + outputFilePath,
                UseShellExecute = false
            };
            var p = Process.Start(startInfo);
            p.Exited += (sender, e) =>
            {
                Console.WriteLine("Exited with code: " + p.ExitCode);
                File.Delete(jsonFile);
                if (p.ExitCode != 0) //Exit code error
                {
                    throw new Exception(p.StandardError.ReadToEnd());
                }
            };
        }

    }
}
