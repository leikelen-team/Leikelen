using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cl.uv.leikelen.API.Module.Input
{
    /// <summary>
    /// Interface for the sensor's player
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Resume the playback.
        /// </summary>
        void Unpause();
        /// <summary>
        /// Opens the data file and plays the playback from the beginning.
        /// </summary>
        void Play();
        /// <summary>
        /// Pauses the playback.
        /// </summary>
        void Pause();
        /// <summary>
        /// Stops the playback.
        /// </summary>
        void Stop();
        /// <summary>
        /// Sends the playback to a specific time.
        /// </summary>
        /// <param name="newTime">The new time.</param>
        void ChangeTime(TimeSpan newTime);
        /// <summary>
        /// Determines whether this instance is playing.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is playing; otherwise, <c>false</c>.
        /// </returns>
        bool IsPlaying();
        /// <summary>
        /// Closes the data file.
        /// </summary>
        void Close();
        /// <summary>
        /// Gets the total duration.
        /// </summary>
        /// <returns>The total duration of the recorded data file, if exists</returns>
        TimeSpan? GetTotalDuration();
        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <returns>
        /// The actual location of the playback, if its playing or paused
        /// </returns>
        TimeSpan? GetLocation();
        /// <summary>
        /// Occurs when playback [finished].
        /// </summary>
        event EventHandler Finished;
        /// <summary>
        /// Occurs when playback's [location changed].
        /// </summary>
        event EventHandler LocationChanged;
    }
}
