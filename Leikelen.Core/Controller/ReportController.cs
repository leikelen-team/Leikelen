using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cl.uv.leikelen.Controller
{
    public class ReportController
    {
        public ReportController()
        {

        }

        public async Task StartServer()
        {
        }

        public async Task GenerateSceneReport()
        {
            var datas = View.Widget.HomeTab.TabScene.GetEventsAndIntervals();

            using (var fileStream2 = File.CreateText(@"C:\Users\Erick\Downloads\data.json"))
            {
                var d = new
                {
                    intervals = datas.Item1,
                    events = datas.Item2
                };
                fileStream2.WriteLine(d.ToJsonString());
                //result.Content.CopyTo(fileStream2);
            }

            Console.WriteLine("COOOOOOOOOOOOOOOOOOOOOOOOOOOONECTÓOOOOOOOOOOOOOO");
            
            
            using (var fileStream = File.Create(@"C:\Users\Erick\Downloads\report.pdf"))
            {

            }
        }

        public async Task GeneratePersonReport()
        {

        }

    }
}
