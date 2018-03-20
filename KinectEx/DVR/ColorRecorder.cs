﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace KinectEx.DVR
{
    /// <summary>
    /// public class that provides the services necessary to encode and store
    /// a <c>ColorFrame</c>.
    /// </summary>
    public class ColorRecorder
    {
        private readonly BinaryWriter _writer;
        private bool _isStarted = false;

        private IColorCodec _codec;

        /// <summary>
        /// Gets or sets the Codec used for recording.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot change Codec after recording has started.</exception>
        public IColorCodec Codec
        {
            get { return _codec; }
            set
            {
                if (_isStarted)
                {
                    throw new InvalidOperationException("Cannot change Codec after recording has started.");
                }
                else
                {
                    _codec = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRecorder"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public ColorRecorder(BinaryWriter writer)
        {
            this._writer = writer;
            this._codec = new RawColorCodec();
        }

        /// <summary>
        /// Records a <c>ReplayColorFrame</c>.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public async Task RecordAsync(ReplayColorFrame frame)
        {
            if (_writer.BaseStream == null || _writer.BaseStream.CanWrite == false)
                return;

            _isStarted = true;

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
                        _codec.Width = frame.Width;
                        _codec.Height = frame.Height;
                        await _codec.EncodeAsync(frame.FrameData, dataWriter);

                        // Reset frame data stream
                        dataWriter.Flush();
                        dataStream.Position = 0;

                        // Write FrameSize
                        _writer.Write(dataStream.Length);

                        // Write actual frame data
                        dataStream.CopyTo(_writer.BaseStream);

                        // Write end of frame marker
                        _writer.Write(ReplayFrame.EndOfFrameMarker);
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: Change to log the error
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
