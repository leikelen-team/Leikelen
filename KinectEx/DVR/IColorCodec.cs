﻿using System.IO;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.UI.Xaml.Media.Imaging;
#else
using System.Windows.Media;
using System.Windows.Media.Imaging;
#endif

namespace KinectEx.DVR
{
    /// <summary>
    /// Classes implementing this interface can be used by a <c>ReplayColorFrame</c>
    /// to (potentially) resize and compress the color frame data.
    /// </summary>
    public interface IColorCodec
    {
        /// <summary>
        /// Unique ID for this <c>IColorCodec</c> instance.
        /// </summary>
        int CodecId { get; }

        /// <summary>
        /// Width of the frame in pixels.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Height of the frame in pixels.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// If changed, resizes the width of an encoded bitmap to the specified
        /// number of pixels. By default, the frame will be encoded using the
        /// same width as the input bitmap.
        /// </summary>
        int OutputWidth { get; set; }

        /// <summary>
        /// If changed, resizes the height of an encoded bitmap to the specified
        /// number of pixels. By default, the frame will be encoded using the
        /// same height as the input bitmap.
        /// </summary>
        int OutputHeight { get; set; }

#if !NETFX_CORE
        /// <summary>
        /// Gets the pixel format of the last image decoded.
        /// </summary>
        PixelFormat PixelFormat { get; }
#endif

        /// <summary>
        /// Encodes the specified bitmap data and outputs it to the specified
        /// <c>BinaryWriter</c>. Bitmap data should be in BGRA format.
        /// For public use only.
        /// </summary>
        Task EncodeAsync(byte[] bytes, BinaryWriter writer);

        /// <summary>
        /// Reads the codec-specific header information from the supplied
        /// <c>BinaryReader</c> and writes it to the supplied <c>ReplayFrame</c>.
        /// For public use only.
        /// </summary>
        void ReadHeader(BinaryReader reader, ReplayFrame frame);

        /// <summary>
        /// Decodes the supplied encoded bitmap data into an array of pixels.
        /// For public use only.
        /// </summary>
        Task<byte[]> DecodeAsync(byte[] encodedBytes);
    }
}
