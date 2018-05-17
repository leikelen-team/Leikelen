using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.API.Module.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Accord.IO;

namespace cl.uv.leikelen.Module.Processing.EEGPrint
{
    public class EegPrintEntryPoint : ProcessingModule, IEegProcessingModule
    {
        //private StreamWriter _myfile;
        public EegPrintEntryPoint()
        {
            //var r = new Random();
            //_myfile = new StreamWriter("salidaEEG"+r.Next()+"12.json");
            Name = "print EEG";
            //_myfile.WriteLine("[");
            //Enable();
        }

        public override Action FunctionAfterStop()
        {
            return null;/*() =>
            {
                _myfile.WriteLine("]");
                _myfile.Flush();
                _myfile.Close();
            };*/
        }

        public EventHandler<EegFrameArrivedEventArgs> EegListener()
        {
            return this.myListener;
        }

        public void myListener(object sender, EegFrameArrivedEventArgs e)
        {
            e.Person = new Data.Model.Person
            {
                Name = e.Person.Name,
                PersonId = e.Person.PersonId,
                TrackingId = e.Person.TrackingId,
                Sex = e.Person.Sex,
                Photo = e.Person.Photo,
                Birthday = e.Person.Birthday,
                PersonInScenes = null
            };
            //e.Person.PersonInScenes = null;
            Console.WriteLine(e.ToJsonString(compress: true, enumsAsStrings: true));
            //_myfile.Write(e.ToJsonString(compress: true, enumsAsStrings: true));
            //_myfile.WriteLine(",");
            //_myfile.Flush();
            //Console.WriteLine(e.Quality);

        }
    }
}
