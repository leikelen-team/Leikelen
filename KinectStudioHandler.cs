using Microsoft.Kinect.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.VisualizadorMultimodal
{
    class KinectStudioHandler
    {
        public static void RecordClip(string filePath, TimeSpan duration)
        {
            using (KStudioClient client = KStudio.CreateClient())
            {
                client.ConnectToService();

                KStudioEventStreamSelectorCollection streamCollection = new KStudioEventStreamSelectorCollection();
                streamCollection.Add(KStudioEventStreamDataTypeIds.Ir);
                streamCollection.Add(KStudioEventStreamDataTypeIds.Depth);
                streamCollection.Add(KStudioEventStreamDataTypeIds.Body);
                streamCollection.Add(KStudioEventStreamDataTypeIds.BodyIndex);

                using (KStudioRecording recording = client.CreateRecording(filePath, streamCollection))
                {
                    recording.StartTimed(duration);
                    while (recording.State == KStudioRecordingState.Recording)
                    {
                        Thread.Sleep(500);
                    }

                    if (recording.State == KStudioRecordingState.Error)
                    {
                        throw new InvalidOperationException("Error: Recording failed!");
                    }
                }

                client.DisconnectFromService();
            }
        }

        public static void PlaybackClip()
        {
            using (KStudioClient client = KStudio.CreateClient())
            {
                client.ConnectToService();

                string filePath = "C:/Users/elrod_000/Documents/Develop/Kinect/Kinect Studio/Repository/20160531_220653_00.xef";
                using (KStudioPlayback playback = client.CreatePlayback(filePath))
                {
                    //playback.LoopCount = loopCount;
                    playback.Start();

                    while (playback.State == KStudioPlaybackState.Playing)
                    {
                        Thread.Sleep(500);
                    }

                    if (playback.State == KStudioPlaybackState.Error)
                    {
                        throw new InvalidOperationException("Error: Playback failed!");
                    }
                }

                client.DisconnectFromService();
            }
        }
    }
}
