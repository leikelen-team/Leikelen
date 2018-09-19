using cl.uv.leikelen.API.FrameProvider.EEG;
using cl.uv.leikelen.API.Module.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Accord.IO;

/// <summary>
/// Processing module for print the EEG data.
/// </summary>
namespace cl.uv.leikelen.Module.Processing.EEGPrint
{
    /// <summary>
    /// Entry point for a processing module just 
    /// for printing as a json in console the events of EEG
    /// </summary>
    /// <seealso cref="cl.uv.leikelen.API.Module.Processing.ProcessingModule" />
    /// <seealso cref="cl.uv.leikelen.API.FrameProvider.EEG.IEegProcessingModule" />
    public class EegPrintEntryPoint : ProcessingModule, IEegProcessingModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EegPrintEntryPoint"/> class.
        /// </summary>
        public EegPrintEntryPoint()
        {
            Name = "print EEG";
        }

        /// <summary>
        /// Functions called after the recorder stops.
        /// </summary>
        /// <returns></returns>
        public override Action FunctionAfterStop()
        {
            return null;
        }

        /// <summary>
        /// Gets the handler for the EEG frame events.
        /// </summary>
        /// <returns>
        /// the handler for the EEG frame events
        /// </returns>
        public EventHandler<API.FrameProvider.EEG.EegFrameArrivedEventArgs> EegListener()
        {
            return this.myListener;
        }

        /// <summary>
        /// A listener for EEG frame events that just prints as a Json the data received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="API.FrameProvider.EEG.EegFrameArrivedEventArgs"/> instance containing the event data.</param>
        public void myListener(object sender, API.FrameProvider.EEG.EegFrameArrivedEventArgs e)
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
            Console.WriteLine(e.ToJsonString(compress: true, enumsAsStrings: true));
        }
    }
}
