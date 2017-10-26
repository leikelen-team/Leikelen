﻿using System;
using System.IO;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
#else
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif

namespace KinectEx.DVR
{
    /// <summary>
    /// An <c>IColorCodec</c> that performs compresses bitmaps using the JPEG
    /// standard. Choosing this codec when creating a <c>KinectRecorder</c>
    /// effectively saves color frames as an MJPEG.
    /// </summary>
    public class JpegColorCodec : IColorCodec
    {
        private int _outputWidth = int.MinValue;
        private int _outputHeight = int.MinValue;
        private int _jpegQuality = 70;

        /// <summary>
        /// Unique ID for this <c>IColorCodec</c> instance.
        /// </summary>
        public int CodecId { get { return 1; } }

        /// <summary>
        /// Width of the frame in pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the frame in pixels.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// If changed, resizes the width of an encoded bitmap to the specified
        /// number of pixels. By default, the frame will be encoded using the
        /// same width as the input bitmap.
        /// </summary>
        public int OutputWidth
        {
            get { return _outputWidth == int.MinValue ? Width : _outputWidth; }
            set { _outputWidth = value; }
        }

        /// <summary>
        /// If changed, resizes the height of an encoded bitmap to the specified
        /// number of pixels. By default, the frame will be encoded using the
        /// same height as the input bitmap.
        /// </summary>
        public int OutputHeight
        {
            get { return _outputHeight == int.MinValue ? Height : _outputHeight; }
            set { _outputHeight = value; }
        }

        /// <summary>
        /// Gets or sets the compression quality factor used to encode the individual
        /// JPEG bitmaps. Should be between 1 (smallest / lowest quality) and 100
        /// (largest / highest quality). Default is 70.
        /// </summary>
        public int JpegQuality
        {
            get { return _jpegQuality; }
            set { _jpegQuality = value; }
        }

#if !NETFX_CORE
        /// <summary>
        /// Gets the pixel format of the last image decoded.
        /// </summary>
        public PixelFormat PixelFormat { get; private set; }
#endif

        /// <summary>
        /// Encodes the specified bitmap data and outputs it to the specified
        /// <c>BinaryWriter</c>. Bitmap data should be in BGRA format.
        /// For public use only.
        /// </summary>
        public async Task EncodeAsync(byte[] bytes, BinaryWriter writer)
        {
#if NETFX_CORE
            using (var jpegStream = new InMemoryRandomAccessStream())
            {
                var propertySet = new BitmapPropertySet();
                var qualityValue = new BitmapTypedValue(this.JpegQuality / 100.0, PropertyType.Single);
                propertySet.Add("ImageQuality", qualityValue);

                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, jpegStream, propertySet);
                if (this.Width != this.OutputWidth)
                {
                    encoder.BitmapTransform.ScaledWidth = (uint)this.OutputWidth;
                    encoder.BitmapTransform.ScaledHeight = (uint)this.OutputHeight;
                }

                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    (uint)this.Width,
                    (uint)this.Height,
                    96,
                    96,
                    bytes);
                await encoder.FlushAsync();

                if (writer.BaseStream == null || writer.BaseStream.CanWrite == false)
                    return;

                // Header
                writer.Write(this.OutputWidth);
                writer.Write(this.OutputHeight);
                writer.Write((int)jpegStream.Size);

                // Data
                jpegStream.AsStreamForRead().CopyTo(writer.BaseStream);
            }
#else
            await Task.Run(() =>
            {
                var format = PixelFormats.Bgra32;
                int stride = (int)this.Width * format.BitsPerPixel / 8;
                var bmp = BitmapSource.Create(
                    this.Width,
                    this.Height,
                    96.0,
                    96.0,
                    format,
                    null,
                    bytes,
                    stride);
                BitmapFrame frame;
                if (this.Width != this.OutputWidth || this.Height != this.OutputHeight)
                {
                    var transform = new ScaleTransform((double)this.OutputHeight / this.Height, (double)this.OutputHeight / this.Height);
                    var scaledbmp = new TransformedBitmap(bmp, transform);
                    frame = BitmapFrame.Create(scaledbmp);
                }
                else
                {
                    frame = BitmapFrame.Create(bmp);
                }

                var encoder = new JpegBitmapEncoder()
                {
                    QualityLevel = this.JpegQuality
                };
                encoder.Frames.Add(frame);
                using (var jpegStream = new MemoryStream())
                {
                    encoder.Save(jpegStream);

                    if (writer.BaseStream == null || writer.BaseStream.CanWrite == false)
                        return;

                    // Header
                    writer.Write(this.OutputWidth);
                    writer.Write(this.OutputHeight);
                    writer.Write((int)jpegStream.Length);

                    // Data
                    jpegStream.Position = 0;
                    jpegStream.CopyTo(writer.BaseStream);
                }
            });
#endif
        }

        /// <summary>
        /// Reads the codec-specific header information from the supplied
        /// <c>BinaryReader</c> and writes it to the supplied <c>ReplayFrame</c>.
        /// For public use only.
        /// </summary>
        public void ReadHeader(BinaryReader reader, ReplayFrame frame)
        {
            var colorFrame = frame as ReplayColorFrame;

            if (colorFrame == null)
                throw new InvalidOperationException("Must be a ReplayColorFrame");

            colorFrame.Width = reader.ReadInt32();
            colorFrame.Height = reader.ReadInt32();
            colorFrame.FrameDataSize = reader.ReadInt32();
        }

        /// <summary>
        /// Decodes the supplied encoded bitmap data into an array of pixels.
        /// For public use only.
        /// </summary>
        public async Task<byte[]> DecodeAsync(byte[] encodedBytes)
        {
#if NETFX_CORE
            using (var ras = new InMemoryRandomAccessStream())
            {
                await ras.AsStream().WriteAsync(encodedBytes, 0, encodedBytes.Length);
                ras.Seek(0);
                var dec = await BitmapDecoder.CreateAsync(BitmapDecoder.JpegDecoderId, ras);
                var pixelDataProvider = await dec.GetPixelDataAsync();
                return pixelDataProvider.DetachPixelData();
            }
#else
            using (var str = new MemoryStream())
            {
                str.Write(encodedBytes, 0, encodedBytes.Length);
                str.Position = 0;
                var dec = new JpegBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                var frame = dec.Frames[0];
                this.PixelFormat = frame.Format;
                var bpp = frame.Format.BitsPerPixel / 8;
                var stride = bpp * frame.PixelWidth;
                var size = stride * frame.PixelHeight;
                var output = new byte[size];
                frame.CopyPixels(output, stride, 0);
                return await Task.FromResult(output);
            }
#endif
        }
    }
}
