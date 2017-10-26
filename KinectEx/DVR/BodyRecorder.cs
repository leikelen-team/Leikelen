﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KinectEx.DVR
{
    /// <summary>
    /// public class that provides the services necessary to encode and store
    /// a <c>BodyFrame</c>.
    /// </summary>
    public class BodyRecorder
    {
        private readonly BinaryWriter _writer = null;
        private List<CustomBody> _bodies = new List<CustomBody>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BodyRecorder"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public BodyRecorder(BinaryWriter writer)
        {
            this._writer = writer;
        }

        /// <summary>
        /// Records a <c>ReplayBodyFrame</c>.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public async Task RecordAsync(ReplayBodyFrame frame)
        {
            if (_writer.BaseStream == null || _writer.BaseStream.CanWrite == false)
                return;

            await Task.Run(() =>
            {
                try
                {
                    // Header
                    _writer.Write((int)frame.FrameType);
                    _writer.Write(frame.RelativeTime.TotalMilliseconds);

                    // Data
                    using (var dataStream = new MemoryStream())
                    {
                        using (var dataWriter = new BinaryWriter(dataStream))
                        {
                            dataWriter.Write(frame.BodyCount);
                            dataWriter.Write(frame.FloorClipPlane.W);
                            dataWriter.Write(frame.FloorClipPlane.X);
                            dataWriter.Write(frame.FloorClipPlane.Y);
                            dataWriter.Write(frame.FloorClipPlane.Z);

                            int trackedBodyCount = 0;
                            for (var i = 0; i < frame.BodyCount; i++)
                            {
                                if (RecordBody(frame.Bodies[i], dataWriter))
                                    trackedBodyCount++;
                            }
                            
                            // Reset frame data stream
                            dataWriter.Flush();
                            dataStream.Position = 0;

                            // Write FrameSize
                            _writer.Write(dataStream.Length);

                            // Write actual frame data
                            dataStream.CopyTo(_writer.BaseStream);

                            // Write end of frame marker
                            _writer.Write(ReplayFrame.EndOfFrameMarker);

                            System.Diagnostics.Debug.WriteLine("    (recorded {0} tracked bodies)", trackedBodyCount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
        }

        private bool RecordBody(IBody body, BinaryWriter dataWriter)
        {
            dataWriter.Write(body.IsTracked);

            // Skip the rest if it isn't tracked...is a waste since it isn't
            // materially different from a new body object
            if (!body.IsTracked)
                return false;

            dataWriter.Write((int)body.ClippedEdges);
            dataWriter.Write((int)body.HandLeftConfidence);
            dataWriter.Write((int)body.HandLeftState);
            dataWriter.Write((int)body.HandRightConfidence);
            dataWriter.Write((int)body.HandRightState);
            dataWriter.Write(body.IsRestricted);

            dataWriter.Write(body.JointOrientations.Count);
            foreach (var pair in body.JointOrientations)
            {
                dataWriter.Write((int)pair.Key);
                dataWriter.Write((int)pair.Value.JointType);
                dataWriter.Write(pair.Value.Orientation.W);
                dataWriter.Write(pair.Value.Orientation.X);
                dataWriter.Write(pair.Value.Orientation.Y);
                dataWriter.Write(pair.Value.Orientation.Z);
            }

            dataWriter.Write(body.Joints.Count);
            foreach (var pair in body.Joints)
            {
                dataWriter.Write((int)pair.Key);
                dataWriter.Write((int)pair.Value.JointType);
                dataWriter.Write(pair.Value.Position.X);
                dataWriter.Write(pair.Value.Position.Y);
                dataWriter.Write(pair.Value.Position.Z);
                dataWriter.Write(pair.Value.DepthPosition.X);
                dataWriter.Write(pair.Value.DepthPosition.Y);
                dataWriter.Write(pair.Value.ColorPosition.X);
                dataWriter.Write(pair.Value.ColorPosition.Y);
                dataWriter.Write((int)pair.Value.TrackingState);
            }

            dataWriter.Write((float)body.Lean.X);
            dataWriter.Write((float)body.Lean.Y);
            dataWriter.Write((int)body.LeanTrackingState);
            dataWriter.Write(body.TrackingId);
            dataWriter.Write(body.HasMappedDepthPositions);
            dataWriter.Write(body.HasMappedColorPositions);

            return true;
        }
    }
}
