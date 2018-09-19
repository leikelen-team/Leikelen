using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// API for Input modules.
/// </summary>
namespace cl.uv.leikelen.API.Module.Input
{
    /// <summary>
    /// Interface for the sensor's monitor
    /// </summary>
    public interface IMonitor
    {
        /// <summary>
        /// Occurs when current sensor's [status changed].
        /// </summary>
        event EventHandler StatusChanged;
        /// <summary>
        /// Determines whether this instance is recording.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is recording; otherwise, <c>false</c>.
        /// </returns>
        bool IsRecording();
        /// <summary>
        /// Gets the current sensor status.
        /// </summary>
        /// <returns>Current sensor status</returns>
        API.Module.Input.InputStatus GetStatus();
        /// <summary>
        /// Opens the sensor for start monitoring and get data.
        /// </summary>
        /// <returns></returns>
        Task Open();
        /// <summary>
        /// Closes the sensor.
        /// </summary>
        /// <returns></returns>
        Task Close();
        /// <summary>
        /// Starts the recording. 
        /// Start feeding data to associated <see cref="Processing.ProcessingModule"/>, 
        /// and save the data to a file in current scene's directory.
        /// </summary>
        /// <returns></returns>
        Task StartRecording();
        /// <summary>
        /// Stops the recording.
        /// Stop feeding data to associated <see cref="Processing.ProcessingModule"/>
        /// Stop and closes the data file.
        /// </summary>
        /// <returns></returns>
        Task StopRecording();
        /// <summary>
        /// Opens the port if its needed.
        /// </summary>
        /// <param name="portName">Name of the COM port.</param>
        /// <returns></returns>
        Task OpenPort(string portName);
    }
}
